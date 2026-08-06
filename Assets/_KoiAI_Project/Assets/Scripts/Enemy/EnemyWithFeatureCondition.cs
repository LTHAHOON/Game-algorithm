using System;
using System.Collections.Generic;
using UnityEngine;
using static KoiAI.Enemy.EnemyFeature;
using static KoiAI.Enemy.EnemyFeatureTransition;

namespace KoiAI.Enemy
{
    
    [Serializable]
    public struct EnemyWithFeatureConditionData
    {
        [SerializeField] private EnemyFeatureProperty _featureProperty;

        [SerializeField] private bool _shouldBeActive;

        public EnemyFeatureProperty FeatureProperty => _featureProperty;
        public bool ShouldBeActive => _shouldBeActive;
    }

    public class EnemyWithFeatureCondition : EnemyTransitionCondition
    {
        private List<EnemyWithFeatureConditionData> _conditionsData;
        public override EnemyFeatureTransitionType TransitionType => EnemyFeatureTransitionType.WithFeature;

        public EnemyWithFeatureCondition(List<EnemyWithFeatureConditionData> conditionsData)
        {
            _conditionsData = conditionsData;
        }

        public override bool Check(EnemyAI owner)
        {
            foreach (var condition in _conditionsData)
            {
                bool isActive =
                    owner.IsFeatureActive(condition.FeatureProperty);

                if (isActive != condition.ShouldBeActive)
                    return false;
            }

            return true;
        }
    }
}
