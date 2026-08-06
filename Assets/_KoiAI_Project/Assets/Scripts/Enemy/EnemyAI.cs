using System;
using System.Collections.Generic;
using UnityEngine;
using static KoiAI.Enemy.EnemyFeature;
using static KoiAI.Enemy.EnemyFeatureTransition;

namespace KoiAI.Enemy
{
    using Cysharp.Threading.Tasks;
    using KoiAI.AnimatorSystem;
    using KoiAI.Nav;
    using KoiAI.Utilities;
    using NaughtyAttributes;

    public abstract class EnemyFeatureExtensionData { }
    public abstract class EnemyFeatureValueData { }

    public abstract class EnemyFeature
    {
        public enum EnemyFeatureProperty
        {
            None,
            Idle,
            Movement,
            Rotation,
            Attack,
        }

        public CompareEnumCondition<EnemyFeatureProperty> FeatureProperty => _enemyFeatureTransition.FeatureProperty;

        public EnemyAI Owner { get; set; }
        public EnemyTransitionCondition _enableTransitionCondition;
        public EnemyTransitionCondition _disableTransitionCondition;
        public EnemyFeatureTransition _enemyFeatureTransition;
        
        public void Init(EnemyFeatureTransition enemyFeatureTransition, EnemyFeatureValueData monsterFeatureValueData = null, EnemyFeatureExtensionData monsterFeatureExtensionData= null)
        {
            InitFeature(monsterFeatureValueData, monsterFeatureExtensionData);
            InitCondition(enemyFeatureTransition);
        }

        public virtual void InitFeature(EnemyFeatureValueData monsterFeatureValueData = null, EnemyFeatureExtensionData monsterFeatureExtensionData = null) { }

        private void InitCondition(EnemyFeatureTransition enemyFeatureTransition)
        {
            _enemyFeatureTransition = enemyFeatureTransition;

            _enableTransitionCondition = _enemyFeatureTransition.EnableTransitionType switch
            {
                EnemyFeatureTransitionType.None => null,
                EnemyFeatureTransitionType.EntitySight => new EnemySightCondition(_enemyFeatureTransition.EnableEntitySightConditionData),
                EnemyFeatureTransitionType.WithFeature => new EnemyWithFeatureCondition(_enemyFeatureTransition.EnableWithFeatureConditionData),
                _ => null,
            };
            _disableTransitionCondition = _enemyFeatureTransition.DisableTransitionType switch
            {
                EnemyFeatureTransitionType.None => null,
                EnemyFeatureTransitionType.EntitySight => new EnemySightCondition(_enemyFeatureTransition.DisableEntitySightConditionData),
                EnemyFeatureTransitionType.WithFeature => new EnemyWithFeatureCondition(_enemyFeatureTransition.DisableWithFeatureConditionData),
                _ => null,
            };
        }

        public bool CheckEnable()
        {
            if(_enemyFeatureTransition.EnableTransitionType == EnemyFeatureTransitionType.None)
            {
                return true;
            }
            bool isEnable = _enableTransitionCondition.Check(Owner);
            return isEnable;
        }

        public bool CheckDisable()
        {
            if (_enemyFeatureTransition.EnableTransitionType == EnemyFeatureTransitionType.None)
            {
                return false;
            }
            bool isDisable = _disableTransitionCondition.Check(Owner);
            return isDisable;
        }

        public bool TryGetTarget(out GameObject target)
        {
            target = _enableTransitionCondition.GetTarget();
            return target != null;
        }

        public abstract void EnterFeature();
        public abstract void UpdateFeature();
        public abstract void ExitFeature();
    }

