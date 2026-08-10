using KoiAI.Enemy;
using NaughtyAttributes;
using System;
using UnityEngine;
using static KoiAI.AI.AIFeature;

namespace KoiAI.AI
{
    [Serializable]
    public enum AIFeatureDataType
    {
        Small, //소형 캐릭터
        Medium, //중형 캐릭터
        Large, //대형 캐릭터
        Static, //움직이지 않는 캐릭터
    }

    [CreateAssetMenu(fileName = "new EnemyFeatureData", menuName = "KoiAI/Enemy/EnemyFeatureData")]
    public class AIFeatureData : ScriptableObject
    {
        [SerializeField]
        private AIFeatureDataType _aiFeatureDataType;
        [SerializeField]
        private AIFeatureProperty[] _properties;
        [ShowIf(nameof(HasMovementProperty))]
        [SerializeField]
        private AIMovementValueData aiMovementValueData;
        [ShowIf(nameof(HasRotationProperty))]
        [SerializeField]
        private AIRotationValueData aiRotationValueData;
        private bool HasProperty(AIFeatureProperty property)
        {
            if (_properties == null)
            {
                return false;
            }
            bool bHas = Array.IndexOf(_properties, property) != -1;
            return bHas;
        }

        public AIMovementValueData AIMovementValueData => aiMovementValueData;
        public AIRotationValueData AIRotationValueData => aiRotationValueData;
        public bool HasMovementProperty => HasProperty(AIFeatureProperty.Movement);
        public bool HasRotationProperty => HasProperty(AIFeatureProperty.Rotation);
        public bool HasAttackProperty => HasProperty(AIFeatureProperty.Attack);
        public AIFeatureDataType AIFeatureDataType => _aiFeatureDataType;
        public AIFeatureProperty[] Properties => _properties;

    }
}