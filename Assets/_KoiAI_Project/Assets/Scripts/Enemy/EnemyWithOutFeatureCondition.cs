using System;
using System.Collections.Generic;
using UnityEngine;
using static KoiAI.Enemy.EnemyFeature;
using static KoiAI.Enemy.EnemyFeatureTransition;

namespace KoiAI.Enemy
{
    [Serializable]
    public class EnemyWithOutFeatureData
    {
        [SerializeField]
        private List<EnemyFeatureProperty> _enemyFeatureProperties;

        public List<EnemyFeatureProperty> EnemyFeatureProperties => _enemyFeatureProperties;
    }

    public class EnemyWithOutFeatureCondition : EnemyTransitionCondition
    {
        private EnemyWithOutFeatureData _enemyWithOutFeatureData;
        public override EnemyFeatureTransitionType TransitionType => EnemyFeatureTransitionType.WithOutFeature;

        public EnemyWithOutFeatureCondition(EnemyWithOutFeatureData enemyWithOutFeatureData)
        {
            _enemyWithOutFeatureData = enemyWithOutFeatureData;
        }

        public override bool Check(EnemyAI owner)
        {

        }
    }
}
