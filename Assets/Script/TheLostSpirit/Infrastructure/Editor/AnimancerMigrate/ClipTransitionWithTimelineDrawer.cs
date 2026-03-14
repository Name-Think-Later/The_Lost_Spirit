using System;
using Animancer;
using TheLostSpirit.Infrastructure;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Component = UnityEngine.Component;

// ================================================================
// [AnimancerMigrate] ClipTransitionWithTimelineDrawer
// 用途：AnimancerClipEvent 的自訂 PropertyDrawer
//       ClipTransition 欄位 + 可互動事件時間線 + EventData Odin 面板
// ================================================================

namespace TheLostSpirit.Infrastructure.Editor.AnimancerMigrate
{
    [CustomPropertyDrawer(typeof(AnimancerClipEvent))]
    public class ClipTransitionWithTimelineDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();

            // ── 1. ClipTransition 標準欄位 ────────────────────────
            var transitionProp = property.FindPropertyRelative("transition");
            var transitionField = new PropertyField(transitionProp, "Transition");
            root.Add(transitionField);

            // ── 2. EventData List（由時間線管理，不直接渲染）────────
            var eventDataListProp = property.FindPropertyRelative("eventDataList");

            // ── 3. 時間線 Viewer ──────────────────────────────────
            var viewer = new ClipTransitionTimelineViewer { style = { minHeight = 80 } };
            root.Add(viewer);

            // ── 4. Odin Inspector Panel ───────────────────────────
            var odinView = new OdinInspectorView();
            root.Add(odinView);

            // ── 5. 初次載入 ───────────────────────────────────────
            viewer.LoadFromSerializedProperty(transitionProp, eventDataListProp);

            // ── 6. 當前選取 index ──────────────────────────────────
            int selectedIndex = -1;

            // ── 7. Marker 選取 → 更新 OdinView ───────────────────
            viewer.onMarkerSelected += index =>
            {
                selectedIndex = index;

                if (index < 0)
                {
                    odinView.style.display = DisplayStyle.None;
                    viewer.HideDurationBar();
                    return;
                }

                if (eventDataListProp == null || !eventDataListProp.isArray ||
                    index >= eventDataListProp.arraySize) return;

                var elemProp = eventDataListProp.GetArrayElementAtIndex(index);
                elemProp.managedReferenceValue ??= new EventData();
                var data = elemProp.managedReferenceValue as EventData;

                var clip = transitionProp.FindPropertyRelative("_Clip")?.objectReferenceValue as AnimationClip;
                float frameRate = clip ? clip.frameRate : 60f;
                float clipLen = clip ? clip.length : 1f;

                var timesProp = transitionProp.FindPropertyRelative("_Events")
                                             ?.FindPropertyRelative("_NormalizedTimes");
                float timeSec = 0f;
                if (timesProp != null && timesProp.isArray && index < timesProp.arraySize)
                    timeSec = timesProp.GetArrayElementAtIndex(index).floatValue * clipLen;

                int totalCount = timesProp != null ? Mathf.Max(0, timesProp.arraySize - 1) : 0;

                odinView.SetData(data, timeSec, frameRate, index, totalCount);

                // ── 展示 DurationFrame 色塊 ────────────────────────
                viewer.UpdateDurationVisuals(data, index, timeSec);
            };

            // ── 8. OdinView 存檔 → 寫回 SP ───────────────────────
            odinView.onSave += data =>
            {
                if (selectedIndex < 0 || eventDataListProp == null ||
                    !eventDataListProp.isArray || selectedIndex >= eventDataListProp.arraySize) return;

                Undo.RecordObject(property.serializedObject.targetObject, "Save EventData");
                eventDataListProp.GetArrayElementAtIndex(selectedIndex).managedReferenceValue = data;

                // CombatStep 可能改了 DurationFrames，立即刷新色塊
                var clip2 = transitionProp.FindPropertyRelative("_Clip")?.objectReferenceValue as AnimationClip;
                var timesProp2 = transitionProp.FindPropertyRelative("_Events")?.FindPropertyRelative("_NormalizedTimes");
                float timeSec2 = 0f;
                if (timesProp2 != null && timesProp2.isArray && selectedIndex < timesProp2.arraySize)
                    timeSec2 = timesProp2.GetArrayElementAtIndex(selectedIndex).floatValue * (clip2 ? clip2.length : 1f);
                viewer.UpdateDurationVisuals(data, selectedIndex, timeSec2);

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

            // ── 10. OdinView Delete ────────────────────────────────
            odinView.onDelete += () =>
            {
                if (selectedIndex < 0) return;
                viewer.OnRequestDeleteEvent(selectedIndex);
            };

            // ── 11. OdinView Frame 欄位 → 移動 Marker ─────────────
            odinView.onFrameChanged += frame =>
            {
                if (selectedIndex < 0) return;
                var clip = transitionProp.FindPropertyRelative("_Clip")?.objectReferenceValue as AnimationClip;
                if (clip == null || clip.length <= 0f || clip.frameRate <= 0f) return;
                float normalizedTime = frame / (clip.frameRate * clip.length);
                viewer.OnMoveEvent(selectedIndex, Mathf.Clamp01(normalizedTime));
            };

            // ── 12. Scrub → AnimationMode 預覽 ─────────────────────
            viewer.onTimeScrubbed += timeSec =>
            {
                var target = property.serializedObject.targetObject;
                var go = target is Component c ? c.gameObject : null;
                var clip = transitionProp.FindPropertyRelative("_Clip")?.objectReferenceValue as AnimationClip;
                if (go == null || clip == null) return;

                if (!AnimationMode.InAnimationMode()) AnimationMode.StartAnimationMode();
                AnimationMode.BeginSampling();
                AnimationMode.SampleAnimationClip(go, clip, timeSec);
                AnimationMode.EndSampling();
            };

            // ── 13. Viewer 分離 / SceneView 訂閱 ──────────────────
            // 設定 CharacterTarget 給 DrawDebugRange
            if (property.serializedObject.targetObject is Component comp)
                viewer.SetCharacterTarget(comp.gameObject);

            Action<SceneView> sceneGuiCallback = _ => viewer.DrawDebugRange();
            SceneView.duringSceneGui += sceneGuiCallback;

            viewer.onDetached += () =>
            {
                if (AnimationMode.InAnimationMode()) AnimationMode.StopAnimationMode();
                SceneView.duringSceneGui -= sceneGuiCallback;
            };

            // ── 14. 監聽 SP 變更重新載入 ──────────────────────────
            var clipProp = transitionProp?.FindPropertyRelative("_Clip");
            if (clipProp != null)
                root.TrackPropertyValue(
                    clipProp, _ => viewer.LoadFromSerializedProperty(transitionProp, eventDataListProp));

            root.TrackSerializedObjectValue(
                property.serializedObject,
                _ => viewer.LoadFromSerializedProperty(transitionProp, eventDataListProp));

            return root;
        }
    }
}