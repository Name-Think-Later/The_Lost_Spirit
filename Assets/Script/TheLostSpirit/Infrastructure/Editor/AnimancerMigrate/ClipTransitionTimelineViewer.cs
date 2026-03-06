using System.Collections.Generic;
using System.Linq;
using Animancer;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

// ================================================================
// [AnimancerMigrate] ClipTransitionTimelineViewer
// 用途：讀取 ClipTransition.Events 並顯示可互動的時間線
// 操作：
//   - 左鍵拖動軌道 → 移動 Playhead
//   - 右鍵點軌道   → 在當前位置新增事件
//   - 左鍵拖動標記 → 移動事件
//   - 右鍵點標記   → 刪除事件
//   - Add 按鈕     → 在 Playhead 位置新增事件
// ================================================================

namespace TheLostSpirit.Infrastructure.Editor.AnimancerMigrate
{
    public class ClipTransitionTimelineViewer : VisualElement, ITimelineViewerController
    {
        // ── 子元件 ─────────────────────────────────────────────
        readonly VisualElement _track; // 可互動的軌道區域
        readonly VisualElement _clipBar;
        readonly VisualElement _rulerLayer;
        readonly VisualElement _markerContainer;
        readonly VisualElement _playHeadLine;
        readonly Label         _noClipLabel;
        readonly Label         _timeLabel; // 顯示 Playhead 時間
        readonly Button        _btnAdd;

        // ── 狀態 ───────────────────────────────────────────────
        AnimationClip      _clip;
        float              _clipLength;
        float              _currentNormalized; // Playhead 位置（0~1）
        SerializedProperty _transitionProp;    // 寫回用（nullable）

        // ── Animancer Events SP 路徑 ────────────────────────────
        // transitionProp → "_Events" → "_NormalizedTimes" (float[])
        // 最後一格永遠是 End Event（NaN = 預設）

        // ================================================================

        public ClipTransitionTimelineViewer() {
            StyleRoot();

            // Toolbar
            var toolbar = MakeToolbar(out _btnAdd, out _timeLabel);
            Add(toolbar);

            // Track（持有 ClipBar、Ruler、Markers、Playhead）
            _track = MakeTrack();
            Add(_track);

            _clipBar         = MakeClipBar();
            _rulerLayer      = MakeLayer();
            _markerContainer = MakeLayer();
            var cursorLayer = MakeCursorLayer(out _playHeadLine);
            _noClipLabel = MakeNoClipLabel();

            _track.Add(_clipBar);
            _track.Add(_rulerLayer);
            _track.Add(_markerContainer);
            _track.Add(cursorLayer);
            _track.Add(_noClipLabel);

            // Scrubber（左鍵拖 playhead，右鍵新增事件）
            _track.AddManipulator(new TimelineScrubberManipulator(this));

            SetNoClip();
        }

        // ================================================================
        // Public API
        // ================================================================

        /// <summary>傳入 ClipTransition 的 SerializedProperty，讀取並顯示（可編輯）。</summary>
        public void LoadFromSerializedProperty(SerializedProperty transitionProperty) {
            _transitionProp = transitionProperty;

            if (transitionProperty == null) {
                SetNoClip();

                return;
            }

            var clipProp = transitionProperty.FindPropertyRelative("_Clip");
            _clip = clipProp?.objectReferenceValue as AnimationClip;

            if (_clip == null) {
                SetNoClip();

                return;
            }

            _clipLength = _clip.length;
            Reload();
        }

        /// <summary>直接傳入 ClipTransition 實例（唯讀，無法寫回）。</summary>
        public void LoadFromTransition(ClipTransition transition) {
            _transitionProp = null;

            if (transition == null || transition.Clip == null) {
                SetNoClip();

                return;
            }

            _clip       = transition.Clip;
            _clipLength = _clip.length;

            var    normalEvents = new List<float>();
            float? endEventTime = null;
            var    events       = transition.Events;

            if (events != null) {
                for (int i = 0; i < events.Count; i++)
                    normalEvents.Add(events[i].normalizedTime);

                var endNT = events.NormalizedEndTime;
                if (!float.IsNaN(endNT))
                    endEventTime = endNT;
            }

            ShowClip(normalEvents, endEventTime);
            _btnAdd.SetEnabled(false); // 無 SP 時禁止新增
        }

