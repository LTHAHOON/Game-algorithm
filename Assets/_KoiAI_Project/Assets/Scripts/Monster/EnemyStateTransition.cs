using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using static KoiAI.Enemy.EnemyFeature;

namespace KoiAI.Enemy
{
    public enum EnemyState
    {
        Idle,
        Move,
        Attack,
    }

    [CreateAssetMenu(fileName = "new EnemyStateTransition", menuName = "KoiAI/Enemy/EnemyStateTransition")]
    public class EnemyStateTransition : ScriptableObject
    {
        [SerializeField]
        private EnemyState _state;
        [Header("Enable 딜레이 시간")]
        [SerializeField]
        private float _enabledDelayTime;
        [SerializeField]
        private List<EnemyFeatureProperty> _enabledProperties;
        [SerializeField]
        private bool _allDisableWithOutThis = false;
        [ShowIf("_allDisable")]
        [SerializeField]
        private List<EnemyFeatureProperty> _disabledProperties;


        private List<EnemyFeatureProperty> _allProperties = new();
        public EnemyState State => _state;
        public List<EnemyFeatureProperty> GetAllProperties()
        {
            if (_allProperties.Count <= 0)
            {
                for (int i = 0; i < _enabledProperties.Count; i++)
                {
                    if (!_allProperties.Contains(_enabledProperties[i]))
                    {
                        _allProperties.Add(_enabledProperties[i]);
                    }
                }
                for (int i = 0; i < _disabledProperties.Count; i++)
                {
                    if (!_allProperties.Contains(_disabledProperties[i]))
                    {
                        _allProperties.Add(_disabledProperties[i]);
                    }
                }
            }
            return _allProperties;
        }

        public List<EnemyFeatureProperty> EnabledProperties => _enabledProperties;
        public List<EnemyFeatureProperty> GetDisabledProperties()
        {
            if(_allDisableWithOutThis)
            {
                return 
            }
        }

    }
}
