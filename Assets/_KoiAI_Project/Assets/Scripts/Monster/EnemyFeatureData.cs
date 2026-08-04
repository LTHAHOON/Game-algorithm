using KoiAI.Camera;
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

    [CreateAssetMenu(fileName = "new MonsterFeatureData", menuName = "KoiAI/Monster/MonsterFeatureData")]
    public class EnemyFeatureData : ScriptableObject
    {
        [SerializeField]
        private EnemyFeatureDataType _monsterFeatureDataType;
        [SerializeField]
        private EnemyFeatureProperty[] _properties;
        [ShowIf(nameof(HasMovementProperty))]
        [SerializeField]
        private EnemyMovementValueData _monsterMovementValueData;
        [ShowIf(nameof(HasRotationProperty))]
        [SerializeField]
        private EnemyRotationValueData _monsterRotationValueData;
        private bool HasProperty(EnemyFeatureProperty property)
        {
            bool bHas = Array.IndexOf(_properties, property) != -1;
            return bHas;
        }

        public EnemyMovementValueData EnemyMovementValueData => _monsterMovementValueData;
        public EnemyRotationValueData EnemyRotationValueData => _monsterRotationValueData;
        public bool HasMovementProperty => HasProperty(EnemyFeatureProperty.Movement);
        public bool HasRotationProperty => HasProperty(EnemyFeatureProperty.Rotation);
        public EnemyFeatureDataType MonsterFeatureDataType => _monsterFeatureDataType;
        public EnemyFeatureProperty[] Properties => _properties;

    }
}