        /// <summary>移動 Playhead 到指定 normalizedTime（0~1）。</summary>
        public void SetPlayHeadNormalized(float n) {
            _currentNormalized       = Mathf.Clamp01(n);
            _playHeadLine.style.left = Length.Percent(_currentNormalized * 100f);
            _timeLabel.text          = FormatTime(_currentNormalized);
        }

        // ================================================================
        // ITimelineViewerController
        // ================================================================

        public void OnScrub(float normalizedRatio) {
            var snapped = SnapToFrame(normalizedRatio);
            SetPlayHeadNormalized(snapped);
        }

        public void OnRequestAddEvent(float normalizedRatio) {
            if (_transitionProp == null || _clip == null) return;

            var snapped = SnapToFrame(normalizedRatio);
            AddEventAt(snapped);
        }

        public void OnMoveEvent(int eventIndex, float normalizedRatio) {
            if (_transitionProp == null || _clip == null) return;

            var snapped = SnapToFrame(normalizedRatio);
            MoveEventAt(eventIndex, snapped);
        }

        public void OnRequestDeleteEvent(int eventIndex) {
            if (_transitionProp == null || _clip == null) return;

            DeleteEventAt(eventIndex);
        }

        // ================================================================
        // Private: Data Reload
        // ================================================================

        void Reload() {
            if (_transitionProp == null || _clip == null) return;

            var eventsProp = _transitionProp.FindPropertyRelative("_Events");
            ReadNormalizedTimes(eventsProp, out var normalEvents, out var endEventTime);
            ShowClip(normalEvents, endEventTime);
            _btnAdd.SetEnabled(true);
        }

        // ================================================================
        // Private: SerializedProperty 寫入
        // ================================================================

        /// <summary>新增事件到 _Events._NormalizedTimes（保持排序，保留 End Event）。</summary>
        void AddEventAt(float normalizedTime) {
            var timesProp = GetNormalizedTimesProp();

            if (timesProp == null) return;

            // 取得現有所有時間（含 End Event 位於最後）
            var times = ReadRawTimes(timesProp);

            // 確認不重複
            if (times.Take(times.Count - 1).Any(t => Mathf.Approximately(t, normalizedTime))) {
                Debug.LogWarning($"[ClipTransitionViewer] 事件 normalizedTime={normalizedTime:F4} 已存在，跳過新增。");

                return;
            }

            // 插入新時間（End Event 保持最後）
            float endEvt = times.Count > 0 ? times[times.Count - 1] : float.NaN;
            var   normal = times.Take(times.Count - 1).ToList();
            normal.Add(normalizedTime);
            normal.Sort();
            normal.Add(endEvt);

            WriteRawTimes(timesProp, normal);
            Reload();
        }

        /// <summary>移動指定 index 的事件到新位置。</summary>
        void MoveEventAt(int index, float normalizedTime) {
            var timesProp = GetNormalizedTimesProp();

            if (timesProp == null) return;

            var times = ReadRawTimes(timesProp);

            // index 對應 normal events（不含最後的 End Event）
            int normalCount = times.Count - 1;

            if (index < 0 || index >= normalCount) return;

            // 確認不和其他事件衝突
            for (int i = 0; i < normalCount; i++) {
                if (i != index && Mathf.Approximately(times[i], normalizedTime)) {
                    Debug.LogWarning($"[ClipTransitionViewer] 位置 {normalizedTime:F4} 已有事件，移動取消。");
                    Reload(); // 回滾視覺

                    return;
                }
            }

            float endEvt = times[times.Count - 1];
            var   normal = times.Take(normalCount).ToList();
            normal[index] = normalizedTime;
            normal.Sort();
            normal.Add(endEvt);

            WriteRawTimes(timesProp, normal);
            Reload();
        }

        /// <summary>刪除指定 index 的事件。</summary>
        void DeleteEventAt(int index) {
            var timesProp = GetNormalizedTimesProp();

            if (timesProp == null) return;

            var times       = ReadRawTimes(timesProp);
            int normalCount = times.Count - 1;

            if (index < 0 || index >= normalCount) return;

            float endEvt = times[times.Count - 1];
            var   normal = times.Take(normalCount).ToList();
            normal.RemoveAt(index);
            normal.Add(endEvt);

            WriteRawTimes(timesProp, normal);
            Reload();
        }

