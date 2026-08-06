using System;
using System.Collections.Generic;
using System.Linq;
using KoiAI.Utilities;
using NaughtyAttributes;
using UnityEngine;
using static KoiAI.Enemy.EnemyFeatureTransition;

namespace KoiAI.Enemy
{
    public enum ConditionMatchMode
    {
        All,
        Any
    }

    [Serializable]
    public class EnemyConditionRule
    {
        [SerializeField]
        private EnemyFeatureTransitionType _conditionType;

        [SerializeField]
        private bool _invert;

        [ShowIf(nameof(_conditionType), EnemyFeatureTransitionType.Distance)]
        [AllowNesting]
        [SerializeField]
        private EnemyDistanceConditionData _distanceConditionData;
        
        [ShowIf(nameof(_conditionType), EnemyFeatureTransitionType.WithFeature)]
        [AllowNesting]
        [SerializeField]
        private Wrapperlist<EnemyWithFeatureConditionData> _withFeatureConditionData;

        public EnemyTransitionCondition CreateCondition()
        {
            return _conditionType switch
            {
                EnemyFeatureTransitionType.HasTarget
                    => new EnemyHasTargetCondition(),

                EnemyFeatureTransitionType.Distance
                    => new EnemyDistanceCondition(
                        _distanceConditionData),

                EnemyFeatureTransitionType.WithFeature
                    => new EnemyWithFeatureCondition(
                        _withFeatureConditionData.ListValue),
                
                _ => null
            };
        }

        public bool Invert => _invert;
    }
}
