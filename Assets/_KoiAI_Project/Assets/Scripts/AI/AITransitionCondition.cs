using UnityEngine;
using static KoiAI.AI.AIFeatureTransition;

namespace KoiAI.AI
{
    public abstract class AITransitionCondition
    {
        public abstract AIFeatureTransitionType TransitionType { get; }
        public abstract bool Check(AIBrain brain);
        public virtual GameObject GetTarget() { return null; }
    }
}
