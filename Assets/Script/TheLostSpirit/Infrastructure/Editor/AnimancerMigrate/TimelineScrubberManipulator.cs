using UnityEngine;
using UnityEngine.UIElements;

// ================================================================
// [AnimancerMigrate] TimelineScrubberManipulator
// 用途：在 Track 上拖動以移動 Playhead
// ================================================================

namespace TheLostSpirit.Infrastructure.Editor.AnimancerMigrate
{
    /// <summary>
    /// 掛載在 Track VisualElement 上，拖動時移動 Playhead。
    /// </summary>
    public class TimelineScrubberManipulator : Manipulator
    {
        readonly ITimelineViewerController _controller;
        bool _isTracking;

        public TimelineScrubberManipulator(ITimelineViewerController controller)
        {
            _controller = controller;
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
            // 右鍵：加入Event
            if (evt.button == 1)
            {
                var ratio = LocalRatio(evt.localPosition.x);
                _controller.OnRequestAddEvent(ratio);
                evt.StopPropagation();
                return;
            }

            if (evt.button != 0) return;

            _isTracking = true;
            target.CapturePointer(evt.pointerId);

            var n = LocalRatio(evt.localPosition.x);
            _controller.OnScrub(n);
            evt.StopPropagation();
        }

        void OnMove(PointerMoveEvent evt)
        {
            if (!_isTracking || !target.HasPointerCapture(evt.pointerId)) return;

            var n = LocalRatio(evt.localPosition.x);
            _controller.OnScrub(n);
            evt.StopPropagation();
        }

        void OnUp(PointerUpEvent evt)
        {
            if (!_isTracking || !target.HasPointerCapture(evt.pointerId)) return;

            _isTracking = false;
            target.ReleasePointer(evt.pointerId);
            evt.StopPropagation();
        }

        float LocalRatio(float localX)
        {
            var w = target.contentRect.width;
            return w > 0 ? Mathf.Clamp01(localX / w) : 0f;
        }
    }
}
