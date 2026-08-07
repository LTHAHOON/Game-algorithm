using UnityEngine;

namespace KoiAI.AI
{
    public class AITargetContext
    {
        public Transform Target { get; private set; }
        public float Distance { get; private set; }
        public bool HasTarget => Target != null;

        public void SetTarget(Transform owner, Transform target)
        {
            Target = target;

            Distance = target ? Vector3.Distance(owner.position, target.position) : float.MaxValue;
        }

        public void Clear()
        {
            Target = null;
            Distance = float.MaxValue;
        }
    }
}
