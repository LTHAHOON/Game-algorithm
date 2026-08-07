using Cysharp.Threading.Tasks;
using KoiAI.AnimatorSystem;
using KoiAI.Enemy;
using KoiAI.Nav;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static KoiAI.AI.AIFeature;
using static KoiAI.AI.AIFeatureTransition;

namespace KoiAI.AI
{
    public abstract class AIFeatureExtensionData { }
    public abstract class AIFeatureValueData { }

    public class AIBrain : MonoBehaviour
    {
        [SerializeField]
        private AISightConditionData _sightConditionData;
        private AISightCondition _sightCondition;

        private readonly AITargetContext _targetContext = new();

        [SerializeField]
        private bool _useDebugState = true;
        [SerializeField]
        private NavigationController _agentController;
        [SerializeField]
        private AIStatData _aiStatData;
        [SerializeField]
        private AIBrainData _aiBrainData;
        [ReadOnly]
        [SerializeField]
        private List<AIFeatureTransitionRuntimeDebug> _aiRuntimeDebug;
        [SerializeField]
        private AIFeatureTransitionRuntimeSetting[] _aiRuntimeSetting;

        private Action[] _aiDecisionLogics;
        private int _aiDecisionLogicIndex = -1;
        private List<AIFeature> _aiFeatures;
        private Dictionary<AIFeatureProperty, Func<AIFeature>> _dicAIFeatureCreator;
        private readonly HashSet<AIFeatureProperty> _activeFeatures = new();
        private Vector3 _originPosition;
        private Animator _aiAnimator;

        private void Awake()
        {
            _originPosition = transform.position;
            _sightCondition = new AISightCondition(_sightConditionData);

            _aiFeatures = new();
            _dicAIFeatureCreator = new()
            {
                { AIFeatureProperty.Idle, () => new AIIdle()},
                { AIFeatureProperty.Movement, () => new AIMovement()},
                { AIFeatureProperty.Rotation, () => new AIRotation()},
                { AIFeatureProperty.Attack, () => new AIAttack()},
                { AIFeatureProperty.Return, () => new AIReturn()}

            };
            _aiDecisionLogics = new Action[]
            {
                DecisionDisableLogic,
                DecisionEnableLogic,
            };
            _aiAnimator = GetComponent<Animator>();
        }

        private void Start()
        {
            InitFeatures();
            StartCoroutine(AIDecisionLogic());
        }

        private void Update()
        {
            _sightCondition.Tick(this);

            //SightCondition 결과를 Context에 복사
            UpdateTargetContext();

            UpdateActiveFeature();
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
            for (int i = 0; i < _aiBrainData.FeatureTransitions.Count; i++)
            {
                AIFeatureTransition transition = _aiBrainData.FeatureTransitions[i];
                AIFeatureProperty property = transition.FeatureProperty;
                if (_activeFeatures.Contains(property))
                {
                    continue;
                }

                AIFeatureValueData valueData = _aiStatData.GetEnemyFeatureValueData(property);
                AIFeatureExtensionData extensionData = _aiStatData.GetEnemyFeatureExtensionData(property);

                if (_dicAIFeatureCreator.TryGetValue(property, out Func<AIFeature> featureCreator))
                {
                    AIFeature aiFeature = featureCreator?.Invoke();
                    if (aiFeature == null)
                    {
                        continue;
                    }
                    aiFeature.Brain = this;
                    aiFeature.Init(transition, valueData, extensionData);

                    if(_useDebugState)
                    {
                        _aiRuntimeDebug.Add(new());
                        _aiRuntimeDebug[i].AssignDebug(aiFeature);
                    }
                    _activeFeatures.Add(property);
                    _aiFeatures.Add(aiFeature);
                }
            }
            _activeFeatures.Clear();
        }

        private IEnumerator AIDecisionLogic()
        {
            while (true)
            {
                NextAIDecisionLogic();
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void NextAIDecisionLogic()
        {
            ++_aiDecisionLogicIndex;
            _aiDecisionLogicIndex = _aiDecisionLogicIndex % _aiDecisionLogics.Length;
            _aiDecisionLogics[_aiDecisionLogicIndex]?.Invoke();
        }

        /// <summary>
        /// Feature 비활성화 판단 로직
        /// </summary>
        public void DecisionDisableLogic()
        {
            foreach (AIFeature feature in _aiFeatures)
            {
                if (_activeFeatures.Contains(feature.FeatureProperty))
                {
                    if (feature.CheckDisable())
                    {
                        feature.AIFeatureTransition.DEBUG_STATE = $"{feature.AIFeatureTransition.FeatureProperty.ToString()} {nameof(AIFeatureState.EXIT)}";
                        DisableFeature(feature).Forget();
                    }
                }
            }
        }

        /// <summary>
        /// Feature 활성화 판단 로직
        /// </summary>
        public void DecisionEnableLogic()
        {
            foreach (AIFeature feature in _aiFeatures)
            {
                if (!_activeFeatures.Contains(feature.FeatureProperty))
                {
                    if (feature.CheckEnable())
                    {
                        EnableFeature(feature).Forget();
                    }
                }
            }
        }

        /// <summary>
        /// 활성화된 Feature 업데이트 로직
        /// </summary>
        public void UpdateActiveFeature()
        {
            foreach (AIFeature feature in _aiFeatures)
            {
                if (_activeFeatures.Contains(feature.FeatureProperty))
                {
                    feature.UpdateFeature();
                }
            }
        }

        public async UniTask EnableFeature(AIFeature feature)
        {
            if (_activeFeatures.Contains(feature.FeatureProperty))
            {
                return;
            }

            feature.EnterFeature();

            float delayTime = feature.AIFeatureTransition.EnableDelayTime;
            await UniTask.Delay(TimeSpan.FromSeconds(delayTime));

            _activeFeatures.Add(feature.FeatureProperty);
        }

        public async UniTask DisableFeature(AIFeature feature)
        {
            if (!_activeFeatures.Contains(feature.FeatureProperty))
            {
                return;
            }

            feature.ExitFeature();

            float delayTime = feature.AIFeatureTransition.DisableDelayTime;
            await UniTask.Delay(TimeSpan.FromSeconds(delayTime));

            _activeFeatures.Remove(feature.FeatureProperty);
        }

        public bool IsFeatureActive(AIFeatureProperty property)
        {
            return _activeFeatures.Contains(property);
        }

        private void OnDrawGizmosSelected()
        {
            if (_sightConditionData == null || !_sightConditionData.UseGizmos)
            {
                return;
            }

            Transform eye = _sightConditionData.EyeTransform ? _sightConditionData.EyeTransform : transform;

            Gizmos.color = _sightConditionData.GizmosColor;

            Gizmos.DrawWireSphere(eye.position, _sightConditionData.DetectionDistance);

            Gizmos.DrawWireSphere(eye.position, _sightConditionData.LoseDistance);
        }

        public AITargetContext TargetContext => _targetContext;
        public NavigationController AgentController => _agentController;
        public AnimatorData EnemyAnimatorData => _aiStatData.AnimatorData;
        public Animator EnemyAnimator => _aiAnimator;
        public Vector3 OriginPosition => _originPosition;
    }
}
