using UnityEngine;

namespace TheLostSpirit.Infrastructure.Editor.EventBindableAnimationClipDrawer
{
    public interface IMarkerController
    {
        void SelectEvent(int index, bool  forceRefreshOdin = false);
        void MoveEvent(int   index, float newTime);

        float GetCurrentFrameRate();
        AnimationClip GetCurrentClip();
    }
}