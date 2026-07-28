using UnityEngine;

namespace KoiAI.Monster
{
    [CreateAssetMenu(fileName = "new MonsterFeatureDataBase", menuName = "KoiAI/Monster/MonsterFeatureDataBase")]
    public class MonsterFeatureDataBase : ScriptableObject
    {
        [SerializeField]
        private MonsterFeatureData[] _monsterFeatureData;

        public MonsterFeatureData GetMonsterFeatureData(MonsterFeatureDataType monsterFeatureDataType)
        {
            for (int i = 0; i < _monsterFeatureData.Length; i++)
            {
                MonsterFeatureData monsterFeatureData = _monsterFeatureData[i];
                if (monsterFeatureData.MonsterFeatureDataType == monsterFeatureDataType)
                {
                    return monsterFeatureData;
                }
            }

            return null;
        }
    }
}
