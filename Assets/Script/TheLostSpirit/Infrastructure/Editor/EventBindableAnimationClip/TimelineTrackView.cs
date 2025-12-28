using UnityEngine;
using UnityEngine.UIElements;

namespace TheLostSpirit.Infrastructure.Editor.EventBindableAnimationClip
{
    public class TimelineTrackView : VisualElement
    {
        readonly VisualElement       _clipBar;
        readonly ITimelineController _controller;
        readonly VisualElement       _cursorLayer;
        readonly VisualElement       _durationBar;
        readonly VisualElement       _markerContainer;
        readonly Label               _noClipLabel;
        readonly VisualElement       _rulerLayer;
        AnimationClip                _clip;
        VisualElement                _playHeadLine;

        public TimelineTrackView(ITimelineController controller) {
            _controller = controller;

            StyleSetting();

            _clipBar         = ClipBar();
            _rulerLayer      = RulerLayer();
            _durationBar     = DurationBar();
            _markerContainer = MarkerContainer();
            _cursorLayer     = CursorLayer();
            _noClipLabel     = NoClipLabel();

            Add(_clipBar);
            Add(_rulerLayer);
            Add(_durationBar);
            Add(_markerContainer);
            Add(_cursorLayer);
            Add(_noClipLabel);

            // Manipulators
            this.AddManipulator(new ScrubberManipulator(_controller, this));
        }

        void StyleSetting() {
            style.height            = Layout.TrackHeight;
            style.marginTop         = 2;
            style.marginBottom      = 5;
            style.backgroundColor   = Styles.TrackBg;
            style.borderTopWidth    = 1;
            style.borderBottomWidth = 1;
            style.borderLeftWidth   = 1;
            style.borderRightWidth  = 1;
            style.borderTopColor    = Styles.Border;
            style.borderBottomColor = Styles.Border;
            style.borderLeftColor   = Styles.Border;
            style.borderRightColor  = Styles.Border;
            style.overflow          = Overflow.Visible;
            style.justifyContent    = Justify.Center;
            style.alignItems        = Align.Center;
        }

        public void SetClip(AnimationClip clip) {
            _clip = clip;

            if (_clip == null) {
                SetLayersVisible(false);
                _noClipLabel.style.display = DisplayStyle.Flex;
            }
            else {
                SetLayersVisible(true);
                _noClipLabel.style.display = DisplayStyle.None;
                DrawRuler();
            }
        }

        void SetLayersVisible(bool isVisible) {
            var display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;
            _clipBar.style.display         = display;
            _rulerLayer.style.display      = display;
            _markerContainer.style.display = display;
            _cursorLayer.style.display     = display;
        }

        public void UpdateDurationVisuals(EventData eventData, int selectedEventIndex, float eventTime) {
            _durationBar.style.display = DisplayStyle.None;

            if (_clip == null || selectedEventIndex == -1 || eventData == null) {
                return;
            }

            if (eventData.CantInspectDuration) {
                return;
            }

            var selectedIdx     = eventData.selectedIndex;
            var action          = eventData.combatSteps[selectedIdx];
            var frames          = action.DurationFrames;
            var durationSeconds = TimelineMath.FrameToTime(frames, _clip.frameRate);

            var startTime  = eventTime;
            var startRatio = startTime / _clip.length;
            var widthRatio = durationSeconds / _clip.length;

            _durationBar.style.left    = Length.Percent(startRatio * 100f);
            _durationBar.style.width   = Length.Percent(widthRatio * 100f);
            _durationBar.style.display = DisplayStyle.Flex;
            _durationBar.tooltip       = $"{frames} frames ({durationSeconds:F2}s)";
        }

        public void HideDurationBar() {
            _durationBar.style.display = DisplayStyle.None;
        }

        public void SetPlayHead(float time) {
            if (!_clip) {
                return;
            }

            var p = time / _clip.length * 100f;
            _playHeadLine.style.left = Length.Percent(p);
        }

