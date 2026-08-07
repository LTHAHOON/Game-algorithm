using NaughtyAttributes;
using R3;
using System;
using UnityEngine;
using static KoiAI.AI.AIFeature;
using static KoiAI.AI.AIFeatureTransition;

namespace KoiAI.AI
{
    [Serializable]
    public class AIFeatureTransitionRuntimeDebug
    {
        [ReadOnly]
        [AllowNesting]
        public string DEBUG_STATE;

        private bool _wasActive = false;

        public void AssignDebug(AIFeature feature)
        {
            Observable.EveryUpdate(UnityFrameProvider.PreLateUpdate)
                .Subscribe(async _ =>
                {
                    bool isActive = feature.Brain.IsFeatureActive(feature.FeatureProperty);
                    if (isActive && !_wasActive)
                    {
                        DEBUG_STATE = $"{feature.FeatureProperty.ToString()} {nameof(AIFeatureState.ENTER)}";
                    }
                    else if (isActive && _wasActive)
                    {
                        DEBUG_STATE = $"{feature.FeatureProperty.ToString()} {nameof(AIFeatureState.UPDATE)}";
                    }
                    else
                    {
                        DEBUG_STATE = $"{feature.FeatureProperty.ToString()} {nameof(AIFeatureState.EXIT)}";
                    }
                    _wasActive = isActive;
                })
                .AddTo(feature.Brain);
        }
    }

    [Serializable]
    public class AIFeatureTransitionRuntimeSetting
    {
        [SerializeField]
        private AIFeatureProperty _featureProperty;

        [SerializeField]
        private float _enableDelayTime;
        [SerializeField]
        private float _disableDelayTime;
        //여기부터 하면됨
    }

    [Serializable]
    public struct AIFeatureTransition
    {
        public enum AIFeatureTransitionType
        {
            None,
            HasTarget,
            Distance,
            WithFeature
        }

        public enum AIFeatureState
        {
            ENTER,
            UPDATE,
            EXIT
        }

        [SerializeField]
        private AIFeatureProperty _featureProperty;

        [SerializeField]
        private AIConditionGroupData _enableConditions;

        [SerializeField]
        private AIConditionGroupData _disableConditions;


        public AIFeatureProperty FeatureProperty => _featureProperty;
        public AIConditionGroupData EnableConditions => _enableConditions;
        public AIConditionGroupData DisableConditions => _disableConditions;
    }
}