        SerializedProperty GetNormalizedTimesProp() {
            if (_transitionProp == null) return null;
            var eventsProp = _transitionProp.FindPropertyRelative("_Events");

            return eventsProp?.FindPropertyRelative("_NormalizedTimes");
        }

        static List<float> ReadRawTimes(SerializedProperty timesProp) {
            var list = new List<float>();

            if (timesProp == null || !timesProp.isArray) return list;
            for (int i = 0; i < timesProp.arraySize; i++)
                list.Add(timesProp.GetArrayElementAtIndex(i).floatValue);

            return list;
        }

        static void WriteRawTimes(SerializedProperty timesProp, List<float> times) {
            Undo.RecordObject(timesProp.serializedObject.targetObject, "Modify Animancer Events");
            timesProp.arraySize = times.Count;
            for (int i = 0; i < times.Count; i++)
                timesProp.GetArrayElementAtIndex(i).floatValue = times[i];
            timesProp.serializedObject.ApplyModifiedProperties();
        }

        // ================================================================
        // Private: Read Events SP (read-only)
        // ================================================================

        static void ReadNormalizedTimes(
            SerializedProperty eventsProp,
            out List<float>    normalEvents,
            out float?         endEventTime
        ) {
            normalEvents = new List<float>();
            endEventTime = null;

            if (eventsProp == null) return;

            var timesProp = eventsProp.FindPropertyRelative("_NormalizedTimes");

            if (timesProp == null || !timesProp.isArray || timesProp.arraySize == 0)
                return;

            int lastIndex = timesProp.arraySize - 1;

            for (int i = 0; i < timesProp.arraySize; i++) {
                float v = timesProp.GetArrayElementAtIndex(i).floatValue;

                if (i == lastIndex) {
                    if (!float.IsNaN(v))
                        endEventTime = v;
                }
                else {
                    normalEvents.Add(v);
                }
            }
        }

        // ================================================================
        // Private: UI
        // ================================================================

        void ShowClip(List<float> normalEvents, float? endEventTime) {
            _clipBar.style.display         = DisplayStyle.Flex;
            _rulerLayer.style.display      = DisplayStyle.Flex;
            _markerContainer.style.display = DisplayStyle.Flex;
            _noClipLabel.style.display     = DisplayStyle.None;

            DrawRuler();
            DrawMarkers(normalEvents, endEventTime);
            SetPlayHeadNormalized(_currentNormalized);
        }

        void SetNoClip() {
            _clip           = null;
            _clipLength     = 0f;
            _transitionProp = null;

            _clipBar.style.display         = DisplayStyle.None;
            _rulerLayer.style.display      = DisplayStyle.None;
            _markerContainer.style.display = DisplayStyle.None;
            _noClipLabel.style.display     = DisplayStyle.Flex;
            _timeLabel.text                = "--";
            _btnAdd.SetEnabled(false);
        }

        void DrawRuler() {
            _rulerLayer.Clear();

            if (_clipLength <= 0f) return;

            var step               = _clipLength < 2f ? 0.1f : _clipLength < 4f ? 0.5f : 1f;
            var count              = Mathf.CeilToInt(_clipLength / step);
            if (count > 200) count = 200;

            for (int i = 0; i <= count; i++) {
                var t = i * step;

                if (t > _clipLength) break;

                var p = t / _clipLength * 100f;

                _rulerLayer.Add(new VisualElement {
                    style = {
                        position        = Position.Absolute,
                        left            = Length.Percent(p),
                        top             = 0,
                        width           = 1,
                        height          = 6,
                        backgroundColor = Styles.RulerTick
                    }
                });
                _rulerLayer.Add(new Label(t.ToString("0.0")) {
                    style = {
                        position = Position.Absolute,
                        left     = Length.Percent(p),
                        top      = 8,
                        fontSize = 9,
                        color    = Styles.RulerText
                    }
                });
            }
        }

