using UnityEngine;

namespace KoiAI.Enemy
{
    [CreateAssetMenu(fileName = "new EnemyFeatureDataBase", menuName = "KoiAI/Enemy/EnemyFeatureDataBase")]
    public class EnemyFeatureDataBase : ScriptableObject
    {
        [SerializeField]
        private EnemyFeatureData[] _enemyFeatureData;

        public EnemyFeatureData GetEnemyFeatureData(EnemyFeatureDataType enemyFeatureDataType)
        {
            for (int i = 0; i < _enemyFeatureData.Length; i++)
            {
                EnemyFeatureData enemyFeatureData = _enemyFeatureData[i];
                if (enemyFeatureData.EnemyFeatureDataType == enemyFeatureDataType)
                {
                    return enemyFeatureData;
                }
            }

            return null;
        }
    }
}
