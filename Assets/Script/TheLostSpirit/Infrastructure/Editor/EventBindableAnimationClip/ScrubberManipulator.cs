using UnityEngine;
using UnityEngine.UIElements;

namespace TheLostSpirit.Infrastructure.Editor.EventBindableAnimationClip
{
    public class ScrubberManipulator : Manipulator
    {
        readonly IScrubberController _controller;
        readonly TimelineTrackView   _view;

        public ScrubberManipulator(IScrubberController controller, TimelineTrackView view) {
            _controller = controller;
            _view       = view;
        }

        protected override void RegisterCallbacksOnTarget() {
            target.RegisterCallback<PointerDownEvent>(OnPointerDown);
            target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            target.RegisterCallback<PointerUpEvent>(OnPointerUp);
        }

        protected override void UnregisterCallbacksFromTarget() {
            target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
            target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
            target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
        }

        void OnPointerDown(PointerDownEvent evt) {
            if (evt.button != 0) {
                return;
            }

            target.CapturePointer(evt.pointerId);
            Update(evt.localPosition);
            evt.StopPropagation();
        }

        void OnPointerMove(PointerMoveEvent evt) {
            if (target.HasPointerCapture(evt.pointerId)) {
                Update(evt.localPosition);
                evt.StopPropagation();
            }
        }

        void OnPointerUp(PointerUpEvent evt) {
            if (target.HasPointerCapture(evt.pointerId)) {
                target.ReleasePointer(evt.pointerId);
                evt.StopPropagation();
            }
        }

        void Update(Vector2 localPos) {
            var time = _view.LocalToTime(localPos.x);
            _controller.SetTime(time);
        }
    }
}