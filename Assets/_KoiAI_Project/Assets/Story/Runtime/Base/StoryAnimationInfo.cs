using System;
using DG.Tweening;
using UnityEngine;

namespace Story.GraphToolkit.Runtime
{
    [Serializable]
    public struct StoryAnimationInfo
    {
        [SerializeField]
        private Ease _easeType;
        [SerializeField]
        private float _delayTime;
        [SerializeField]
        private float _duration;

        public StoryAnimationInfo(Ease easeType, float delayTime, float duration)
        {
            _easeType = easeType;
            _delayTime = delayTime;
            _duration = duration;
        }
        public Ease EaseType => _easeType;
        public float DelayTime => _delayTime;
        public float Duration => _duration;
    }

}
