using UnityEngine;

namespace KoiAI.Enemy
{
    [CreateAssetMenu(fileName = "new MonsterFeatureDataBase", menuName = "KoiAI/Monster/MonsterFeatureDataBase")]
    public class EnemyFeatureDataBase : ScriptableObject
    {
        [SerializeField]
        private EnemyFeatureData[] _monsterFeatureData;

        public EnemyFeatureData GetMonsterFeatureData(EnemyFeatureDataType monsterFeatureDataType)
        {
            for (int i = 0; i < _monsterFeatureData.Length; i++)
            {
                EnemyFeatureData monsterFeatureData = _monsterFeatureData[i];
                if (monsterFeatureData.MonsterFeatureDataType == monsterFeatureDataType)
                {
                    return monsterFeatureData;
                }
            }

            return null;
        }
    }
}