    [Serializable]
    public class EnemyFeatureTransition
    {
        public enum EnemyFeatureTransitionType
        {
            None, //계속 지속
            EntitySight, //거리 계산
            WithFeature, //무조건 포함되어야하는 Feature
        }
        public enum EnemyFeatureState
        {
            ENTER,
            UPDATE,
            EXIT
        }
        [ReadOnly]
        [AllowNesting]
        public string DEBUG_STATE;
        [SerializeField]
        private CompareEnumCondition<EnemyFeatureProperty> _featureProperty;
        [SerializeField]
        private List<EnemyFeatureTransitionType> _enableTransitionTypes;
        [SerializeField]
        private List<EnemyFeatureTransitionType> _disableTransitionTypes;
        [ShowIf(nameof(_enableTransitionTypes), EnemyFeatureTransitionType.EntitySight)]
        [AllowNesting]
        [SerializeField]
        private EnemySightConditionData _enableEntitySightConditionData;
        [ShowIf(nameof(_disableTransitionTypes), EnemyFeatureTransitionType.EntitySight)]
        [AllowNesting]
        [SerializeField]
        private EnemySightConditionData _disableEntitySightConditionData;
        [ShowIf(nameof(_enableTransitionTypes), EnemyFeatureTransitionType.WithFeature)]
        [AllowNesting]
        [SerializeField]
        private EnemyWithFeatureConditionData _enableWithFeatureConditionData;
        [ShowIf(nameof(_disableTransitionTypes), EnemyFeatureTransitionType.WithFeature)]
        [AllowNesting]
        [SerializeField]
        private EnemyWithFeatureConditionData _disableWithFeatureConditionData;

        public CompareEnumCondition<EnemyFeatureProperty> FeatureProperty => _featureProperty;
        public EnemyFeatureTransitionType EnableTransitionType => _enableTransitionTypes;
        public EnemyFeatureTransitionType DisableTransitionType => _disableTransitionType;
        public EnemySightConditionData EnableEntitySightConditionData => _enableEntitySightConditionData;
        public EnemySightConditionData DisableEntitySightConditionData => _disableEntitySightConditionData;
        public EnemyWithFeatureConditionData EnableWithFeatureConditionData => _enableWithFeatureConditionData;
        public EnemyWithFeatureConditionData DisableWithFeatureConditionData => _disableWithFeatureConditionData;
    }

