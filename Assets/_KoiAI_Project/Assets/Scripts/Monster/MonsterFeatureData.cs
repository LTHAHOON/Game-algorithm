using KoiAI.Camera;
using NaughtyAttributes;
using System;
using UnityEngine;
using static KoiAI.Monster.MonsterFeature;

namespace KoiAI.Monster
{
    [Serializable]
    public enum MonsterFeatureDataType
    {
        Small, //소형 캐릭터
        Medium, //중형 캐릭터
        Large, //대형 캐릭터
        Static, //움직이지 않는 캐릭터
    }

    [CreateAssetMenu(fileName = "new MonsterFeatureData", menuName = "KoiAI/Monster/MonsterFeatureData")]
    public class MonsterFeatureData : ScriptableObject
    {
        [SerializeField]
        private MonsterFeatureDataType _monsterFeatureDataType;
        [SerializeField]
        private MonsterFeatureProperty[] _properties;
        [ShowIf(nameof(HasMovementProperty))]
        [SerializeField]
        private MonsterMovementValueData _monsterMovementValueData;
        [ShowIf(nameof(HasRotationProperty))]
        [SerializeField]
        private MonsterRotationValueData _monsterRotationValueData;
        private bool HasProperty(MonsterFeatureProperty property)
        {
            bool bHas = Array.IndexOf(_properties, property) != -1;
            return bHas;
        }

        public MonsterMovementValueData MonsterMovementValueData => _monsterMovementValueData;
        public MonsterRotationValueData MonsterRotationValueData => _monsterRotationValueData;
        public bool HasMovementProperty => HasProperty(MonsterFeatureProperty.Movement);
        public bool HasRotationProperty => HasProperty(MonsterFeatureProperty.Rotation);
        public MonsterFeatureDataType MonsterFeatureDataType => _monsterFeatureDataType;
        public MonsterFeatureProperty[] Properties => _properties;

    }
}