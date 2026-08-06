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
            Return
        }

        public EnemyFeatureProperty FeatureProperty => _enemyFeatureTransition.FeatureProperty;

        public EnemyAI Owner { get; set; }
        private EnemyConditionGroup _enableConditions;
        private EnemyConditionGroup _disableConditions;
        public EnemyFeatureTransition _enemyFeatureTransition;
        
        public void Init(EnemyFeatureTransition enemyFeatureTransition, EnemyFeatureValueData enemyFeatureValueData = null, EnemyFeatureExtensionData enemyFeatureExtensionData= null)
        {
            InitFeature(enemyFeatureValueData, enemyFeatureExtensionData);
            InitCondition(enemyFeatureTransition);
        }

        public virtual void InitFeature(EnemyFeatureValueData enemyFeatureValueData = null, EnemyFeatureExtensionData enemyFeatureExtensionData = null) { }

        private void InitCondition(
            EnemyFeatureTransition transition)
        {
            _enemyFeatureTransition = transition;

            _enableConditions =
                new EnemyConditionGroup(
                    transition.EnableConditions);

            _disableConditions =
                new EnemyConditionGroup(
                    transition.DisableConditions);
        }

        public bool CheckEnable()
        {
            bool isEnable = _enableConditions.Check(Owner,true);
            return isEnable;
        }

        public bool CheckDisable()
        {
            bool isDisable = _disableConditions.Check(Owner, false);
            return isDisable;
        }

        public bool TryGetTarget(out GameObject target)
        {
            Transform targetTransform =
                Owner.TargetContext.Target;

            target = targetTransform
                ? targetTransform.gameObject
                : null;

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
            None,
            HasTarget,
            Distance,
            WithFeature
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
        private EnemyFeatureProperty _featureProperty;

        [SerializeField]
        private EnemyConditionGroupData _enableConditions;

        [SerializeField]
        private EnemyConditionGroupData _disableConditions;

        public EnemyFeatureProperty FeatureProperty => _featureProperty;
        public EnemyConditionGroupData EnableConditions => _enableConditions;
        public EnemyConditionGroupData DisableConditions => _disableConditions;
    }

    [RequireComponent(typeof(Animator))]
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField]
        private EnemySightConditionData _sightConditionData;
        private EnemySightCondition _sightCondition;

        private readonly EnemyTargetContext _targetContext = new();
        
        [SerializeField]
        private NavigationController _agentController;
        [SerializeField]
        private EnemyData _enemyData;

        [SerializeField]
        private List<EnemyFeatureTransition> _enemyFeatureTransitions;
        
        
        private List<EnemyFeature> _enemyFeatures;
        private Dictionary<EnemyFeatureProperty, Func<EnemyFeature>> _dicEnemyFeatureCreator;
        private readonly HashSet<EnemyFeatureProperty> _activeFeatures = new();
        private Vector3 _originPosition;

        public bool IsFeatureActive(
            EnemyFeatureProperty property)
        {
            return _activeFeatures.Contains(property);
        }
        private Animator _monsterAnimator;
        private void Awake()
        {
            _originPosition = transform.position;
            _sightCondition =
                new EnemySightCondition(
                    _sightConditionData);
            
            _enemyFeatures = new();
            _dicEnemyFeatureCreator = new()
            {
                { EnemyFeatureProperty.Idle, () => new EnemyIdle()},
                { EnemyFeatureProperty.Movement, () => new EnemyMovement()},
                { EnemyFeatureProperty.Rotation, () => new EnemyRotation()},
                { EnemyFeatureProperty.Attack, () => new EnemyAttack()},
                { EnemyFeatureProperty.Return, () => new EnemyReturn()}
                
            };
            _monsterAnimator = GetComponent<Animator>();
        }

        private void Start()
        {
            InitFeatures();
        }

        private void Update()
        {
            // 1. 공용 센서 갱신
            _sightCondition.Tick(this);

            // 2. 센서 결과를 Context에 복사
            UpdateTargetContext();

            // 3. 모든 Feature가 동일한 Context를 사용하여 조건 평가
            ActiveFeaturesProcess();
        }

        private void UpdateTargetContext()
        {
            GameObject target = _sightCondition.Target;

            if (target)
            {
                _targetContext.SetTarget(transform, target.transform);
            }
            else
            {
                _targetContext.Clear();
            }
        }
        
        private void InitFeatures()
        {
            for (int i = 0; i < _enemyFeatureTransitions.Count; i++)
            {
                EnemyFeatureTransition transition = _enemyFeatureTransitions[i];
                EnemyFeatureProperty property = transition.FeatureProperty;
                if (_activeFeatures.Contains(property))
                {
                    continue;
                }

                EnemyFeatureValueData valueData = _enemyData.GetEnemyFeatureValueData(property);
                EnemyFeatureExtensionData extensionData = _enemyData.GetEnemyFeatureExtensionData(property);

                if (_dicEnemyFeatureCreator.TryGetValue(property, out Func<EnemyFeature> featureCreator))
                {
                    EnemyFeature enemyFeature = featureCreator?.Invoke();
                    if (enemyFeature == null)
                    {
                        continue;
                    }
                    enemyFeature.Owner = this;
                    enemyFeature.Init(transition, valueData, extensionData);

                    _activeFeatures.Add(property);
                    _enemyFeatures.Add(enemyFeature);
                }
            }
            _activeFeatures.Clear();
            ActiveFeaturesProcess();
        }

        private void ActiveFeaturesProcess()
        {
            // 1. 비활성화
            foreach (EnemyFeature feature in _enemyFeatures)
            {
                if (_activeFeatures.Contains(feature.FeatureProperty))
                {
                    if (feature.CheckDisable())
                    {
                        feature._enemyFeatureTransition.DEBUG_STATE = $"{feature._enemyFeatureTransition.FeatureProperty.ToString()} {nameof(EnemyFeatureState.EXIT)}";
                        DisableFeature(feature);
                    }    
                }
            }

            // 2. 활성화
            foreach (EnemyFeature feature in _enemyFeatures)
            {
                if (!_activeFeatures.Contains(feature.FeatureProperty))
                {
                    if (feature.CheckEnable())
                    {
                        feature._enemyFeatureTransition.DEBUG_STATE = $"{feature._enemyFeatureTransition.FeatureProperty.ToString()} {nameof(EnemyFeatureState.ENTER)}";
                        EnableFeature(feature);
                    }
                }
            }

            // 3. 활성 Feature 업데이트
            foreach (EnemyFeature feature in _enemyFeatures)
            {
                if (_activeFeatures.Contains(feature.FeatureProperty))
                {
                    feature._enemyFeatureTransition.DEBUG_STATE = $"{feature._enemyFeatureTransition.FeatureProperty.ToString()} {nameof(EnemyFeatureState.UPDATE)}";
                    feature.UpdateFeature();
                }
            }
            
        }

        public void EnableFeature(EnemyFeature feature)
        {
            if (!_activeFeatures.Add(feature.FeatureProperty))
                return;

            feature.EnterFeature();
        }

        public void DisableFeature(EnemyFeature feature)
        {
            if (!_activeFeatures.Remove(feature.FeatureProperty))
                return;

            feature.ExitFeature();
        }

        private void OnDrawGizmosSelected()
        {
            if (_sightConditionData == null ||
                !_sightConditionData.UseGizmos)
            {
                return;
            }

            Transform eye =
                _sightConditionData.EyeTransform
                    ? _sightConditionData.EyeTransform
                    : transform;

            Gizmos.color =
                _sightConditionData.GizmosColor;

            Gizmos.DrawWireSphere(
                eye.position,
                _sightConditionData.DetectionDistance);

            Gizmos.DrawWireSphere(
                eye.position,
                _sightConditionData.LoseDistance);
        }

        public EnemyTargetContext TargetContext => _targetContext;
        public NavigationController AgentController => _agentController;
        public AnimatorData EnemyAnimatorData => _enemyData.AnimatorData;
        public Animator EnemyAnimator => _monsterAnimator;
        public Vector3 OriginPosition => _originPosition;
    }
}