using NaughtyAttributes;
using UnityEngine;
using static KoiAI.AI.AIFeature;

namespace KoiAI.AI
{
    using KoiAI.AnimatorSystem;
    
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
        private AIFeatureDataBase _aiFeatureDataBase;
        [Space(10)]
        [SerializeField]
        private AIFeatureDataType _aiFeatureDataType;
        
        [Space(10)]
        [ShowIf(nameof(HasMovementProperty))]
        [SerializeField]
        private AIMovementExtensionData _aiMovementExtensionData;
        [ShowIf(nameof(HasRotationProperty))]
        [SerializeField]
        private AIRotationExtensionData _aiRotationExtensionData;
        
        public AIFeatureData GetEnemyFeatureData()
        {
            AIFeatureData data = _aiFeatureDataBase?.GetAIFeatureData(_aiFeatureDataType);
            return data;
        }

        public AIFeatureExtensionData GetEnemyFeatureExtensionData(AIFeatureProperty featureProperty)
        {
            return featureProperty switch
            {
                AIFeatureProperty.Movement => _aiMovementExtensionData,
                AIFeatureProperty.Return => _aiMovementExtensionData,
                AIFeatureProperty.Rotation => _aiRotationExtensionData,
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
                    AIFeatureProperty.Movement => data.AIMovementValueData,
                    AIFeatureProperty.Return => data.AIMovementValueData,
                    AIFeatureProperty.Rotation => data.AIRotationValueData,
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
