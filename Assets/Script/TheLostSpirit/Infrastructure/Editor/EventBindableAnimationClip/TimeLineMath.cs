using UnityEngine;

namespace TheLostSpirit.Infrastructure.Editor.EventBindableAnimationClip
{
    static class TimelineMath
    {
        public static float FrameToTime(int frame, float rate) {
            return rate <= 0 ? 0 : frame / rate;
        }

        public static int TimeToFrame(float time, float rate) {
            return rate <= 0 ? 0 : Mathf.RoundToInt(time * rate);
        }

        public static float SnapTime(float time, float rate) {
            if (rate <= 0) {
                return time;
            }

            return Mathf.Round(time * rate) / rate;
        }

        public static float ClampTime(float time, float max) {
            return Mathf.Clamp(time, 0, max);
        }
    }
}