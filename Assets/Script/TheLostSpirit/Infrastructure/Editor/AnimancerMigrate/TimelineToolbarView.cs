using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

// ================================================================
// [AnimancerMigrate] TimelineToolbarView
// 職責：時間軸頂部 Toolbar。
//       對應原版 EventBindableAnimationClipDrawer/TimelineToolbarView。
// ================================================================

namespace TheLostSpirit.Infrastructure.Editor.AnimancerMigrate
{
    /// <summary>
    /// 包含 AddEvent 圖示按鈕 與 Playhead 時間顯示 Label。
    /// </summary>
    public class TimelineToolbarView : VisualElement
    {
        readonly Button _btnAdd;
        readonly Label  _timeLabel;

        public TimelineToolbarView()
        {
            StyleSetting();

            _btnAdd = BuildAddButton();
            Add(_btnAdd);

            _timeLabel = new Label("--")
            {
                style = {
                    fontSize       = 10,
                    color          = Styles.InfoText,
                    flexGrow       = 1,
                    unityTextAlign = TextAnchor.MiddleRight
                }
            };
            Add(_timeLabel);
        }

        // ── Events ─────────────────────────────────────────────
        /// <summary>使用者按下 Add Event 按鈕時觸發。</summary>
        public event Action onAddEvent;

        // ── Public API ─────────────────────────────────────────

        /// <summary>更新 Playhead 時間顯示文字。</summary>
        public void SetTime(float timeSec, float frameRate)
        {
            var frame = frameRate > 0 ? Mathf.RoundToInt(timeSec * frameRate) : 0;
            _timeLabel.text = $"f{frame}  {timeSec:F2}s";
        }

        /// <summary>設定 AddEvent 按鈕是否可用。</summary>
        public void SetAddEnabled(bool enabled) => _btnAdd.SetEnabled(enabled);

        // ── Setup ──────────────────────────────────────────────

        void StyleSetting()
        {
            style.flexDirection  = FlexDirection.Row;
            style.alignItems     = Align.Center;
            style.justifyContent = Justify.SpaceBetween;
            style.marginTop      = 5;
            style.marginBottom   = 2;
        }

        Button BuildAddButton()
        {
            var btn = new Button(() => onAddEvent?.Invoke())
            {
                tooltip = "在 Playhead 位置新增 Animancer Event",
                style   = {
                    width               = 24,
                    height              = 20,
                    marginLeft          = 2,
                    backgroundImage     = (Texture2D)EditorGUIUtility.IconContent("Animation.AddEvent").image,
                    backgroundColor     = Styles.BtnNormal,
                    borderTopWidth      = 1, borderBottomWidth = 1,
                    borderLeftWidth     = 1, borderRightWidth  = 1,
                    borderTopColor      = Styles.Border,
                    borderBottomColor   = Styles.Border,
                    borderLeftColor     = Styles.Border,
                    borderRightColor    = Styles.Border,
                    backgroundSize      = new BackgroundSize(BackgroundSizeType.Contain),
                    backgroundPositionX = new BackgroundPosition(BackgroundPositionKeyword.Center),
                    backgroundPositionY = new BackgroundPosition(BackgroundPositionKeyword.Center)
                }
            };

            btn.RegisterCallback<MouseEnterEvent>(_ => btn.style.backgroundColor = Styles.BtnHover);
            btn.RegisterCallback<MouseLeaveEvent>(_ => btn.style.backgroundColor = Styles.BtnNormal);

            return btn;
        }

        // ── Styles ─────────────────────────────────────────────

        static class Styles
        {
            public static readonly Color BtnNormal = new Color(0.22f, 0.22f, 0.22f);
            public static readonly Color BtnHover  = new Color(0.35f, 0.35f, 0.35f);
            public static readonly Color Border    = new Color(0.1f,  0.1f,  0.1f);
            public static readonly Color InfoText  = new Color(0.7f,  0.7f,  0.7f);
        }
    }
}
