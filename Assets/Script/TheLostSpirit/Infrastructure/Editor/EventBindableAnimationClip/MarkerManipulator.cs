using UnityEngine.UIElements;

namespace TheLostSpirit.Infrastructure.Editor.EventBindableAnimationClip
{
    public class MarkerManipulator : Manipulator
    {
        readonly IMarkerController _controller;
        readonly int               _index;
        readonly TimelineTrackView _trackView;

        bool _isDragging;

        public MarkerManipulator(IMarkerController controller, int index, TimelineTrackView trackView) {
            _controller = controller;
            _index      = index;
            _trackView  = trackView;
        }

        protected override void RegisterCallbacksOnTarget() {
            target.RegisterCallback<PointerDownEvent>(OnDown);
            target.RegisterCallback<PointerMoveEvent>(OnMove);
            target.RegisterCallback<PointerUpEvent>(OnUp);
        }

        protected override void UnregisterCallbacksFromTarget() {
            target.UnregisterCallback<PointerDownEvent>(OnDown);
            target.UnregisterCallback<PointerMoveEvent>(OnMove);
            target.UnregisterCallback<PointerUpEvent>(OnUp);
        }

        void OnDown(PointerDownEvent evt) {
            if (evt.button != 0) {
                return;
            }

            _isDragging = true;
            target.CapturePointer(evt.pointerId);

            _controller.SelectEvent(_index);
            evt.StopPropagation();
        }

        void OnMove(PointerMoveEvent evt) {
            if (!_isDragging || !target.HasPointerCapture(evt.pointerId)) {
                return;
            }

            var trackPos    = _trackView.WorldToLocal(evt.position);
            var rawTime     = _trackView.LocalToTime(trackPos.x);
            var frameRate   = _controller.GetCurrentFrameRate();
            var snappedTime = TimelineMath.SnapTime(rawTime, frameRate);
            var clip        = _controller.GetCurrentClip();
            if (clip != null && clip.length > 0) {
                var snappedRatio = snappedTime / clip.length;
                target.style.left = Length.Percent(snappedRatio * 100f);

                var frame = TimelineMath.TimeToFrame(snappedTime, frameRate);
                target.tooltip = $"Dragging...\n({snappedTime:F2}s | f: {frame})";
            }

            // DON'T update controller time during drag to prevent playhead jumping
            evt.StopPropagation();
        }

        void OnUp(PointerUpEvent evt) {
            if (!_isDragging || !target.HasPointerCapture(evt.pointerId)) {
                return;
            }

            _isDragging = false;
            target.ReleasePointer(evt.pointerId);

            var trackPos = _trackView.WorldToLocal(evt.position);
            var rawTime  = _trackView.LocalToTime(trackPos.x);

            var frameRate   = _controller.GetCurrentFrameRate();
            var snappedTime = TimelineMath.SnapTime(rawTime, frameRate);

            _controller.MoveEvent(_index, snappedTime);
            evt.StopPropagation();
        }
    }
}