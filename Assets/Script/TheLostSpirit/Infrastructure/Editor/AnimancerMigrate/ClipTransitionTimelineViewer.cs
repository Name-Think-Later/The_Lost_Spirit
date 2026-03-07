using System;
using System.Collections.Generic;
using System.Linq;
using Animancer;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

// ================================================================
// [AnimancerMigrate] ClipTransitionTimelineViewer
// 操作：
//   - 左鍵拖動軌道   → 移動 Playhead（snap 到 frame）
//   - 右鍵點軌道     → 在當前位置新增事件
//   - +Add Event 按鈕 → 在 Playhead 位置新增事件
//   - 左鍵點擊 Marker → 選取，觸發 onMarkerSelected
//   - 左鍵拖動 Marker → 移動事件（同步重排 EventData）
//   - 右鍵點 Marker   → 刪除事件（同步刪除 EventData）
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
        readonly Label _noClipLabel;
        readonly Label _timeLabel; // 顯示 Playhead 時間
        readonly Button _btnAdd;

        // ── 狀態 ───────────────────────────────────────────────
        AnimationClip _clip;
        float _clipLength;
        float _currentNormalized; // Playhead 位置（0~1）
        SerializedProperty _transitionProp;    // ClipTransition SP（nullable）
        SerializedProperty _eventDataListProp; // eventDataList SP（nullable）
        int _selectedIndex = -1;

        // ── 拖曳保護 ───────────────────────────────────────────
        bool _suppressReload; // true 時拖曳中，Reload 暫停以保護 DOM
        bool _pendingReload;  // 拖曳期間有 Reload 請求，EndDrag 後執行

        // ── Callbacks ──────────────────────────────────────────
        /// <summary>點擊 Marker 時觸發，參數為 event index（-1 = 取消選取）。</summary>
        public event Action<int> onMarkerSelected;

        /// <summary>Playhead 移動時觸發，參數為時間（秒）。用於外部驅動 AnimationMode 預覽。</summary>
        public event Action<float> onTimeScrubbed;

        /// <summary>Viewer 從 Panel 分離時觸發（例如 Inspector 關閉），通知外部停止 AnimationMode。</summary>
        public event Action onDetached;

        // ================================================================

        public ClipTransitionTimelineViewer()
        {
            StyleRoot();

            var toolbar = MakeToolbar(out _btnAdd, out _timeLabel);
            Add(toolbar);

            _track = MakeTrack();
            Add(_track);

            // 分離時通知外部停止 AnimationMode
            RegisterCallback<DetachFromPanelEvent>(_ => onDetached?.Invoke());

            _clipBar = MakeClipBar();
            _rulerLayer = MakeLayer();
            _markerContainer = MakeLayer();
            var cursorLayer = MakeCursorLayer(out _playHeadLine);
            _noClipLabel = MakeNoClipLabel();

            _track.Add(_clipBar);
            _track.Add(_rulerLayer);
            _track.Add(_markerContainer);
            _track.Add(cursorLayer);
            _track.Add(_noClipLabel);

            _track.AddManipulator(new TimelineScrubberManipulator(this));

            SetNoClip();
        }

        // ================================================================
        // Public API
        // ================================================================

        /// <summary>傳入 ClipTransition 與 eventDataList 的 SerializedProperty，讀取並顯示（可編輯）。</summary>
        public void LoadFromSerializedProperty(
            SerializedProperty transitionProperty,
            SerializedProperty eventDataListProperty = null
        )
        {
            _transitionProp = transitionProperty;
            _eventDataListProp = eventDataListProperty;

            if (transitionProperty == null)
            {
                _selectedIndex = -1;
                SetNoClip();

                return;
            }

            var clipProp = transitionProperty.FindPropertyRelative("_Clip");
            var newClip = clipProp?.objectReferenceValue as AnimationClip;

            // Clip 變更才重置選取狀態；僅資料更新時保留當前選取
            if (newClip != _clip)
                _selectedIndex = -1;

            _clip = newClip;

            if (_clip == null)
            {
                SetNoClip();

                return;
            }

            _clipLength = _clip.length;
            Reload();
        }

        /// <summary>直接傳入 ClipTransition 實例（唯讀，無法寫回）。</summary>
        public void LoadFromTransition(ClipTransition transition)
        {
            _transitionProp = null;
            _eventDataListProp = null;
            _selectedIndex = -1;

            if (transition == null || transition.Clip == null)
            {
                SetNoClip();

                return;
            }

            _clip = transition.Clip;
            _clipLength = _clip.length;

            var normalEvents = new List<float>();
            float? endEventTime = null;
            var events = transition.Events;

            if (events != null)
            {
                for (int i = 0; i < events.Count; i++)
                    normalEvents.Add(events[i].normalizedTime);

                var endNT = events.NormalizedEndTime;
                if (!float.IsNaN(endNT))
                    endEventTime = endNT;
            }

            ShowClip(normalEvents, endEventTime);
            _btnAdd.SetEnabled(false);
        }

        /// <summary>移動 Playhead 到指定 normalizedTime（0~1）。</summary>
        public void SetPlayHeadNormalized(float n)
        {
            _currentNormalized = Mathf.Clamp01(n);
            _playHeadLine.style.left = Length.Percent(_currentNormalized * 100f);
            _timeLabel.text = FormatTime(_currentNormalized);
            onTimeScrubbed?.Invoke(_currentNormalized * _clipLength);
        }

        /// <summary>程式化選取指定 index 的 Marker，更新選取視覺並觸發 onMarkerSelected。</summary>
        public void SelectMarker(int index)
        {
            _selectedIndex = index;
            // 刷新 Marker 視覺
            if (_transitionProp != null)
            {
                var eventsProp = _transitionProp.FindPropertyRelative("_Events");
                ReadNormalizedTimes(eventsProp, out var ne, out var et);
                DrawMarkers(ne, et);
            }
            onMarkerSelected?.Invoke(_selectedIndex);
        }

        // ================================================================
        // ITimelineViewerController
        // ================================================================

        public void OnScrub(float normalizedRatio)
        {
            var snapped = SnapToFrame(normalizedRatio);
            SetPlayHeadNormalized(snapped);
        }

        public void OnRequestAddEvent(float normalizedRatio)
        {
            if (_transitionProp == null || _clip == null) return;

            var snapped = SnapToFrame(normalizedRatio);
            AddEventAt(snapped);
        }

        public void OnMoveEvent(int eventIndex, float normalizedRatio)
        {
            if (_transitionProp == null || _clip == null) return;

            var snapped = SnapToFrame(normalizedRatio);
            MoveEventAt(eventIndex, snapped);
        }

        public void OnRequestDeleteEvent(int eventIndex)
        {
            if (_transitionProp == null || _clip == null) return;

            DeleteEventAt(eventIndex);
        }

        public void OnBeginDrag()
        {
            _suppressReload = true;
            _pendingReload = false;
        }

        public void OnEndDrag()
        {
            _suppressReload = false;
            if (_pendingReload)
            {
                _pendingReload = false;
                Reload();
            }
        }

        // ================================================================
        // Private: Data Reload
        // ================================================================

        void Reload()
        {
            if (_suppressReload) { _pendingReload = true; return; }
            if (_transitionProp == null || _clip == null) return;

            var eventsProp = _transitionProp.FindPropertyRelative("_Events");
            ReadNormalizedTimes(eventsProp, out var normalEvents, out var endEventTime);
            SyncEventDataListSize(normalEvents.Count);
            ShowClip(normalEvents, endEventTime);
            _btnAdd.SetEnabled(true);
        }

        // ── EventData List との同期 ───────────────────────────────
        // eventDataList のサイズを normalEvents の数と同期します。
        // アイテムの追加は末尾へ、足りない分はnullで埋めます。

        /// <summary>確保 eventDataListProp 的 arraySize 等於 normalEventCount。</summary>
        void SyncEventDataListSize(int normalEventCount)
        {
            if (_eventDataListProp == null || !_eventDataListProp.isArray) return;

            int current = _eventDataListProp.arraySize;

            if (current == normalEventCount) return;

            Undo.RecordObject(_eventDataListProp.serializedObject.targetObject, "Sync EventData List");

            if (current < normalEventCount)
            {
                // 補足空 EventData
                for (int i = current; i < normalEventCount; i++)
                    _eventDataListProp.InsertArrayElementAtIndex(i);
            }
            else
            {
                // 移除多餘
                for (int i = current - 1; i >= normalEventCount; i--)
                    _eventDataListProp.DeleteArrayElementAtIndex(i);
            }

            _eventDataListProp.serializedObject.ApplyModifiedProperties();
        }

        // ================================================================
        // Private: SerializedProperty 寫入
        // ================================================================

        void AddEventAt(float normalizedTime)
        {
            var timesProp = GetNormalizedTimesProp();

            if (timesProp == null) return;

            var times = ReadRawTimes(timesProp);

            if (times.Take(times.Count - 1).Any(t => Mathf.Approximately(t, normalizedTime)))
            {
                Debug.LogWarning($"[ClipTransitionViewer] normalizedTime={normalizedTime:F4} 已存在，跳過。");

                return;
            }

            // 計算插入排序後的 index
            float endEvt = times.Count > 0 ? times[times.Count - 1] : float.NaN;
            var normal = times.Take(times.Count - 1).ToList();
            normal.Add(normalizedTime);
            normal.Sort();
            int insertedIndex = normal.IndexOf(normalizedTime);
            normal.Add(endEvt);

            WriteRawTimes(timesProp, normal);

            // 同步 eventDataList：在相同 index 插入空 EventData
            if (_eventDataListProp != null && _eventDataListProp.isArray)
            {
                _eventDataListProp.InsertArrayElementAtIndex(insertedIndex);
                // InsertArrayElementAtIndex 複製前一個，清空它
                var elem = _eventDataListProp.GetArrayElementAtIndex(insertedIndex);
                elem.managedReferenceValue = new global::TheLostSpirit.Infrastructure.EventData();
                _eventDataListProp.serializedObject.ApplyModifiedProperties();
            }

            _selectedIndex = insertedIndex;
            Reload();
            onMarkerSelected?.Invoke(_selectedIndex);
        }

        void MoveEventAt(int index, float normalizedTime)
        {

            var timesProp = GetNormalizedTimesProp();

            if (timesProp == null) return;

            var times = ReadRawTimes(timesProp);
            int normalCount = times.Count - 1;

            if (index < 0 || index >= normalCount) return;

            for (int i = 0; i < normalCount; i++)
            {
                if (i != index && Mathf.Approximately(times[i], normalizedTime))
                {
                    Debug.LogWarning($"[ClipTransitionViewer] 位置 {normalizedTime:F4} 已有事件，移動取消。");
                    Reload();

                    return;
                }
            }

            float endEvt = times[times.Count - 1];
            var normal = times.Take(normalCount).ToList();
            normal[index] = normalizedTime;

            // 記住舊順序下 index 位置的 EventData，排序後重新排列
            EventData movedData = null;
            if (_eventDataListProp != null && _eventDataListProp.isArray &&
                index < _eventDataListProp.arraySize)
            {
                movedData = GetEventData(index);
            }

            // 排序，找出排序後新 index
            var sortOrder = Enumerable.Range(0, normalCount).ToList();
            sortOrder.Sort((a, b) => normal[a].CompareTo(normal[b]));

            var sortedNormal = sortOrder.Select(i2 => normal[i2]).ToList();
            sortedNormal.Add(endEvt);

            WriteRawTimes(timesProp, sortedNormal);

            // 同步重排 EventData list
            if (_eventDataListProp != null && _eventDataListProp.isArray &&
                _eventDataListProp.arraySize == normalCount)
            {
                ReorderEventDataList(sortOrder);
            }

            // 選取移動後的新 index
            int newIndex = sortOrder.IndexOf(index);
            _selectedIndex = newIndex;
            Reload();   // ★ 必須重建 Marker DOM，否則各 EventMarkerManipulator 的 _eventIndex 會與 SP 錯位
            onMarkerSelected?.Invoke(_selectedIndex);
        }

        void DeleteEventAt(int index)
        {
            var timesProp = GetNormalizedTimesProp();

            if (timesProp == null) return;

            var times = ReadRawTimes(timesProp);
            int normalCount = times.Count - 1;

            if (index < 0 || index >= normalCount) return;

            float endEvt = times[times.Count - 1];
            var normal = times.Take(normalCount).ToList();
            normal.RemoveAt(index);
            normal.Add(endEvt);

            WriteRawTimes(timesProp, normal);

            // 同步刪除 eventDataList[index]
            if (_eventDataListProp != null && _eventDataListProp.isArray &&
                index < _eventDataListProp.arraySize)
            {
                Undo.RecordObject(_eventDataListProp.serializedObject.targetObject, "Delete EventData");
                _eventDataListProp.DeleteArrayElementAtIndex(index);
                _eventDataListProp.serializedObject.ApplyModifiedProperties();
            }

            _selectedIndex = -1;
            Reload();
            onMarkerSelected?.Invoke(-1);
        }

        /// <summary>讀取 _eventDataListProp[index] 的 managedReferenceValue 為 EventData。</summary>
        EventData GetEventData(int index)
        {
            if (_eventDataListProp == null || !_eventDataListProp.isArray ||
                index < 0 || index >= _eventDataListProp.arraySize)
                return null;

            return _eventDataListProp
                   .GetArrayElementAtIndex(index)
                   .managedReferenceValue as global::TheLostSpirit.Infrastructure.EventData;
        }

        /// <summary>依照 sortOrder 重排 _eventDataListProp。</summary>
        void ReorderEventDataList(List<int> sortOrder)
        {
            // 先把所有 EventData 緩存出來
            var snapshot = new List<global::TheLostSpirit.Infrastructure.EventData>();
            for (int i = 0; i < sortOrder.Count; i++)
                snapshot.Add(GetEventData(i));

            Undo.RecordObject(_eventDataListProp.serializedObject.targetObject, "Reorder EventData");

            for (int i = 0; i < sortOrder.Count; i++)
            {
                var elem = _eventDataListProp.GetArrayElementAtIndex(i);
                elem.managedReferenceValue = snapshot[sortOrder[i]];
            }

            _eventDataListProp.serializedObject.ApplyModifiedProperties();
        }

        SerializedProperty GetNormalizedTimesProp()
        {
            if (_transitionProp == null) return null;
            var eventsProp = _transitionProp.FindPropertyRelative("_Events");

            return eventsProp?.FindPropertyRelative("_NormalizedTimes");
        }

        static List<float> ReadRawTimes(SerializedProperty timesProp)
        {
            var list = new List<float>();

            if (timesProp == null || !timesProp.isArray) return list;
            for (int i = 0; i < timesProp.arraySize; i++)
                list.Add(timesProp.GetArrayElementAtIndex(i).floatValue);

            return list;
        }

        static void WriteRawTimes(SerializedProperty timesProp, List<float> times)
        {
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
            out List<float> normalEvents,
            out float? endEventTime
        )
        {
            normalEvents = new List<float>();
            endEventTime = null;

            if (eventsProp == null) return;

            var timesProp = eventsProp.FindPropertyRelative("_NormalizedTimes");

            if (timesProp == null || !timesProp.isArray || timesProp.arraySize == 0)
                return;

            int lastIndex = timesProp.arraySize - 1;

            for (int i = 0; i < timesProp.arraySize; i++)
            {
                float v = timesProp.GetArrayElementAtIndex(i).floatValue;

                if (i == lastIndex)
                {
                    if (!float.IsNaN(v))
                        endEventTime = v;
                }
                else
                {
                    normalEvents.Add(v);
                }
            }
        }

        // ================================================================
        // Private: UI
        // ================================================================

        void ShowClip(List<float> normalEvents, float? endEventTime)
        {
            _clipBar.style.display = DisplayStyle.Flex;
            _rulerLayer.style.display = DisplayStyle.Flex;
            _markerContainer.style.display = DisplayStyle.Flex;
            _noClipLabel.style.display = DisplayStyle.None;

            DrawRuler();
            DrawMarkers(normalEvents, endEventTime);
            SetPlayHeadNormalized(_currentNormalized);
        }

        void SetNoClip()
        {
            _clip = null;
            _clipLength = 0f;
            _transitionProp = null;

            _clipBar.style.display = DisplayStyle.None;
            _rulerLayer.style.display = DisplayStyle.None;
            _markerContainer.style.display = DisplayStyle.None;
            _noClipLabel.style.display = DisplayStyle.Flex;
            _timeLabel.text = "--";
            _btnAdd.SetEnabled(false);
        }

        void DrawRuler()
        {
            _rulerLayer.Clear();

            if (_clipLength <= 0f) return;

            var step = _clipLength < 2f ? 0.1f : _clipLength < 4f ? 0.5f : 1f;
            var count = Mathf.CeilToInt(_clipLength / step);
            if (count > 200) count = 200;

            for (int i = 0; i <= count; i++)
            {
                var t = i * step;

                if (t > _clipLength) break;

                var p = t / _clipLength * 100f;

                _rulerLayer.Add(new VisualElement
                {
                    style = {
                        position        = Position.Absolute,
                        left            = Length.Percent(p),
                        top             = 0,
                        width           = 1,
                        height          = 6,
                        backgroundColor = Styles.RulerTick
                    }
                });
                _rulerLayer.Add(new Label(t.ToString("0.0"))
                {
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

        void DrawMarkers(List<float> normalEvents, float? endEventTime)
        {
            _markerContainer.Clear();

            var frameRate = _clip ? _clip.frameRate : 60f;

            // ── 普通事件 Markers（原版風格：垂直細條）─────────────
            for (int i = 0; i < normalEvents.Count; i++)
            {
                var n = Mathf.Clamp01(normalEvents[i]);
                var timeSec = n * _clipLength;
                var frame = Mathf.RoundToInt(timeSec * frameRate);
                var isSelected = i == _selectedIndex;

                var marker = new VisualElement
                {
                    tooltip = $"Event [{i}]  f{frame} ({timeSec:F2}s)\n左鍵點擊選取 | 左鍵拖動移動 | 右鍵刪除",
                    pickingMode = PickingMode.Position,
                    style =
                    {
                        position        = Position.Absolute,
                        left            = Length.Percent(n * 100f),
                        top             = Layout.MarkerTop,
                        width           = Layout.MarkerWidth,
                        height          = Layout.MarkerHeight,
                        marginLeft      = -Layout.MarkerWidth * 0.5f,
                        backgroundColor = isSelected ? Styles.MarkerSelected : Styles.MarkerNormal,
                        borderTopWidth    = 1, borderBottomWidth = 1,
                        borderLeftWidth   = 1, borderRightWidth  = 1,
                        borderTopColor    = Styles.MarkerBorder,
                        borderBottomColor = Styles.MarkerBorder,
                        borderLeftColor   = Styles.MarkerBorder,
                        borderRightColor  = Styles.MarkerBorder,
                    }
                };

                int capturedIndex = i;
                marker.RegisterCallback<ClickEvent>(evt =>
                {
                    if (evt.button == 0 && !evt.shiftKey)
                    {
                        _selectedIndex = capturedIndex;
                        var eventsProp = _transitionProp?.FindPropertyRelative("_Events");
                        ReadNormalizedTimes(eventsProp, out var ne, out var et);
                        DrawMarkers(ne, et);
                        onMarkerSelected?.Invoke(_selectedIndex);
                        evt.StopPropagation();
                    }
                });

                if (_transitionProp != null)
                    marker.AddManipulator(new EventMarkerManipulator(this, _track, i));

                _markerContainer.Add(marker);
            }

            // ── End Event 垂直線（唯讀，青綠色）───────────────
            if (endEventTime.HasValue)
            {
                var n = Mathf.Clamp01(endEventTime.Value);
                var timeSec = n * _clipLength;
                var frame = Mathf.RoundToInt(timeSec * frameRate);

                var endLine = new VisualElement
                {
                    tooltip = $"End Event  f{frame} ({timeSec:F2}s)",
                    pickingMode = PickingMode.Position,
                    style =
                    {
                        position        = Position.Absolute,
                        left            = Length.Percent(n * 100f),
                        top             = 0,
                        bottom          = 0,
                        width           = 2,
                        marginLeft      = -1,
                        backgroundColor = Styles.EndEventColor
                    }
                };

                endLine.Add(new Label("END")
                {
                    pickingMode = PickingMode.Ignore,
                    style =
                    {
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

        float SnapToFrame(float normalizedRatio)
        {
            if (_clip == null || _clipLength <= 0f) return normalizedRatio;
            var frameRate = _clip.frameRate;
            var timeSec = normalizedRatio * _clipLength;
            var frame = Mathf.RoundToInt(timeSec * frameRate);
            var snappedTime = frame / frameRate;

            return Mathf.Clamp01(snappedTime / _clipLength);
        }

        string FormatTime(float normalizedRatio)
        {
            if (_clip == null) return "--";
            var timeSec = normalizedRatio * _clipLength;
            var frame = Mathf.RoundToInt(timeSec * _clip.frameRate);

            return $"f{frame}  {timeSec:F2}s";
        }

        // ================================================================
        // Private: VisualElement Factories
        // ================================================================

        void StyleRoot()
        {
            style.marginTop = 4;
            style.marginBottom = 4;
        }

        VisualElement MakeToolbar(out Button btnAdd, out Label timeLabel)
        {
            var bar = new VisualElement
            {
                style = {
                    flexDirection  = FlexDirection.Row,
                    alignItems     = Align.Center,
                    justifyContent = Justify.SpaceBetween,
                    marginBottom   = 2
                }
            };

            timeLabel = new Label("--")
            {
                style = {
                    fontSize = 10,
                    color    = Styles.InfoText,
                    flexGrow = 1
                }
            };

            btnAdd = new Button(() => OnRequestAddEvent(_currentNormalized))
            {
                text = "+ Add Event",
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

        VisualElement MakeTrack()
        {
            return new VisualElement
            {
                style = {
                    height          = Layout.TrackHeight,
                    marginTop       = 2,
                    marginBottom    = 4,
                    backgroundColor = Styles.TrackBg,
                    borderTopWidth  = 1, borderBottomWidth = 1,
                    borderLeftWidth = 1, borderRightWidth  = 1,
                    borderTopColor  = Styles.Border, borderBottomColor = Styles.Border,
                    borderLeftColor = Styles.Border, borderRightColor  = Styles.Border,
                    overflow        = Overflow.Visible
                }
            };
        }

        VisualElement MakeClipBar()
        {
            return new VisualElement
            {
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

        VisualElement MakeLayer()
        {
            return new VisualElement
            {
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

        VisualElement MakeCursorLayer(out VisualElement playHeadLine)
        {
            var layer = new VisualElement
            {
                style = { position = Position.Absolute, left = 0, right = 0, top = 0, bottom = 0 },
                pickingMode = PickingMode.Ignore
            };

            playHeadLine = new VisualElement
            {
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

        Label MakeNoClipLabel()
        {
            return new Label("No ClipTransition / Clip Assigned")
            {
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
            public const float TrackHeight = 45f;
            public const float ClipBarHeight = 20f;
            public const float RulerTop = 20f; // Ruler 從 ClipBar 底部開始
            public const float MarkerTop = -1f; // 原版風格：略超出軌道上緣
            public const float MarkerWidth = 4f;  // 原版風格：細條
            public const float MarkerHeight = 15f; // 原版風格
        }

        static class Styles
        {
            public static readonly Color TrackBg = new Color(0.15f, 0.15f, 0.15f);
            public static readonly Color ClipBar = new Color(0.3f, 0.4f, 0.55f);
            public static readonly Color Border = new Color(0.1f, 0.1f, 0.1f);
            public static readonly Color MarkerNormal = new Color(0.7f, 0.7f, 0.7f); // 原版灰色
            public static readonly Color MarkerSelected = new Color(1f, 0.7f, 0.2f); // 原版橙黃色
            public static readonly Color MarkerBorder = Color.black;
            public static readonly Color EndEventColor = new Color(0.3f, 0.9f, 0.8f); // 青綠色
            public static readonly Color RulerTick = Color.gray;
            public static readonly Color RulerText = Color.gray;
            public static readonly Color PlayHead = Color.white;
            public static readonly Color InfoText = new Color(0.7f, 0.7f, 0.7f);
            public static readonly Color AddButtonBg = new Color(0.2f, 0.5f, 0.3f);
        }
    }
}