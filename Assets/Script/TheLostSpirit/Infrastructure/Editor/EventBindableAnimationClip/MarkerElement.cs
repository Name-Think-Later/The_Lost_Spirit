using UnityEngine;
using UnityEngine.UIElements;

namespace TheLostSpirit.Infrastructure.Editor.EventBindableAnimationClip
{
    public class MarkerElement : VisualElement
    {
        public MarkerElement(int index, bool selected) {
            Index = index;

            pickingMode = PickingMode.Position;

            style.position        = Position.Absolute;
            style.width           = Layout.Width;
            style.height          = Layout.Height;
            style.backgroundColor = selected ? Styles.Selected : Styles.Normal;

            style.borderTopWidth    = 1;
            style.borderBottomWidth = 1;
            style.borderLeftWidth   = 1;
            style.borderRightWidth  = 1;
            style.borderTopColor    = Color.black;
            style.borderBottomColor = Color.black;
            style.borderLeftColor   = Color.black;
            style.borderRightColor  = Color.black;

            style.marginLeft = -Layout.Width / 2f;
        }

        public int Index { get; }

        public void SetSelected(bool selected) {
            style.backgroundColor =
                selected ? Styles.Selected : Styles.Normal;
        }

        public static class Layout
        {
            public const float Top    = -1f;
            public const float Width  = 4f;
            public const float Height = 15f;
        }

        public static class Styles
        {
            public static readonly Color Normal   = new Color(0.7f, 0.7f, 0.7f);
            public static readonly Color Selected = new Color(1f, 0.7f, 0.2f);
        }
    }
}