    [RequireComponent(typeof(Animator))]
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField]
        private NavigationController _agentController;
        [SerializeField]
        private EnemyData _enemyData;

        [SerializeField]
        private List<EnemyFeatureTransition> _enemyFeatureTransitions;
        private List<EnemyFeature> _enemyFeatures;
        private Dictionary<EnemyFeatureProperty, Func<EnemyFeature>> _dicEnemyFeatureCreator;
        private HashSet<CompareEnumCondition<EnemyFeatureProperty>> _enabledPropertiesHashSet;
        private Animator _monsterAnimator;
        private void Awake()
        {
            _enabledPropertiesHashSet = new();
            _enemyFeatures = new();
            _dicEnemyFeatureCreator = new()
            {
                { EnemyFeatureProperty.Idle, () => new EnemyIdle()},
                { EnemyFeatureProperty.Movement, () => new EnemyMovement()},
                { EnemyFeatureProperty.Rotation, () => new EnemyRotation()},
                { EnemyFeatureProperty.Attack, () => new EnemyAttack()}
            };
            _monsterAnimator = GetComponent<Animator>();
        }

        private void Start()
        {
            InitFeatures();
        }

        private void Update()
        {
            ActiveFeaturesProcess();
        }

        private void InitFeatures()
        {
            for (int i = 0; i < _enemyFeatureTransitions.Count; i++)
            {
                EnemyFeatureTransition transition = _enemyFeatureTransitions[i];
                CompareEnumCondition<EnemyFeatureProperty> property = transition.FeatureProperty;
                if (_enabledPropertiesHashSet.Contains(property))
                {
                    continue;
                }

                EnemyFeatureValueData valueData = _enemyData.GetEnemyFeatureValueData(property.CompareValue);
                EnemyFeatureExtensionData extensionData = _enemyData.GetEnemyFeatureExtensionData(property.CompareValue);

                if (_dicEnemyFeatureCreator.TryGetValue(property.CompareValue, out Func<EnemyFeature> featureCreator))
                {
                    EnemyFeature enemyFeature = featureCreator?.Invoke();
                    enemyFeature.Owner = this;
                    enemyFeature.Init(transition, valueData, extensionData);

                    _enabledPropertiesHashSet.Add(property);
                    _enemyFeatures.Add(enemyFeature);
                }
            }
            _enabledPropertiesHashSet.Clear();
            //Enable 조건 체크 후 Enter호출
            ActiveFeaturesProcess();
        }

        private void ActiveFeaturesProcess()
        {
            foreach (EnemyFeature feature in _enemyFeatures)
            {
                if (_enabledPropertiesHashSet.Contains(feature.FeatureProperty))
                {
                    feature.UpdateFeature();
                    feature._enemyFeatureTransition.DEBUG_STATE = $"{feature._enemyFeatureTransition.FeatureProperty.CompareValue.ToString()} {EnemyFeatureState.UPDATE.ToString()}";
                    bool canDisable = feature.CheckDisable();
                    if (canDisable)
                    {
                        feature._enemyFeatureTransition.DEBUG_STATE = $"{feature._enemyFeatureTransition.FeatureProperty.CompareValue.ToString()} {EnemyFeatureState.EXIT.ToString()}";
                        DisableFeature(feature);
                        return;
                    }
                }
                else
                {
                    bool canEnable = feature.CheckEnable();
                    if (canEnable)
                    {
                        feature._enemyFeatureTransition.DEBUG_STATE = $"{feature._enemyFeatureTransition.FeatureProperty.CompareValue.ToString()} {EnemyFeatureState.ENTER.ToString()}";
                        EnableFeature(feature);
                    }
                }
            }
        }

        public void EnableFeature(EnemyFeature feature)
        {
            if(!_enabledPropertiesHashSet.Contains(feature.FeatureProperty))
            {
                feature.EnterFeature();
                _enabledPropertiesHashSet.Add(feature.FeatureProperty);
            }
        }

        public void DisableFeature(EnemyFeature feature)
        {
            if(_enabledPropertiesHashSet.Contains(feature.FeatureProperty))
            {
                feature.ExitFeature();
                _enabledPropertiesHashSet.Remove(feature.FeatureProperty);
            }
        }

        public void OnDrawGizmos()
        {
            for (int i = 0; i < _enemyFeatureTransitions.Count; i++)
            {
                if(_enemyFeatureTransitions[i].EnableTransitionType == EnemyFeatureTransitionType.EntitySight)
                {
                    if (_enemyFeatureTransitions[i].EnableEntitySightConditionData.UseGizmo)
                    {
                        Gizmos.color = _enemyFeatureTransitions[i].EnableEntitySightConditionData.GizmosColor;
                        Gizmos.DrawWireSphere(transform.position, _enemyFeatureTransitions[i].EnableEntitySightConditionData.MaxDetectionDistance);
                        Gizmos.DrawWireSphere(transform.position, _enemyFeatureTransitions[i].EnableEntitySightConditionData.MinDetectionDistance);
                    }
                }
          
                if (_enemyFeatureTransitions[i].DisableTransitionType == EnemyFeatureTransitionType.EntitySight)
                {
                    if (_enemyFeatureTransitions[i].DisableEntitySightConditionData.UseGizmo)
                    {
                        Gizmos.color = _enemyFeatureTransitions[i].DisableEntitySightConditionData.GizmosColor;
                        Gizmos.DrawWireSphere(transform.position, _enemyFeatureTransitions[i].DisableEntitySightConditionData.MaxDetectionDistance);
                        Gizmos.DrawWireSphere(transform.position, _enemyFeatureTransitions[i].DisableEntitySightConditionData.MinDetectionDistance);
                    }
                }
                    
            }
        }

        public HashSet<CompareEnumCondition<EnemyFeatureProperty>> EnabledPropertiesHashSet => _enabledPropertiesHashSet;
        public NavigationController AgentController => _agentController;
        public AnimatorData EnemyAnimatorData => _enemyData.AnimatorData;
        public Animator EnemyAnimator => _monsterAnimator;
    }
}