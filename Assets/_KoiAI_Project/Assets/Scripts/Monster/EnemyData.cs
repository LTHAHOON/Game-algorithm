
using NaughtyAttributes;
using UnityEngine;
using static KoiAI.Enemy.EnemyFeature;
using static KoiAI.Player.PlayerFeature;

namespace KoiAI.Enemy
{
    using KoiAI.AnimatorSystem;

    [CreateAssetMenu(fileName = "new EnemyData", menuName = "KoiAI/Enemy/EnemyData")]
    public class EnemyData : ScriptableObject
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
        private EnemyFeatureDataBase _enemyFeatureDataBase;
        [Space(10)]
        [SerializeField]
        private EnemyFeatureDataType _enemyFeatureDataType;


        [Space(10)]
        [ShowIf(nameof(HasMovementProperty))]
        [SerializeField]
        private EnemyMovementExtensionData _enemyMovementExtensionData;
        [ShowIf(nameof(HasRotationProperty))]
        [SerializeField]
        private EnemyRotationExtensionData _enemyRotationExtensionData;

        public EnemyFeatureData GetEnemyFeatureData()
        {
            EnemyFeatureData data = _enemyFeatureDataBase?.GetMonsterFeatureData(_enemyFeatureDataType);
            return data;
        }

        public EnemyFeatureExtensionData GetEnemyFeatureExtensionData(EnemyFeatureProperty featureProperty)
        {
            return featureProperty switch
            {
                EnemyFeatureProperty.Movement => _enemyMovementExtensionData,
                EnemyFeatureProperty.Rotation => _enemyRotationExtensionData,
                _ => null
            };
        }

        public EnemyFeatureValueData GetEnemyFeatureValueData(EnemyFeatureProperty featureProperty)
        {
            EnemyFeatureData data = GetEnemyFeatureData();
            if (data)
            {
                return featureProperty switch
                {
                    EnemyFeatureProperty.Movement => data.EnemyMovementValueData,
                    EnemyFeatureProperty.Rotation => data.EnemyRotationValueData,
                    _ => null
                };
            }
            return null;
        }

        public bool HasMovementProperty => GetEnemyFeatureData() is var data && data != null && data.HasMovementProperty;
        public bool HasRotationProperty => GetEnemyFeatureData() is var data && data != null && data.HasRotationProperty;

        public AnimatorData AnimatorData => _animatorData;
    }
}
