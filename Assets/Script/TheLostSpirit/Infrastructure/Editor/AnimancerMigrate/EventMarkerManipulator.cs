using UnityEngine;
using UnityEngine.UIElements;

// ================================================================
// [AnimancerMigrate] EventMarkerManipulator
// 用途：在 Marker 上拖動以移動事件 normalizedTime
// ================================================================

namespace TheLostSpirit.Infrastructure.Editor.AnimancerMigrate
{
    /// <summary>
    /// 掛載在 <see cref="AnimancerMarkerElement"/> 上，允許拖動移動事件位置。
    /// 左鍵點擊 → SelectEvent；左鍵拖動 → 移動；右鍵 → 刪除。
    /// </summary>
    public class EventMarkerManipulator : Manipulator
    {
        readonly ITimelineViewerController _controller;
        readonly TimelineTrackView _trackView;
        readonly int _eventIndex;

        bool _isDragging;

        public EventMarkerManipulator(
            ITimelineViewerController controller,
            TimelineTrackView trackView,
            int eventIndex)
        {
            _controller = controller;
            _trackView = trackView;
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

            // 點擊立即回報選取（拖動開始前就更新 OdinView）
            _controller.SelectEvent(_eventIndex);
            _controller.OnBeginDrag(); // ★ 通知 Viewer 暫停 Reload，防止 DOM 被中途清除
            evt.StopPropagation();
        }

        void OnMove(PointerMoveEvent evt)
        {
            if (!_isDragging || !target.HasPointerCapture(evt.pointerId)) return;

            var clip = _controller.GetClip();
            if (clip != null && clip.length > 0f)
            {
                var localX = _trackView.WorldToLocal(evt.position).x;
                var rawRatio = Mathf.Clamp01(localX / _trackView.contentRect.width);
                // snap to frame
                var rawTime = rawRatio * clip.length;
                var snapped = Mathf.RoundToInt(rawTime * clip.frameRate) / clip.frameRate;
                var snapRatio = Mathf.Clamp01(snapped / clip.length);
                target.style.left = Length.Percent(snapRatio * 100f);
            }

            evt.StopPropagation();
        }

        void OnUp(PointerUpEvent evt)
        {
            if (!_isDragging || !target.HasPointerCapture(evt.pointerId)) return;

            _isDragging = false;

            // ★ 先 release，再 EndDrag，最後寫入 SP（觸發 Reload）
            target.ReleasePointer(evt.pointerId);
            _controller.OnEndDrag(); // 解除 Reload 抑制

            var clip = _controller.GetClip();
            float finalRatio;

            if (clip != null && clip.length > 0f)
            {
                var localX = _trackView.WorldToLocal(evt.position).x;
                var rawRatio = Mathf.Clamp01(localX / _trackView.contentRect.width);
                var rawTime = rawRatio * clip.length;
                var snapped = Mathf.RoundToInt(rawTime * clip.frameRate) / clip.frameRate;
                finalRatio = Mathf.Clamp01(snapped / clip.length);
            }
            else
            {
                var w = _trackView.contentRect.width;
                finalRatio = w > 0
                    ? Mathf.Clamp01(_trackView.WorldToLocal(evt.position).x / w)
                    : 0f;
            }

            _controller.OnMoveEvent(_eventIndex, finalRatio);
            evt.StopPropagation();
        }
    }
}
