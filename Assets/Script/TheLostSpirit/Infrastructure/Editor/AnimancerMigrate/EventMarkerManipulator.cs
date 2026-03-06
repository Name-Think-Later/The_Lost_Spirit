using UnityEngine;
using UnityEngine.UIElements;

// ================================================================
// [AnimancerMigrate] EventMarkerManipulator
// 用途：在 Marker 上拖動以移動事件 normalizedTime
// ================================================================

namespace TheLostSpirit.Infrastructure.Editor.AnimancerMigrate
{
    /// <summary>
    /// 掛載在單一 Marker VisualElement 上，允許拖動移動事件位置。
    /// </summary>
    public class EventMarkerManipulator : Manipulator
    {
        readonly ITimelineViewerController _controller;
        readonly VisualElement             _track;
        readonly int                       _eventIndex;

        bool _isDragging;

        public EventMarkerManipulator(ITimelineViewerController controller, VisualElement track, int eventIndex)
        {
            _controller = controller;
            _track      = track;
            _eventIndex = eventIndex;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerDownEvent>(OnDown);
            target.RegisterCallback<PointerMoveEvent>(OnMove);
            target.RegisterCallback<PointerUpEvent>(OnUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(OnDown);
            target.UnregisterCallback<PointerMoveEvent>(OnMove);
            target.UnregisterCallback<PointerUpEvent>(OnUp);
        }

        void OnDown(PointerDownEvent evt)
        {
            if (evt.button == 1)
            {
                // 右鍵刪除
                _controller.OnRequestDeleteEvent(_eventIndex);
                evt.StopPropagation();
                return;
            }

            if (evt.button != 0) return;

            _isDragging = true;
            target.CapturePointer(evt.pointerId);
            evt.StopPropagation();
        }

        void OnMove(PointerMoveEvent evt)
        {
            if (!_isDragging || !target.HasPointerCapture(evt.pointerId)) return;

            // 換算成相對 track 寬度的 ratio
            var trackLocal = _track.WorldToLocal(evt.position);
            var w          = _track.contentRect.width;
            var ratio      = w > 0 ? Mathf.Clamp01(trackLocal.x / w) : 0f;

            // 即時更新 Marker 視覺位置（不寫入 SP，避免頻繁序列化）
            target.style.left = Length.Percent(ratio * 100f);
            evt.StopPropagation();
        }

        void OnUp(PointerUpEvent evt)
        {
            if (!_isDragging || !target.HasPointerCapture(evt.pointerId)) return;

            _isDragging = false;
            target.ReleasePointer(evt.pointerId);

            // 確認最終位置並寫入 SerializedProperty
            var trackLocal = _track.WorldToLocal(evt.position);
            var w          = _track.contentRect.width;
            var ratio      = w > 0 ? Mathf.Clamp01(trackLocal.x / w) : 0f;

            _controller.OnMoveEvent(_eventIndex, ratio);
            evt.StopPropagation();
        }
    }
}
