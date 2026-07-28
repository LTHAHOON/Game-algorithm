
using NaughtyAttributes;
using UnityEngine;
using static KoiAI.Monster.MonsterFeature;
using static KoiAI.Player.PlayerFeature;

namespace KoiAI.Monster
{
    using KoiAI.AnimatorSystem;

    [CreateAssetMenu(fileName = "new MonsterData", menuName = "KoiAI/Monster/MonsterData")]
    public class MonsterData : ScriptableObject
    {
        [SerializeField]
        private string _characterBaseName;

        [Space(10)]
        [HorizontalLine(5, EColor.Gray)]
        [Space(10)]
        [SerializeField]
        private AnimatorData _animatorData;
        [ReadOnly]
        [SerializeField]
        private MonsterFeatureDataBase _monsterFeatureDataBase;
        [Space(10)]
        [SerializeField]
        private MonsterFeatureDataType _monsterFeatureDataType;


        [Space(10)]
        [ShowIf(nameof(HasMovementProperty))]
        [SerializeField]
        private MonsterMovementExtensionData _monsterMovementExtensionData;
        [ShowIf(nameof(HasRotationProperty))]
        [SerializeField]
        private MonsterRotationExtensionData _monsterRotationExtensionData;

        public MonsterFeatureData GetMonsterFeatureData()
        {
            MonsterFeatureData data = _monsterFeatureDataBase?.GetMonsterFeatureData(_monsterFeatureDataType);
            return data;
        }

        public MonsterFeatureExtensionData GetMonsterFeatureExtensionData(MonsterFeatureProperty featureProperty)
        {
            return featureProperty switch
            {
                MonsterFeatureProperty.Movement => _monsterMovementExtensionData,
                MonsterFeatureProperty.Rotation => _monsterRotationExtensionData,
                _ => null
            };
        }

        public MonsterFeatureValueData GetMonsterFeatureValueData(MonsterFeatureProperty featureProperty)
        {
            MonsterFeatureData data = GetMonsterFeatureData();
            if (data)
            {
                return featureProperty switch
                {
                    MonsterFeatureProperty.Movement => data.MonsterMovementValueData,
                    MonsterFeatureProperty.Rotation => data.MonsterRotationValueData,
                    _ => null
                };
            }
            return null;
        }

        public bool HasMovementProperty => GetMonsterFeatureData() is var data && data != null && data.HasMovementProperty;
        public bool HasRotationProperty => GetMonsterFeatureData() is var data && data != null && data.HasRotationProperty;

        public AnimatorData AnimatorData => _animatorData;
    }
}
