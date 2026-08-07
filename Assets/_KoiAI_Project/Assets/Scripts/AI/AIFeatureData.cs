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
        private AIFeatureDataType _enemyFeatureDataType;
        [SerializeField]
        private AIFeatureProperty[] _properties;
        [ShowIf(nameof(HasMovementProperty))]
        [SerializeField]
        private AIMovementValueData _enemyMovementValueData;
        [ShowIf(nameof(HasRotationProperty))]
        [SerializeField]
        private AIRotationValueData _enemyRotationValueData;
        private bool HasProperty(AIFeatureProperty property)
        {
            if (_properties == null)
            {
                return false;
            }
            bool bHas = Array.IndexOf(_properties, property) != -1;
            return bHas;
        }

        public AIMovementValueData EnemyMovementValueData => _enemyMovementValueData;
        public AIRotationValueData EnemyRotationValueData => _enemyRotationValueData;
        public bool HasMovementProperty => HasProperty(AIFeatureProperty.Movement);
        public bool HasRotationProperty => HasProperty(AIFeatureProperty.Rotation);
        public AIFeatureDataType EnemyFeatureDataType => _enemyFeatureDataType;
        public AIFeatureProperty[] Properties => _properties;

    }
}