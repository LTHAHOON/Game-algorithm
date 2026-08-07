using System;
using KoiAI.Utilities;
using NaughtyAttributes;
using UnityEngine;
using static KoiAI.AI.AIFeatureTransition;

namespace KoiAI.AI
{
    public enum ConditionMatchMode
    {
        All,
        Any
    }

    [Serializable]
    public class AIConditionRule
    {
        [SerializeField]
        private AIFeatureTransitionType _conditionType;

        [SerializeField]
        private bool _invert;

        [ShowIf(nameof(_conditionType), AIFeatureTransitionType.Distance)]
        [AllowNesting]
        [SerializeField]
        private AIDistanceConditionData _distanceConditionData;
        
        [ShowIf(nameof(_conditionType), AIFeatureTransitionType.WithFeature)]
        [AllowNesting]
        [SerializeField]
        private Wrapperlist<AIWithFeatureConditionData> _withFeatureConditionData;

        public AITransitionCondition CreateCondition()
        {
            return _conditionType switch
            {
                AIFeatureTransitionType.HasTarget
                    => new AIHasTargetCondition(),

                AIFeatureTransitionType.Distance
                    => new AIDistanceCondition(
                        _distanceConditionData),

                AIFeatureTransitionType.WithFeature
                    => new AIWithFeatureCondition(
                        _withFeatureConditionData.ListValue),
                
                _ => null
            };
        }

        public bool Invert => _invert;
    }
}