        public void AddMarker(int index, float time, bool isSelected) {
            if (!_clip) {
                return;
            }

            var ratio = time / _clip.length;

            var marker = new MarkerElement(index, isSelected) {
                style = {
                    position = Position.Absolute,
                    left     = Length.Percent(ratio * 100f),
                    top      = MarkerElement.Layout.Top
                }
            };

            marker.AddManipulator(new MarkerManipulator(_controller, index, this));
            _markerContainer.Add(marker);
        }

        public void HighlightMarker(int index) {
            foreach (var child in _markerContainer.Children())
                if (child is MarkerElement m) {
                    m.SetSelected(m.Index == index);
                }
        }

        public void ClearMarkers() {
            _markerContainer.Clear();
        }

        public float LocalToTime(float localX) {
            if (!_clip || contentRect.width <= 0) {
                return 0;
            }

            var ratio = Mathf.Clamp01(localX / contentRect.width);

            return ratio * _clip.length;
        }

        void DrawRuler() {
            _rulerLayer.Clear();
            if (!_clip) {
                return;
            }

            var duration = _clip.length;
            var step     = duration < 2f ? 0.1f : duration < 4f ? 0.5f : 1f;
            var count    = Mathf.CeilToInt(duration / step);
            if (count > 200) {
                count = 200;
            }

            for (var i = 0; i <= count; i++) {
                var t = i * step;
                if (t > duration) {
                    break;
                }

                var p = t / duration * 100f;
                _rulerLayer.Add(new VisualElement {
                    style = {
                        position        = Position.Absolute, left = Length.Percent(p), top = 0, width = 1, height = 6,
                        backgroundColor = Styles.RulerLine
                    }
                });
                _rulerLayer.Add(new Label(t.ToString("0.0")) {
                    style = {
                        position = Position.Absolute, left = Length.Percent(p), top = 10, fontSize = 9,
                        color    = Styles.RulerText
                    }
                });
            }
        }

        #region Layer Factory Methods

        VisualElement ClipBar() {
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

        VisualElement RulerLayer() {
            return new VisualElement {
                style = {
                    position        = Position.Absolute,
                    left            = 0,
                    right           = 0,
                    top             = Layout.RulerTop,
                    bottom          = 0,
                    backgroundColor = Color.clear
                }
            };
        }

        VisualElement DurationBar() {
            return new VisualElement {
                style = {
                    position         = Position.Absolute,
                    left             = 0,
                    top              = 0,
                    height           = Layout.ClipBarHeight,
                    backgroundColor  = new Color(0.4f, 0.8f, 0.4f, 0.4f),
                    borderRightWidth = 1,
                    borderRightColor = new Color(1, 1, 1, 0.5f),
                    display          = DisplayStyle.None
                },
                pickingMode = PickingMode.Ignore
            };
        }

        VisualElement MarkerContainer() {
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

        VisualElement CursorLayer() {
            var cursorLayer = new VisualElement {
                style = {
                    position        = Position.Absolute,
                    left            = 0,
                    right           = 0,
                    top             = 0,
                    bottom          = 0,
                    backgroundColor = Color.clear
                },
                pickingMode = PickingMode.Ignore
            };

            _playHeadLine = new VisualElement {
                style = {
                    position        = Position.Absolute,
                    width           = 1,
                    top             = 0,
                    bottom          = 0,
                    backgroundColor = Color.white,
                    marginLeft      = -0.5f
                }
            };
            cursorLayer.Add(_playHeadLine);

            return cursorLayer;
        }

        Label NoClipLabel() {
            return new Label("No Clip Assigned") {
                style = {
                    color                   = Color.gray,
                    unityFontStyleAndWeight = FontStyle.Italic,
                    alignSelf               = Align.Center
                }
            };
        }

        #endregion

        #region [Configuration] Constants & Styles

        public static class Layout
        {
            public const float TrackHeight   = 45f;
            public const float ClipBarHeight = 20f;
            public const float RulerTop      = 20f;
        }

        public static class Styles
        {
            public static readonly Color TrackBg   = new Color(0.15f, 0.15f, 0.15f);
            public static readonly Color ClipBar   = new Color(0.3f, 0.4f, 0.55f);
            public static readonly Color Border    = new Color(0.1f, 0.1f, 0.1f);
            public static readonly Color RulerLine = Color.gray;
            public static readonly Color RulerText = Color.gray;
        }

        #endregion
    }
}