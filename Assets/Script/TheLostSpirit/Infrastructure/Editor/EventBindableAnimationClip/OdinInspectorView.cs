using System;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace TheLostSpirit.Infrastructure.Editor.EventBindableAnimationClip
{
    public class OdinInspectorView : VisualElement, IDisposable
    {
        Button _btnDelete;
        Button _btnNext;
        Button _btnPrev;
        EventData _data;
        IntegerField _frameField;
        Label _infoLabel;
        string _lastSerializedData;
        PropertyTree _tree;

        public OdinInspectorView()
        {
            StyleSetting();

            Add(NavigationHeader());
            Add(Separator());
            Add(OdinContainer());
            Add(Separator());
            Add(Footer());
        }

        public void Dispose()
        {
            _tree?.Dispose();
        }

        public event Action<EventData> onSave;
        public event Action<int> onNavigate;
        public event Action<int> onDelete;
        public event Action<int> onFrameChanged;

        void StyleSetting()
        {
            style.paddingTop = Layout.Padding;
            style.paddingBottom = Layout.Padding;
            style.paddingLeft = Layout.Padding;
            style.paddingRight = Layout.Padding;
            style.backgroundColor = Styles.InspectorBg;
            style.borderTopWidth = 1;
            style.borderBottomWidth = 1;
            style.borderLeftWidth = 1;
            style.borderRightWidth = 1;
            style.borderTopColor = Styles.Border;
            style.borderBottomColor = Styles.Border;
            style.borderLeftColor = Styles.Border;
            style.borderRightColor = Styles.Border;
            style.display = DisplayStyle.None;
        }

        VisualElement NavigationHeader()
        {
            var navGroup = new VisualElement
            {
                style = {
                    flexDirection  = FlexDirection.Row,
                    justifyContent = Justify.SpaceBetween,
                    alignItems     = Align.Center,
                    marginBottom   = Layout.MarginBottom
                }
            };

            _btnPrev = new Button(() => onNavigate?.Invoke(-1))
            {
                text = "◄",
                style = { width = Layout.NavButtonWidth }
            };
            _btnNext = new Button(() => onNavigate?.Invoke(1))
            {
                text = "►",
                style = { width = Layout.NavButtonWidth }
            };

            var centerGroup = new VisualElement
            {
                style = { flexDirection = FlexDirection.Row, alignItems = Align.Center }
            };
            _infoLabel = new Label
            {
                style = {
                    marginRight             = Layout.MarginBottom,
                    unityFontStyleAndWeight = FontStyle.Bold
                }
            };
            _frameField = new IntegerField
            {
                isDelayed = true,
                style = { width = Layout.FrameFieldWidth }
            };
            _frameField.RegisterValueChangedCallback(e => onFrameChanged?.Invoke(e.newValue));

            centerGroup.Add(_infoLabel);
            centerGroup.Add(_frameField);
            centerGroup.Add(new Label(" f"));

            navGroup.Add(_btnPrev);
            navGroup.Add(centerGroup);
            navGroup.Add(_btnNext);

            return navGroup;
        }

        VisualElement OdinContainer()
        {
            var box = new VisualElement
            {
                style = {
                    backgroundColor = Styles.InspectorBox,
                    paddingTop      = Layout.Padding,
                    paddingBottom   = Layout.Padding,
                    paddingLeft     = Layout.Padding,
                    paddingRight    = Layout.Padding
                }
            };

            var container = new IMGUIContainer(OnGUI) { style = { flexGrow = 1 } };
            box.Add(container);

            return box;
        }

        VisualElement Footer()
        {
            var footerGroup = new VisualElement
            {
                style = {
                    flexDirection  = FlexDirection.Row,
                    justifyContent = Justify.FlexEnd,
                    marginTop      = Layout.MarginBottom
                }
            };

            _btnDelete = new Button(() => onDelete?.Invoke(0))
            {
                text = "Delete",
                style = {
                    width           = Layout.DeleteButtonWidth,
                    backgroundColor = Styles.DeleteButton,
                    color           = Color.white
                }
            };

            footerGroup.Add(_btnDelete);

            return footerGroup;
        }

        static VisualElement Separator()
        {
            return new VisualElement
            {
                style = {
                    height          = Layout.SeparatorHeight,
                    backgroundColor = Styles.Border,
                    marginTop       = Layout.MarginBottom,
                    marginBottom    = Layout.MarginBottom
                }
            };
        }

        public void SetData(
            EventData data,
            float time,
            float frameRate,
            int index,
            int totalCount,
            bool forceRefresh = false
        )
        {
            if (data == null)
            {
                HideInspector();

                return;
            }

            style.display = DisplayStyle.Flex;

            UpdateNavigationInfo(time, frameRate, index, totalCount);
            RefreshPropertyTree(data, forceRefresh);
        }

        void HideInspector()
        {
            style.display = DisplayStyle.None;
            _data = null;
            _tree?.Dispose();
            _tree = null;
        }

        void UpdateNavigationInfo(float time, float frameRate, int index, int totalCount)
        {
            // Update time and frame display
            _infoLabel.text = $"{time:F2}s  /";
            var currentFrame = TimelineMath.TimeToFrame(time, frameRate);
            _frameField.SetValueWithoutNotify(currentFrame);

            // Update navigation button states
            _btnPrev.SetEnabled(index > 0);
            _btnNext.SetEnabled(index < totalCount - 1);
        }

        void RefreshPropertyTree(EventData data, bool forceRefresh)
        {
            if (_data == data && !forceRefresh)
            {
                return;
            }

            _data = data;
            _lastSerializedData = EventDataSerializer.Serialize(_data); // Initialize cache
            _tree?.Dispose();
            _tree = PropertyTree.Create(_data);
        }

        void OnGUI()
        {
            if (_tree == null)
            {
                return;
            }

            // Draw the inspector
            _tree.Draw(false);

            // Defer save until end of frame to ensure all changes are captured
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            EditorApplication.delayCall -= DeferredSave;
            EditorApplication.delayCall += DeferredSave;
        }


        void DeferredSave()
        {
            // Only save if data actually changed
            var currentSerialized = EventDataSerializer.Serialize(_data);
            if (currentSerialized == _lastSerializedData)
            {
                return;
            }

            _lastSerializedData = currentSerialized;
            onSave?.Invoke(_data);
        }

        static class Layout
        {
            public const float Padding = 5f;
            public const float MarginBottom = 5f;
            public const float NavButtonWidth = 30f;
            public const float FrameFieldWidth = 50f;
            public const float DeleteButtonWidth = 80f;
            public const float SeparatorHeight = 1f;
        }

        static class Styles
        {
            public static readonly Color InspectorBg = new Color(0.2f, 0.2f, 0.2f);
            public static readonly Color InspectorBox = new Color(0.25f, 0.25f, 0.25f);
            public static readonly Color Border = new Color(0.1f, 0.1f, 0.1f);
            public static readonly Color DeleteButton = new Color(0.6f, 0.2f, 0.2f);
        }
    }
}