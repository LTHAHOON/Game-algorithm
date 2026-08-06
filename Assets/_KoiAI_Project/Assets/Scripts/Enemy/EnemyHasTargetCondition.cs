using UnityEngine;
using static KoiAI.Enemy.EnemyFeatureTransition;

namespace KoiAI.Enemy
{
    public  class EnemyHasTargetCondition : EnemyTransitionCondition
    {
        public override EnemyFeatureTransitionType TransitionType
            => EnemyFeatureTransitionType.HasTarget;

        public override bool Check(EnemyAI owner)
        {
            return owner.TargetContext.HasTarget;
        }

        public override GameObject GetTarget()
        {
            return null;
        }
    }
}
