using NaughtyAttributes;
using System;
using UnityEngine;
using static KoiAI.Enemy.EnemyFeature;

namespace KoiAI.Enemy
{
    [Serializable]
    public enum EnemyFeatureDataType
    {
        Small, //소형 캐릭터
        Medium, //중형 캐릭터
        Large, //대형 캐릭터
        Static, //움직이지 않는 캐릭터
    }

    [CreateAssetMenu(fileName = "new EnemyFeatureData", menuName = "KoiAI/Enemy/EnemyFeatureData")]
    public class EnemyFeatureData : ScriptableObject
    {
        [SerializeField]
        private EnemyFeatureDataType _enemyFeatureDataType;
        [SerializeField]
        private EnemyFeatureProperty[] _properties;
        [ShowIf(nameof(HasMovementProperty))]
        [SerializeField]
        private EnemyMovementValueData _enemyMovementValueData;
        [ShowIf(nameof(HasRotationProperty))]
        [SerializeField]
        private EnemyRotationValueData _enemyRotationValueData;
        private bool HasProperty(EnemyFeatureProperty property)
        {
            if (_properties == null)
            {
                return false;
            }
            bool bHas = Array.IndexOf(_properties, property) != -1;
            return bHas;
        }

        public EnemyMovementValueData EnemyMovementValueData => _enemyMovementValueData;
        public EnemyRotationValueData EnemyRotationValueData => _enemyRotationValueData;
        public bool HasMovementProperty => HasProperty(EnemyFeatureProperty.Movement);
        public bool HasRotationProperty => HasProperty(EnemyFeatureProperty.Rotation);
        public EnemyFeatureDataType EnemyFeatureDataType => _enemyFeatureDataType;
        public EnemyFeatureProperty[] Properties => _properties;

    }
}