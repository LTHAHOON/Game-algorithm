using NaughtyAttributes;
using UnityEngine;
using static KoiAI.AI.AIFeature;

namespace KoiAI.AI
{
    using KoiAI.AnimatorSystem;

    [CreateAssetMenu(fileName = "new AIStatData", menuName = "KoiAI/AI/AIStatData")]
    public class AIStatData : ScriptableObject
    {
        [SerializeField]
        private string _aiBaseName;

        [Space(10)]
        [HorizontalLine(5, EColor.Gray)]
        [Space(10)]
        [SerializeField]
        private AnimatorData _animatorData;
        [SerializeField]
        private AIFeatureDataBase _enemyFeatureDataBase;
        [Space(10)]
        [SerializeField]
        private AIFeatureDataType _enemyFeatureDataType;


        [Space(10)]
        [ShowIf(nameof(HasMovementProperty))]
        [SerializeField]
        private AIMovementExtensionData _enemyMovementExtensionData;
        [ShowIf(nameof(HasRotationProperty))]
        [SerializeField]
        private AIRotationExtensionData _enemyRotationExtensionData;

        public AIFeatureData GetEnemyFeatureData()
        {
            AIFeatureData data = _enemyFeatureDataBase?.GetEnemyFeatureData(_enemyFeatureDataType);
            return data;
        }

        public AIFeatureExtensionData GetEnemyFeatureExtensionData(AIFeatureProperty featureProperty)
        {
            return featureProperty switch
            {
                AIFeatureProperty.Movement => _enemyMovementExtensionData,
                AIFeatureProperty.Return => _enemyMovementExtensionData,
                AIFeatureProperty.Rotation => _enemyRotationExtensionData,
                _ => null
            };
        }

        public AIFeatureValueData GetEnemyFeatureValueData(AIFeatureProperty featureProperty)
        {
            AIFeatureData data = GetEnemyFeatureData();
            if (data)
            {
                return featureProperty switch
                {
                    AIFeatureProperty.Movement => data.EnemyMovementValueData,
                    AIFeatureProperty.Return => data.EnemyMovementValueData,
                    AIFeatureProperty.Rotation => data.EnemyRotationValueData,
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
