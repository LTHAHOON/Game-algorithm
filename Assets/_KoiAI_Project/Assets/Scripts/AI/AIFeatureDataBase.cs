using UnityEngine;

namespace KoiAI.AI
{
    [CreateAssetMenu(fileName = "new EnemyFeatureDataBase", menuName = "KoiAI/Enemy/EnemyFeatureDataBase")]
    public class AIFeatureDataBase : ScriptableObject
    {
        [SerializeField]
        private AIFeatureData[] _enemyFeatureData;

        public AIFeatureData GetEnemyFeatureData(AIFeatureDataType enemyFeatureDataType)
        {
            for (int i = 0; i < _enemyFeatureData.Length; i++)
            {
                AIFeatureData enemyFeatureData = _enemyFeatureData[i];
                if (enemyFeatureData.EnemyFeatureDataType == enemyFeatureDataType)
                {
                    return enemyFeatureData;
                }
            }

            return null;
        }
    }
}
