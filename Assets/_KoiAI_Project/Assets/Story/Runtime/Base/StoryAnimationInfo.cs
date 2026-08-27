using System;
using DG.Tweening;

namespace Story.GraphToolkit.Runtime
{
    [Serializable]
    public struct StoryAnimationInfo
    {
        public Ease EaseType;
        public float DelayTime;
        public float Duration;

        public StoryAnimationInfo(Ease easeType, float delayTime, float duration)
        {
            EaseType = easeType;
            DelayTime = delayTime;
            Duration = duration;
        }
    }

}
