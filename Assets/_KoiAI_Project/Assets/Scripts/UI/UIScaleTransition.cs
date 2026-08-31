using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace KoiAI.UI
{
    
    [Serializable]
    public struct UIScaleTransitionData
    {
        [SerializeField]
        private Ease _easingType;
        [SerializeField]
        private float _duration;
        [SerializeField]
        private Vector2 _minScale;
        [SerializeField]
        private Vector2 _maxScale;
        [SerializeField]
        private int _loops;
        [SerializeField]
        private LoopType _loopType;

        public UIScaleTransitionData(Ease easingType, float duration, Vector2 minScale, Vector2 maxScale, int loops, LoopType loopType)
        {
            _easingType = easingType;
            _duration = duration;
            _minScale = minScale;
            _maxScale = maxScale;
            _loops = loops;
            _loopType = loopType;
        }

        public Ease EasingType => _easingType;
        public float Duration => _duration;
        public Vector2 MinScale => _minScale;
        public Vector2 MaxScale => _maxScale;
        public int Loops => _loops;
        public LoopType LoopType => _loopType;
    }

    public class UIScaleTransition
    {
        private UIScaleTransitionData _scaleTransitionData;
        private readonly VisualElement _uiTarget;
        private readonly GameObject _caller;
        private Tween targetTween;

        public UIScaleTransition(GameObject caller, VisualElement uiTarget, UIScaleTransitionData scaleTransitionData)
        {
            if(!caller || uiTarget == null)
            {
                return;
            }
            _caller = caller;
            _uiTarget = uiTarget;
            _scaleTransitionData = scaleTransitionData;
        }
        public UIScaleTransition(GameObject caller, Ease easingType,VisualElement uiTarget, float duration, Vector2 minScale, Vector2 maxScale, int loops, LoopType loopType)
        {
            if (!caller || uiTarget == null)
            {
                return;
            }
            _caller = caller;
            _uiTarget = uiTarget;
            _scaleTransitionData = new(easingType, duration, minScale, maxScale, loops, loopType);
        }

        public void ActivateTransition()
        {
            if(!_caller || _uiTarget == null)
            {
                return;
            }

            targetTween = DOTween.To(
                () => _scaleTransitionData.MinScale,
                x =>
                {
                    _uiTarget.style.scale = x;
                },
                _scaleTransitionData.MaxScale,
                _scaleTransitionData.Duration
                )
                .SetId(_uiTarget)
                .SetEase(_scaleTransitionData.EasingType)
                .SetLoops(_scaleTransitionData.Loops, _scaleTransitionData.LoopType)
                .SetLink(_caller, LinkBehaviour.KillOnDestroy)
                .OnComplete(() => StopTransition());
        }

        public void StopTransition()
        {
            if(targetTween == null)
            {
                return;
            }
            targetTween.Kill();
        }

    }
}
