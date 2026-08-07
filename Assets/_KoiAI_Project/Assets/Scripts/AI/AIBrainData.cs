using System.Collections.Generic;
using UnityEngine;

namespace KoiAI.AI
{
    [CreateAssetMenu(fileName = "new AIBrainData", menuName = "KoiAI/AI/AIBrainData")]
    public class AIBrainData : ScriptableObject
    {
        [SerializeField]
        private List<AIFeatureTransition> _aiFeatureTransitions;

        public IReadOnlyList<AIFeatureTransition> FeatureTransitions => _aiFeatureTransitions;
    }
}
