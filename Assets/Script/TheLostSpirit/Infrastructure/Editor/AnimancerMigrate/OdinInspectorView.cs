using System;
using Sirenix.OdinInspector.Editor;
using TheLostSpirit.Infrastructure;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

// ================================================================
// [AnimancerMigrate] OdinInspectorView
// 職責：點選 Marker 後顯示對應 EventData 的 Odin Inspector 面板。
//       對應原版 EventBindableAnimationClipDrawer/OdinInspectorView，
//       但屬於 AnimancerMigrate 命名空間，不跨命名空間依賴。
// ================================================================

namespace TheLostSpirit.Infrastructure.Editor.AnimancerMigrate
{
    /// <summary>
    /// 顯示選取事件的 <see cref="EventData"/> Odin Inspector，
    /// 含導航按鈕（◄ ►）、Frame 欄位、刪除按鈕。
    /// </summary>
    public class OdinInspectorView : VisualElement, IDisposable
    {
        Button       _btnPrev;
        Button       _btnNext;
        Button       _btnDelete;
        Label        _infoLabel;
        IntegerField _frameField;

        EventData    _data;
        string       _lastSerializedData;
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

        // ── Public Events ──────────────────────────────────────
        /// <summary>EventData 被編輯且需要寫回 SP 時觸發。</summary>
        public event Action<EventData> onSave;

        /// <summary>使用者按下 ◄ 或 ► 時觸發，delta = ±1。</summary>
        public event Action<int> onNavigate;

        /// <summary>使用者按下 Delete 按鈕時觸發。</summary>
        public event Action onDelete;

        /// <summary>Frame 欄位變更時觸發，帶新的 frame 值。</summary>
        public event Action<int> onFrameChanged;

        // ── Public API ─────────────────────────────────────────

        public void Dispose()
        {
            _tree?.Dispose();
            _tree = null;
        }

        /// <summary>
        /// 顯示指定的 EventData 面板。data 為 null 時隱藏。
        /// </summary>
        public void SetData(
            EventData data,
            float     timeSec,
            float     frameRate,
            int       index,
            int       totalCount,
            bool      forceRefresh = false)
        {
            if (data == null) { Hide(); return; }

            style.display = DisplayStyle.Flex;
            UpdateNavigationInfo(timeSec, frameRate, index, totalCount);
            RefreshPropertyTree(data, forceRefresh);
        }

        // ── Internal ───────────────────────────────────────────

        void Hide()
        {
            style.display = DisplayStyle.None;
            _data         = null;
            _tree?.Dispose();
            _tree = null;
        }

        void UpdateNavigationInfo(float timeSec, float frameRate, int index, int totalCount)
        {
            _infoLabel.text = $"{timeSec:F2}s  /";
            var frame = frameRate > 0 ? Mathf.RoundToInt(timeSec * frameRate) : 0;
            _frameField.SetValueWithoutNotify(frame);

            _btnPrev.SetEnabled(index > 0);
            _btnNext.SetEnabled(index < totalCount - 1);
        }

        void RefreshPropertyTree(EventData data, bool forceRefresh)
        {
            if (_data == data && !forceRefresh) return;

            _data = data;
            _lastSerializedData = EventDataSerializer.Serialize(_data);
            _tree?.Dispose();
            _tree = PropertyTree.Create(_data);
        }

        void OnGUI()
        {
            if (_tree == null) return;

            _tree.Draw(false);

            if (Event.current.type != EventType.Repaint) return;

            EditorApplication.delayCall -= DeferredSave;
            EditorApplication.delayCall += DeferredSave;
        }

        void DeferredSave()
        {
            if (_data == null) return;

            var current = EventDataSerializer.Serialize(_data);
            if (current == _lastSerializedData) return;

            _lastSerializedData = current;
            onSave?.Invoke(_data);
        }

        // ── VisualElement Factories ────────────────────────────

