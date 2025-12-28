using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace TheLostSpirit.Infrastructure.Editor.EventBindableAnimationClip
{
    public class TimelineToolbarView : VisualElement
    {
        IntegerField _frameField;
        Label        _timeLabel;
        Label        _totalFramesLabel;

        public TimelineToolbarView() {
            StyleSetting();

            Add(AddEventButton());
            Add(RightTools());
        }

        // Events
        public event Action onAddEvent;
        public event Action<int> onFrameChanged;

        void StyleSetting() {
            style.flexDirection  = FlexDirection.Row;
            style.marginTop      = 5;
            style.justifyContent = Justify.SpaceBetween;
            style.alignItems     = Align.Center;
        }

        VisualElement RightTools() {
            var rightTools = new VisualElement {
                style = { flexDirection = FlexDirection.Row, alignItems = Align.Center }
            };

            _timeLabel = new Label("0.00s (") {
                style = {
                    minWidth       = 40,
                    unityTextAlign = TextAnchor.MiddleRight,
                    marginRight    = 5
                }
            };

            _frameField = new IntegerField {
                style     = { width = 45, height = 18, marginLeft = 2, marginRight = 2 },
                isDelayed = true
            };
            _frameField.RegisterValueChangedCallback(evt => onFrameChanged?.Invoke(evt.newValue));

            _totalFramesLabel = new Label("/ 0 f )");

            rightTools.Add(_timeLabel);
            rightTools.Add(_frameField);
            rightTools.Add(_totalFramesLabel);

            return rightTools;
        }

        Button AddEventButton() {
            var addButton = new Button(() => onAddEvent?.Invoke()) {
                tooltip = "Add Event",
                style = {
                    width               = 24,
                    height              = 20,
                    marginLeft          = 2,
                    backgroundImage     = (Texture2D)EditorGUIUtility.IconContent("Animation.AddEvent").image,
                    backgroundColor     = Styles.BtnNormal,
                    borderTopWidth      = 1,
                    borderBottomWidth   = 1,
                    borderLeftWidth     = 1,
                    borderRightWidth    = 1,
                    borderTopColor      = Styles.Border,
                    borderBottomColor   = Styles.Border,
                    borderLeftColor     = Styles.Border,
                    borderRightColor    = Styles.Border,
                    backgroundSize      = new BackgroundSize(BackgroundSizeType.Contain),
                    backgroundPositionX = new BackgroundPosition(BackgroundPositionKeyword.Center),
                    backgroundPositionY = new BackgroundPosition(BackgroundPositionKeyword.Center)
                }
            };

            // Hover effects
            addButton.RegisterCallback<MouseEnterEvent>(_ => addButton.style.backgroundColor = Styles.BtnHover);
            addButton.RegisterCallback<MouseLeaveEvent>(_ => addButton.style.backgroundColor = Styles.BtnNormal);

            return addButton;
        }

        public void SetTime(float time, float frameRate, float clipLength) {
            _timeLabel.text = $"{time:F2}s";

            var currentFrame = TimelineMath.TimeToFrame(time, frameRate);
            var totalFrames  = TimelineMath.TimeToFrame(clipLength, frameRate);

            _frameField.SetValueWithoutNotify(currentFrame);
            _totalFramesLabel.text = $"/ {totalFrames} f )";
        }

        static class Styles
        {
            public static readonly Color BtnNormal = new Color(0.22f, 0.22f, 0.22f);
            public static readonly Color BtnHover  = new Color(0.35f, 0.35f, 0.35f);
            public static readonly Color Border    = new Color(0.1f, 0.1f, 0.1f);
        }
    }
}