using Animancer;
using TheLostSpirit.Infrastructure;
using TheLostSpirit.Infrastructure.Editor.EventBindableAnimationClipDrawer;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Component = UnityEngine.Component;

// ================================================================
// [AnimancerMigrate] AnimancerClipEventDrawer
// 用途：AnimancerClipEvent 的自訂 PropertyDrawer
//       ClipTransition 欄位 + 可互動事件時間線 + EventData Odin 面板
// ================================================================

namespace TheLostSpirit.Infrastructure.Editor.AnimancerMigrate
{
    [CustomPropertyDrawer(typeof(AnimancerClipEvent))]
    public class AnimancerClipEventDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();

            // ── 1. ClipTransition 標準欄位 ────────────────────────
            var transitionProp = property.FindPropertyRelative("transition");
            var transitionField = new PropertyField(transitionProp, "Transition");
            root.Add(transitionField);

            // ── 2. EventData List（序列化但不直接渲染，由時間線管理）
            var eventDataListProp = property.FindPropertyRelative("eventDataList");

            // ── 3. 時間線 Viewer ──────────────────────────────────
            var viewer = new ClipTransitionTimelineViewer
            {
                style = { minHeight = 80 }
            };
            root.Add(viewer);

            // ── 4. OdinInspectorView（點擊 Marker 後顯示 EventData）
            var odinView = new OdinInspectorView();
            root.Add(odinView);

            // ── 5. 初次載入 ───────────────────────────────────────
            viewer.LoadFromSerializedProperty(transitionProp, eventDataListProp);

            // ── 6. 使用 closured index 追蹤當前選取 ──────────────
            int selectedIndex = -1;

            // ── 7. 訂閱 Marker 選取事件 ───────────────────────────
            viewer.onMarkerSelected += index =>
            {
                selectedIndex = index;

                if (index < 0)
                {
                    odinView.style.display = UnityEngine.UIElements.DisplayStyle.None;

                    return;
                }

                if (eventDataListProp == null || !eventDataListProp.isArray ||
                    index >= eventDataListProp.arraySize) return;

                // 取得（或建立）對應 EventData
                var elemProp = eventDataListProp.GetArrayElementAtIndex(index);
                elemProp.managedReferenceValue ??= new EventData();
                var data = elemProp.managedReferenceValue as EventData;

                var clip = transitionProp.FindPropertyRelative("_Clip")
                                         ?.objectReferenceValue as UnityEngine.AnimationClip;
                float frameRate = clip ? clip.frameRate : 60f;

                var eventsProp = transitionProp.FindPropertyRelative("_Events");
                var timesProp = eventsProp?.FindPropertyRelative("_NormalizedTimes");
                float timeSec = 0f;
                if (timesProp != null && timesProp.isArray && index < timesProp.arraySize)
                    timeSec = timesProp.GetArrayElementAtIndex(index).floatValue * (clip ? clip.length : 1f);

                int totalCount = timesProp != null ? Mathf.Max(0, timesProp.arraySize - 1) : 0;

                odinView.SetData(data, timeSec, frameRate, index, totalCount);
            };

            // ── 8. OdinView 存檔 → 寫回 SP（只訂閱一次）─────────
            odinView.onSave += data =>
            {
                if (selectedIndex < 0 || eventDataListProp == null ||
                    !eventDataListProp.isArray || selectedIndex >= eventDataListProp.arraySize)
                    return;

                Undo.RecordObject(property.serializedObject.targetObject, "Save EventData");
                eventDataListProp.GetArrayElementAtIndex(selectedIndex).managedReferenceValue = data;
                property.serializedObject.ApplyModifiedProperties();
            };

            // ── 9. OdinView 導航（◄ / ►）─────────────────────────
            odinView.onNavigate += delta =>
            {
                if (eventDataListProp == null || !eventDataListProp.isArray) return;
                int total = Mathf.Max(0, eventDataListProp.arraySize);
                int newIndex = Mathf.Clamp(selectedIndex + delta, 0, total - 1);
                if (newIndex == selectedIndex) return;
                viewer.SelectMarker(newIndex);
            };

            // ── 10. OdinView 刪除當前 Marker ──────────────────────
            odinView.onDelete += _ =>
            {
                if (selectedIndex < 0) return;
                viewer.OnRequestDeleteEvent(selectedIndex);
            };

            // ── 11. OdinView Frame 欄位變更 → 移動 Marker ─────────
            odinView.onFrameChanged += frame =>
            {
                if (selectedIndex < 0) return;
                var clip = transitionProp.FindPropertyRelative("_Clip")
                                         ?.objectReferenceValue as AnimationClip;
                if (clip == null || clip.length <= 0f || clip.frameRate <= 0f) return;
                float normalizedTime = frame / (clip.frameRate * clip.length);
                viewer.OnMoveEvent(selectedIndex, Mathf.Clamp01(normalizedTime));
            };

            // ── 12. Scrub 動畫預覽（AnimationMode）────────────────
            viewer.onTimeScrubbed += timeSec =>
            {
                var target = property.serializedObject.targetObject;
                var go = target is Component c ? c.gameObject : null;
                var clip = transitionProp.FindPropertyRelative("_Clip")
                                           ?.objectReferenceValue as AnimationClip;
                if (go == null || clip == null) return;

                if (!AnimationMode.InAnimationMode())
                    AnimationMode.StartAnimationMode();

                AnimationMode.BeginSampling();
                AnimationMode.SampleAnimationClip(go, clip, timeSec);
                AnimationMode.EndSampling();
            };

            // ── 13. Viewer 分離 → 停止 AnimationMode ──────────────
            viewer.onDetached += () =>
            {
                if (AnimationMode.InAnimationMode())
                    AnimationMode.StopAnimationMode();
            };

            // ── 14. 監聽 Clip 變更重新載入 ────────────────────────
            var clipProp = transitionProp?.FindPropertyRelative("_Clip");
            if (clipProp != null)
                root.TrackPropertyValue(
                    clipProp, _ => viewer.LoadFromSerializedProperty(transitionProp, eventDataListProp));

            root.TrackSerializedObjectValue(
                property.serializedObject,
                _ => viewer.LoadFromSerializedProperty(transitionProp, eventDataListProp)
            );

            return root;
        }
    }
}