        void StyleSetting()
        {
            style.paddingTop    = Layout.Padding;
            style.paddingBottom = Layout.Padding;
            style.paddingLeft   = Layout.Padding;
            style.paddingRight  = Layout.Padding;
            style.backgroundColor   = Styles.InspectorBg;
            style.borderTopWidth    = 1; style.borderBottomWidth = 1;
            style.borderLeftWidth   = 1; style.borderRightWidth  = 1;
            style.borderTopColor    = Styles.Border;
            style.borderBottomColor = Styles.Border;
            style.borderLeftColor   = Styles.Border;
            style.borderRightColor  = Styles.Border;
            style.display           = DisplayStyle.None;
        }

        VisualElement NavigationHeader()
        {
            var row = new VisualElement
            {
                style = {
                    flexDirection  = FlexDirection.Row,
                    justifyContent = Justify.SpaceBetween,
                    alignItems     = Align.Center,
                    marginBottom   = Layout.MarginBottom
                }
            };

            _btnPrev = new Button(() => onNavigate?.Invoke(-1)) { text = "◄", style = { width = Layout.NavBtnWidth } };
            _btnNext = new Button(() => onNavigate?.Invoke(+1)) { text = "►", style = { width = Layout.NavBtnWidth } };

            var center = new VisualElement { style = { flexDirection = FlexDirection.Row, alignItems = Align.Center } };

            _infoLabel = new Label
            {
                style = { marginRight = Layout.MarginBottom, unityFontStyleAndWeight = FontStyle.Bold }
            };

            _frameField = new IntegerField { isDelayed = true, style = { width = Layout.FrameFieldWidth } };
            _frameField.RegisterValueChangedCallback(e => onFrameChanged?.Invoke(e.newValue));

            center.Add(_infoLabel);
            center.Add(_frameField);
            center.Add(new Label(" f"));

            row.Add(_btnPrev);
            row.Add(center);
            row.Add(_btnNext);

            return row;
        }

        VisualElement OdinContainer()
        {
            var box = new VisualElement
            {
                style = {
                    backgroundColor = Styles.InspectorBox,
                    paddingTop      = Layout.Padding, paddingBottom = Layout.Padding,
                    paddingLeft     = Layout.Padding, paddingRight  = Layout.Padding
                }
            };
            box.Add(new IMGUIContainer(OnGUI) { style = { flexGrow = 1 } });
            return box;
        }

        VisualElement Footer()
        {
            var row = new VisualElement
            {
                style = {
                    flexDirection  = FlexDirection.Row,
                    justifyContent = Justify.FlexEnd,
                    marginTop      = Layout.MarginBottom
                }
            };

            _btnDelete = new Button(() => onDelete?.Invoke())
            {
                text  = "Delete",
                style = {
                    width           = Layout.DeleteBtnWidth,
                    backgroundColor = Styles.DeleteButton,
                    color           = Color.white
                }
            };
            row.Add(_btnDelete);

            return row;
        }

        static VisualElement Separator() => new VisualElement
        {
            style = {
                height          = Layout.SeparatorHeight,
                backgroundColor = Styles.Border,
                marginTop       = Layout.MarginBottom,
                marginBottom    = Layout.MarginBottom
            }
        };

        // ── Constants ──────────────────────────────────────────

        static class Layout
        {
            public const float Padding         = 5f;
            public const float MarginBottom    = 5f;
            public const float NavBtnWidth     = 30f;
            public const float FrameFieldWidth = 50f;
            public const float DeleteBtnWidth  = 80f;
            public const float SeparatorHeight = 1f;
        }

        static class Styles
        {
            public static readonly Color InspectorBg  = new Color(0.2f,  0.2f,  0.2f);
            public static readonly Color InspectorBox = new Color(0.25f, 0.25f, 0.25f);
            public static readonly Color Border       = new Color(0.1f,  0.1f,  0.1f);
            public static readonly Color DeleteButton = new Color(0.6f,  0.2f,  0.2f);
        }
    }
}
