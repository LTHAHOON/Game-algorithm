using System;
using System.Collections.Generic;
using UnityEngine;
using static KoiAI.AI.AIFeature;
using static KoiAI.AI.AIFeatureTransition;

namespace KoiAI.AI
{
    
    [Serializable]
    public struct AIWithFeatureConditionData
    {
        [SerializeField] private AIFeatureProperty _featureProperty;

        [SerializeField] private bool _shouldBeActive;

        public AIFeatureProperty FeatureProperty => _featureProperty;
        public bool ShouldBeActive => _shouldBeActive;
    }

    public class AIWithFeatureCondition : AITransitionCondition
    {
        private List<AIWithFeatureConditionData> _conditionsData;
        public override AIFeatureTransitionType TransitionType => AIFeatureTransitionType.WithFeature;

        public AIWithFeatureCondition(List<AIWithFeatureConditionData> conditionsData)
        {
            _conditionsData = conditionsData;
        }

        public override bool Check(AIBrain brain)
        {
            foreach (var condition in _conditionsData)
            {
                bool isActive =
                    brain.IsFeatureActive(condition.FeatureProperty);

                if (isActive != condition.ShouldBeActive)
                    return false;
            }

            return true;
        }
    }
}