        void DrawMarkers(List<float> normalEvents, float? endEventTime) {
            _markerContainer.Clear();

            var frameRate = _clip ? _clip.frameRate : 60f;

            // ── 普通事件（金黃色圓形，可拖動）─────────────────────
            for (int i = 0; i < normalEvents.Count; i++) {
                var n       = Mathf.Clamp01(normalEvents[i]);
                var timeSec = n * _clipLength;
                var frame   = Mathf.RoundToInt(timeSec * frameRate);

                var marker = new VisualElement {
                    tooltip     = $"Event [{i}]  f{frame} ({n:F3})\n左鍵拖動移動 | 右鍵刪除",
                    pickingMode = PickingMode.Position,
                    style = {
                        position                = Position.Absolute,
                        left                    = Length.Percent(n * 100f),
                        top                     = Layout.MarkerTop,
                        width                   = Layout.MarkerSize,
                        height                  = Layout.MarkerSize,
                        marginLeft              = -Layout.MarkerSize * 0.5f,
                        backgroundColor         = Styles.MarkerColor,
                        borderTopLeftRadius     = new StyleLength(new Length(50, LengthUnit.Percent)),
                        borderTopRightRadius    = new StyleLength(new Length(50, LengthUnit.Percent)),
                        borderBottomLeftRadius  = new StyleLength(new Length(50, LengthUnit.Percent)),
                        borderBottomRightRadius = new StyleLength(new Length(50, LengthUnit.Percent))
                    }
                };

                marker.Add(new Label((i + 1).ToString()) {
                    pickingMode = PickingMode.Ignore,
                    style = {
                        position                = Position.Absolute,
                        left                    = 0, right = 0, top = 0, bottom = 0,
                        unityTextAlign          = TextAnchor.MiddleCenter,
                        fontSize                = 8,
                        color                   = Color.white,
                        unityFontStyleAndWeight = FontStyle.Bold
                    }
                });

                // 讓 Marker 可拖動（傳入 _track 作為寬度計算基準）
                if (_transitionProp != null)
                    marker.AddManipulator(new EventMarkerManipulator(this, _track, i));

                _markerContainer.Add(marker);
            }

            // ── End Event（青綠色垂直線，唯讀）───────────────────
            if (endEventTime.HasValue) {
                var n       = Mathf.Clamp01(endEventTime.Value);
                var timeSec = n * _clipLength;
                var frame   = Mathf.RoundToInt(timeSec * frameRate);

                var endLine = new VisualElement {
                    tooltip     = $"End Event  f{frame} ({n:F3})",
                    pickingMode = PickingMode.Position,
                    style = {
                        position        = Position.Absolute,
                        left            = Length.Percent(n * 100f),
                        top             = 0,
                        bottom          = 0,
                        width           = 2,
                        marginLeft      = -1,
                        backgroundColor = Styles.EndEventColor
                    }
                };

                endLine.Add(new Label("END") {
                    pickingMode = PickingMode.Ignore,
                    style = {
                        position                = Position.Absolute,
                        top                     = -14,
                        left                    = -10,
                        fontSize                = 7,
                        color                   = Styles.EndEventColor,
                        unityFontStyleAndWeight = FontStyle.Bold
                    }
                });

                _markerContainer.Add(endLine);
            }
        }

        // ================================================================
        // Private: Helpers
        // ================================================================

        /// <summary>Snap normalizedTime 到最近的 frame。</summary>
        float SnapToFrame(float normalizedRatio) {
            if (_clip == null || _clipLength <= 0f) return normalizedRatio;
            var frameRate   = _clip.frameRate;
            var timeSec     = normalizedRatio * _clipLength;
            var frame       = Mathf.RoundToInt(timeSec * frameRate);
            var snappedTime = frame / frameRate;

            return Mathf.Clamp01(snappedTime / _clipLength);
        }

        string FormatTime(float normalizedRatio) {
            if (_clip == null) return "--";
            var timeSec = normalizedRatio * _clipLength;
            var frame   = Mathf.RoundToInt(timeSec * _clip.frameRate);

            return $"f{frame}  {timeSec:F2}s";
        }

        // ================================================================
        // Private: VisualElement Factories
        // ================================================================

        void StyleRoot() {
            style.marginTop    = 4;
            style.marginBottom = 4;
        }

