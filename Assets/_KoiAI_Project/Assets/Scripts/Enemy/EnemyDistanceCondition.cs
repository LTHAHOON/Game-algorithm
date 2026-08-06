using System;
using KoiAI.Utilities;
using UnityEngine;

namespace KoiAI.Enemy
{
    [Serializable]
    public class EnemyDistanceConditionData
    {
        [SerializeField]
        private bool _useOriginDistance;
        [SerializeField]
        private CompareValueCondition<float> _distanceCondition;
                
        public bool UseOriginDistance => _useOriginDistance;
        public CompareValueCondition<float> DistanceCondition
            => _distanceCondition;
    }
    public class EnemyDistanceCondition : EnemyTransitionCondition
    {
        private readonly EnemyDistanceConditionData _conditionData;

        public EnemyDistanceCondition(
            EnemyDistanceConditionData conditionData)
        {
            
            _conditionData = conditionData;
        }

        public override EnemyFeatureTransition.EnemyFeatureTransitionType TransitionType => EnemyFeatureTransition.EnemyFeatureTransitionType.Distance;

        public override bool Check(EnemyAI owner)
        {
            if (_conditionData == null || owner == null)
            {
                return false;
            }

            bool result = false;
            if (_conditionData.UseOriginDistance)
            {
                float distance = (owner.OriginPosition - owner.transform.position).sqrMagnitude;
                result = distance.CompareWithCondition(_conditionData.DistanceCondition);
            }
            else
            {
                if (!owner.TargetContext.HasTarget)
                {
                    return false;
                }
                
                result   = owner.TargetContext.Distance.CompareWithCondition(_conditionData.DistanceCondition);
            }
            return result;
        }
    }
}
