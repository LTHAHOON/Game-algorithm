using UnityEngine;
using static KoiAI.AI.AIFeatureTransition;

namespace KoiAI.AI
{
    public  class AIHasTargetCondition : AITransitionCondition
    {
        public override AIFeatureTransitionType TransitionType
            => AIFeatureTransitionType.HasTarget;

        public override bool Check(AIBrain brain)
        {
            return brain.TargetContext.HasTarget;
        }

        public override GameObject GetTarget()
        {
            return null;
        }
    }
}
