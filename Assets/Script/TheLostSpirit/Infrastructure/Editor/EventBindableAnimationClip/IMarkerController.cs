using UnityEngine;

namespace TheLostSpirit.Infrastructure.Editor.EventBindableAnimationClip
{
    public interface IMarkerController
    {
        void SelectEvent(int index, bool  forceRefreshOdin = false);
        void MoveEvent(int   index, float newTime);

        float GetCurrentFrameRate();
        AnimationClip GetCurrentClip();
    }
}