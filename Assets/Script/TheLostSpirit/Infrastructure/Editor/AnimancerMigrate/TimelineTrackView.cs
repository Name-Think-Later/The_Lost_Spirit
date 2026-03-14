using TheLostSpirit.Domain.Skill.Manifest.Manifestation;
using TheLostSpirit.Infrastructure;
using UnityEngine;
using UnityEngine.UIElements;

// ================================================================
// [AnimancerMigrate] TimelineTrackView
// 職責：時間軸軌道的純 UI 管理。
//       對應原版 EventBindableAnimationClipDrawer/TimelineTrackView。
// ================================================================

namespace TheLostSpirit.Infrastructure.Editor.AnimancerMigrate
{
    /// <summary>
    /// 管理時間軌道的所有子層：ClipBar、DurationBar、RulerLayer、MarkerContainer、CursorLayer、NoClipLabel。
    /// 本類別只負責 UI，不讀寫 SerializedProperty。
    /// </summary>
    public class TimelineTrackView : VisualElement
    {
        readonly VisualElement _clipBar;
        readonly VisualElement _durationBar;   // DurationFrames 色塊
        readonly VisualElement _rulerLayer;
        readonly VisualElement _markerContainer;
        readonly VisualElement _playHeadLine;
        readonly Label _noClipLabel;

        AnimationClip _clip;                   // 用於計算比例
        GameObject _characterTarget;        // 用於 DrawDebugRange
#if UNITY_EDITOR
        CombatStep _selectedCombatStep;
#endif

        public TimelineTrackView(ITimelineViewerController controller)
        {
            StyleSetting();

            _clipBar = BuildClipBar();
            _durationBar = BuildDurationBar();
            _rulerLayer = BuildLayer();
            _markerContainer = BuildLayer();
            var cursorLayer = BuildCursorLayer(out _playHeadLine);
            _noClipLabel = BuildNoClipLabel();

            Add(_clipBar);
            Add(_durationBar);   // DurationBar 在 ClipBar 之上，RulerLayer 之下
            Add(_rulerLayer);
            Add(_markerContainer);
            Add(cursorLayer);
            Add(_noClipLabel);

            this.AddManipulator(new TimelineScrubberManipulator(controller));
        }

        // ── Public API ─────────────────────────────────────────

        /// <summary>設定要顯示的 clip，並決定是否顯示各層。</summary>
        public void SetClip(AnimationClip clip)
        {
            _clip = clip;
            bool hasClip = clip != null;
            SetLayersVisible(hasClip);

            if (hasClip)
                DrawRuler(clip.length);
            else
                HideDurationBar();
        }

        /// <summary>設定角色 Transform 目標，用於 DrawDebugRange 的 Scene Gizmo。</summary>
        public void SetCharacterTarget(GameObject target) => _characterTarget = target;

        /// <summary>
        /// 根據選取的 EventData + CombatStep 顯示反應時間色塊。
        /// 對應原版 UpdateDurationVisuals。
        /// </summary>
        public void UpdateDurationVisuals(EventData eventData, int selectedEventIndex, float eventTimeSec)
        {
            HideDurationBar();

#if UNITY_EDITOR
            if (_clip == null || selectedEventIndex < 0 || eventData == null) return;
            if (eventData.CantInspectDuration) return;

            var combatStepIdx = eventData.selectedIndex;
            if (combatStepIdx < 0 || combatStepIdx >= eventData.combatSteps.Count) return;

            var step = eventData.combatSteps[combatStepIdx];
            var durationSec = step.DurationFrames / _clip.frameRate;
            var startRatio = eventTimeSec / _clip.length;
            var widthRatio = durationSec / _clip.length;

            _durationBar.style.left = Length.Percent(Mathf.Clamp01(startRatio) * 100f);
            _durationBar.style.width = Length.Percent(Mathf.Clamp01(widthRatio) * 100f);
            _durationBar.style.display = DisplayStyle.Flex;
            _durationBar.tooltip = $"{step.DurationFrames} f ({durationSec:F2}s)";

            _selectedCombatStep = step;
#endif
        }

        /// <summary>隱藏 DurationBar 並清除 CombatStep 參考。</summary>
        public void HideDurationBar()
        {
            _durationBar.style.display = DisplayStyle.None;
#if UNITY_EDITOR
            _selectedCombatStep = null;
#endif
        }

#if UNITY_EDITOR
        /// <summary>
        /// 在 Scene 中繪製選取 CombatStep 的攻擊範圍 Gizmo。
        /// 對應原版 DrawDebugRange。
        /// 回傳 true 表示有繪製，false 表示條件不足。
        /// </summary>
        public bool DrawDebugRange()
        {
            if (_selectedCombatStep == null || _characterTarget == null) return false;
            _selectedCombatStep.DebugDrawRange(_characterTarget.transform);
            return true;
        }
#endif

        /// <summary>移動 Playhead 線到 normalizedRatio（0~1）。</summary>
        public void SetPlayHead(float normalizedRatio)
        {
            _playHeadLine.style.left = Length.Percent(Mathf.Clamp01(normalizedRatio) * 100f);
        }

        /// <summary>清除所有 Marker。</summary>
        public void ClearMarkers() => _markerContainer.Clear();

