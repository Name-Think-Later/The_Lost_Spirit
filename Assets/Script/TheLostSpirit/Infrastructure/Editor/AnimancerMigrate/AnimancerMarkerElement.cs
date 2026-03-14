using UnityEngine;
using UnityEngine.UIElements;

// ================================================================
// [AnimancerMigrate] AnimancerMarkerElement
// 職責：單一 Marker 的 DOM 節點。
//       對應原版 EventBindableAnimationClipDrawer/MarkerElement。
// ================================================================

namespace TheLostSpirit.Infrastructure.Editor.AnimancerMigrate
{
    /// <summary>
    /// 代表時間軸上的一個事件 Marker（垂直細條）。
    /// 由 <see cref="TimelineTrackView"/> 建立並負責呼叫 SetSelected。
    /// </summary>
    public class AnimancerMarkerElement : VisualElement
    {
        public AnimancerMarkerElement(int index, bool selected)
        {
            Index = index;

            pickingMode = PickingMode.Position;

            style.position        = Position.Absolute;
            style.width           = Layout.Width;
            style.height          = Layout.Height;
            style.top             = Layout.Top;
            style.marginLeft      = -Layout.Width * 0.5f;
            style.backgroundColor = selected ? Styles.Selected : Styles.Normal;

            style.borderTopWidth    = 1;
            style.borderBottomWidth = 1;
            style.borderLeftWidth   = 1;
            style.borderRightWidth  = 1;
            style.borderTopColor    = Styles.Border;
            style.borderBottomColor = Styles.Border;
            style.borderLeftColor   = Styles.Border;
            style.borderRightColor  = Styles.Border;
        }

        public int Index { get; }

        public void SetSelected(bool selected)
        {
            style.backgroundColor = selected ? Styles.Selected : Styles.Normal;
        }

        // ── Layout / Styles ─────────────────────────────────────

        public static class Layout
        {
            public const float Top    = -1f;  // 略超出軌道上緣
            public const float Width  = 4f;
            public const float Height = 15f;
        }

        public static class Styles
        {
            public static readonly Color Normal   = new Color(0.7f, 0.7f, 0.7f);
            public static readonly Color Selected = new Color(1f,   0.7f, 0.2f);
            public static readonly Color Border   = Color.black;
        }
    }
}
