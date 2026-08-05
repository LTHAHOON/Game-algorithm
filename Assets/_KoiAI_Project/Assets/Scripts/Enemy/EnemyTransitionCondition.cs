using UnityEngine;
using static KoiAI.Enemy.EnemyFeatureTransition;

namespace KoiAI.Enemy
{
    public abstract class EnemyTransitionCondition
    {
        public abstract EnemyFeatureTransitionType TransitionType { get; }
        public abstract bool Check(EnemyAI owner);
        public virtual GameObject GetTarget() { return null; }
    }
}
