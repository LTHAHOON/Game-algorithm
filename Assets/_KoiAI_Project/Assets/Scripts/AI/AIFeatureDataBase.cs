using UnityEngine;

namespace KoiAI.AI
{
    [CreateAssetMenu(fileName = "new EnemyFeatureDataBase", menuName = "KoiAI/Enemy/EnemyFeatureDataBase")]
    public class AIFeatureDataBase : ScriptableObject
    {
        [SerializeField]
        private AIFeatureData[] _aiFeatureData;

        public AIFeatureData GetAIFeatureData(AIFeatureDataType aiFeatureDataType)
        {
            for (int i = 0; i < _aiFeatureData.Length; i++)
            {
                AIFeatureData enemyFeatureData = _aiFeatureData[i];
                if (enemyFeatureData.AIFeatureDataType == aiFeatureDataType)
                {
                    return enemyFeatureData;
                }
            }

            return null;
        }
    }
}
