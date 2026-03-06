using Animancer;
using TheLostSpirit.Infrastructure;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

// ================================================================
// [AnimancerMigrate] AnimancerClipEventDrawer
// 用途：AnimancerClipEvent（包含 ClipTransition）的自訂 PropertyDrawer
//       在 ClipTransition 的標準欄位下方顯示唯讀事件時間線
// ================================================================

namespace TheLostSpirit.Infrastructure.Editor.AnimancerMigrate
{
    [CustomPropertyDrawer(typeof(AnimancerClipEvent))]
    public class AnimancerClipEventDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();

            // ── 1. ClipTransition 欄位（標準繪製） ───────────────
            var transitionProp = property.FindPropertyRelative("transition");
            var transitionField = new PropertyField(transitionProp, "Transition");
            root.Add(transitionField);

            // ── 2. 唯讀事件時間線 ─────────────────────────────────
            var viewer = new ClipTransitionTimelineViewer
            {
                style = { height = 80 }
            };
            root.Add(viewer);

            // ── 3. 初次載入 ────────────────────────────────────────
            viewer.LoadFromSerializedProperty(transitionProp);

            // ── 4. 監聽 _Clip 變更刷新（ClipTransition._Clip）───────
            var clipProp = transitionProp?.FindPropertyRelative("_Clip");
            if (clipProp != null)
            {
                root.TrackPropertyValue(clipProp, _ => viewer.LoadFromSerializedProperty(transitionProp));
            }

            // ── 5. 監聽整個 SerializedObject（Events 變更時刷新）──
            root.TrackSerializedObjectValue(
                property.serializedObject,
                _ => viewer.LoadFromSerializedProperty(transitionProp)
            );

            return root;
        }
    }
}
