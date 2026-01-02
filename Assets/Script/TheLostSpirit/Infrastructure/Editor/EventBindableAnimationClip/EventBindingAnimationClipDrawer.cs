using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

// ##################################################################
// [Entry Point] PropertyDrawer for RefactoringEvent BindableAnimationClip
// ##################################################################

namespace TheLostSpirit.Infrastructure.Editor.EventBindableAnimationClip
{
    [CustomPropertyDrawer(typeof(global::TheLostSpirit.Infrastructure.EventBindableAnimationClip))]
    public class RefactoringEventBindableAnimationClipDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property) {
            var root = new VisualElement();

            // 1. Draw default Clip field
            var animationClipProp  = property.FindPropertyRelative(nameof(TheLostSpirit.Infrastructure.EventBindableAnimationClip.inner));
            var animationClipField = new PropertyField(animationClipProp, "Animation Clip");
            root.Add(animationClipField);

            // 2. Initialize Timeline Editor
            var editor = new TimelineEditor(root, property, animationClipProp);

            // Monitor Clip changes to refresh editor
            root.TrackPropertyValue(animationClipProp, _ => editor.Refresh());

            return root;
        }
    }
}