        VisualElement MakeToolbar(out Button btnAdd, out Label timeLabel) {
            var bar = new VisualElement {
                style = {
                    flexDirection  = FlexDirection.Row,
                    alignItems     = Align.Center,
                    justifyContent = Justify.SpaceBetween,
                    marginBottom   = 2
                }
            };

            timeLabel = new Label("--") {
                style = {
                    fontSize = 10,
                    color    = Styles.InfoText,
                    flexGrow = 1
                }
            };

            btnAdd = new Button(() => OnRequestAddEvent(_currentNormalized)) {
                text    = "+ Add Event",
                tooltip = "在 Playhead 位置新增 Animancer Event",
                style = {
                    fontSize        = 10,
                    height          = 18,
                    marginLeft      = 4,
                    backgroundColor = Styles.AddButtonBg,
                    color           = Color.white
                }
            };

            bar.Add(timeLabel);
            bar.Add(btnAdd);

            return bar;
        }

        VisualElement MakeTrack() {
            return new VisualElement {
                style = {
                    height          = Layout.TrackHeight,
                    marginTop       = 2,
                    marginBottom    = 4,
                    backgroundColor = Styles.TrackBg,
                    borderTopWidth  = 1, borderBottomWidth             = 1,
                    borderLeftWidth = 1, borderRightWidth              = 1,
                    borderTopColor  = Styles.Border, borderBottomColor = Styles.Border,
                    borderLeftColor = Styles.Border, borderRightColor  = Styles.Border,
                    overflow        = Overflow.Visible
                }
            };
        }

        VisualElement MakeClipBar() {
            return new VisualElement {
                style = {
                    position        = Position.Absolute,
                    left            = 0,
                    right           = 0,
                    top             = 0,
                    height          = Layout.ClipBarHeight,
                    backgroundColor = Styles.ClipBar
                }
            };
        }

        VisualElement MakeLayer() {
            return new VisualElement {
                style = {
                    position        = Position.Absolute,
                    left            = 0,
                    right           = 0,
                    top             = 0,
                    bottom          = 0,
                    backgroundColor = Color.clear
                }
            };
        }

        VisualElement MakeCursorLayer(out VisualElement playHeadLine) {
            var layer = new VisualElement {
                style       = { position = Position.Absolute, left = 0, right = 0, top = 0, bottom = 0 },
                pickingMode = PickingMode.Ignore
            };

            playHeadLine = new VisualElement {
                style = {
                    position        = Position.Absolute,
                    width           = 1,
                    top             = 0,
                    bottom          = 0,
                    backgroundColor = Styles.PlayHead,
                    marginLeft      = -0.5f,
                    left            = 0
                }
            };

            layer.Add(playHeadLine);

            return layer;
        }

        Label MakeNoClipLabel() {
            return new Label("No ClipTransition / Clip Assigned") {
                style = {
                    color                   = Color.gray,
                    unityFontStyleAndWeight = FontStyle.Italic,
                    unityTextAlign          = TextAnchor.MiddleCenter,
                    position                = Position.Absolute,
                    left                    = 0,
                    right                   = 0,
                    top                     = 0,
                    bottom                  = 0
                }
            };
        }

        // ================================================================
        // Constants
        // ================================================================

        static class Layout
        {
            public const float TrackHeight   = 45f;
            public const float ClipBarHeight = 20f;
            public const float MarkerTop     = -3f;
            public const float MarkerSize    = 14f;
        }

        static class Styles
        {
            public static readonly Color TrackBg       = new Color(0.15f, 0.15f, 0.15f);
            public static readonly Color ClipBar       = new Color(0.25f, 0.38f, 0.55f);
            public static readonly Color Border        = new Color(0.1f, 0.1f, 0.1f);
            public static readonly Color MarkerColor   = new Color(1f, 0.75f, 0.2f);  // 金黃色
            public static readonly Color EndEventColor = new Color(0.3f, 0.9f, 0.8f); // 青綠色
            public static readonly Color RulerTick     = Color.gray;
            public static readonly Color RulerText     = Color.gray;
            public static readonly Color PlayHead      = new Color(1f, 1f, 1f, 0.9f);
            public static readonly Color InfoText      = new Color(0.7f, 0.7f, 0.7f);
            public static readonly Color AddButtonBg   = new Color(0.2f, 0.5f, 0.3f);
        }
    }
}