        /// <summary>加入一個 Marker，並附上 EventMarkerManipulator。</summary>
        public void AddMarker(
            int index,
            float normalizedRatio,
            bool isSelected,
            ITimelineViewerController controller)
        {
            var marker = new AnimancerMarkerElement(index, isSelected)
            {
                style =
                {
                    left = Length.Percent(Mathf.Clamp01(normalizedRatio) * 100f)
                },
                tooltip = BuildMarkerTooltip(index, normalizedRatio, controller.GetClip())
            };

            marker.AddManipulator(new EventMarkerManipulator(controller, this, index));
            _markerContainer.Add(marker);
        }

        /// <summary>僅更新各 Marker 的選取視覺，不重建 DOM。</summary>
        public void HighlightMarker(int selectedIndex)
        {
            foreach (var child in _markerContainer.Children())
                if (child is AnimancerMarkerElement m)
                    m.SetSelected(m.Index == selectedIndex);
        }

        /// <summary>加入 End Event 垂直線（唯讀，青綠色）。</summary>
        public void AddEndEventLine(float normalizedRatio, float clipLength)
        {
            var n = Mathf.Clamp01(normalizedRatio);
            var timeSec = n * clipLength;
            var endLine = new VisualElement
            {
                tooltip = $"End Event  ({timeSec:F2}s)",
                pickingMode = PickingMode.Ignore,
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

        // ── Internal ───────────────────────────────────────────

        void StyleSetting()
        {
            style.height = Layout.TrackHeight;
            style.marginTop = 2;
            style.marginBottom = 4;
            style.backgroundColor = Styles.TrackBg;
            style.borderTopWidth = 1; style.borderBottomWidth = 1;
            style.borderLeftWidth = 1; style.borderRightWidth = 1;
            style.borderTopColor = Styles.Border; style.borderBottomColor = Styles.Border;
            style.borderLeftColor = Styles.Border; style.borderRightColor = Styles.Border;
            style.overflow = Overflow.Visible;
        }

        void SetLayersVisible(bool visible)
        {
            var d = visible ? DisplayStyle.Flex : DisplayStyle.None;
            _clipBar.style.display = d;
            _rulerLayer.style.display = d;
            _markerContainer.style.display = d;
            _noClipLabel.style.display = visible ? DisplayStyle.None : DisplayStyle.Flex;
        }

        void DrawRuler(float clipLength)
        {
            _rulerLayer.Clear();

            if (clipLength <= 0f) return;

            var step = clipLength < 2f ? 0.1f : clipLength < 4f ? 0.5f : 1f;
            var count = Mathf.Min(Mathf.CeilToInt(clipLength / step), 200);

            for (int i = 0; i <= count; i++)
            {
                var t = i * step;
                if (t > clipLength) break;

                var p = t / clipLength * 100f;

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

        static string BuildMarkerTooltip(int index, float normalizedRatio, AnimationClip clip)
        {
            if (clip == null) return $"Event [{index}]";
            var timeSec = normalizedRatio * clip.length;
            var frame = Mathf.RoundToInt(timeSec * clip.frameRate);
            return $"Event [{index}]  f{frame} ({timeSec:F2}s)\n左鍵點擊選取 | 左鍵拖動移動 | 右鍵刪除";
        }

        // ── VisualElement Factories ────────────────────────────

        static VisualElement BuildClipBar() => new VisualElement
        {
            style = {
                position        = Position.Absolute,
                left            = 0, right = 0, top = 0,
                height          = Layout.ClipBarHeight,
                backgroundColor = Styles.ClipBar
            }
        };

        static VisualElement BuildDurationBar() => new VisualElement
        {
            pickingMode = PickingMode.Ignore,
            style = {
                position         = Position.Absolute,
                left             = 0,
                top              = 0,
                height           = Layout.ClipBarHeight,
                backgroundColor  = Styles.DurationBar,
                borderRightWidth = 1,
                borderRightColor = new Color(1f, 1f, 1f, 0.5f),
                display          = DisplayStyle.None
            }
        };

        static VisualElement BuildLayer() => new VisualElement
        {
            style = {
                position        = Position.Absolute,
                left            = 0, right = 0, top = 0, bottom = 0,
                backgroundColor = Color.clear
            }
        };

        static VisualElement BuildCursorLayer(out VisualElement playHeadLine)
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
                    top             = 0, bottom = 0,
                    backgroundColor = Styles.PlayHead,
                    marginLeft      = -0.5f,
                    left            = 0
                }
            };
            layer.Add(playHeadLine);

            return layer;
        }

        static Label BuildNoClipLabel() => new Label("No ClipTransition / Clip Assigned")
        {
            style = {
                color                   = Color.gray,
                unityFontStyleAndWeight = FontStyle.Italic,
                unityTextAlign          = TextAnchor.MiddleCenter,
                position                = Position.Absolute,
                left                    = 0, right = 0, top = 0, bottom = 0
            }
        };

        // ── Constants ──────────────────────────────────────────

        public static class Layout
        {
            public const float TrackHeight = 45f;
            public const float ClipBarHeight = 20f;
        }

        static class Styles
        {
            public static readonly Color TrackBg = new Color(0.15f, 0.15f, 0.15f);
            public static readonly Color ClipBar = new Color(0.3f, 0.4f, 0.55f);
            public static readonly Color DurationBar = new Color(0.4f, 0.8f, 0.4f, 0.4f); // 半透明綠
            public static readonly Color Border = new Color(0.1f, 0.1f, 0.1f);
            public static readonly Color EndEventColor = new Color(0.3f, 0.9f, 0.8f);
            public static readonly Color RulerTick = Color.gray;
            public static readonly Color RulerText = Color.gray;
            public static readonly Color PlayHead = Color.white;
        }
    }
}
