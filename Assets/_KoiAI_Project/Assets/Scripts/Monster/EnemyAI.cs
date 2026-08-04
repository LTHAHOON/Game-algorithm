using System;
using System.Collections.Generic;
using UnityEngine;
using static KoiAI.Enemy.EnemyFeature;

namespace KoiAI.Enemy
{
    using Cysharp.Threading.Tasks;
    using KoiAI.AnimatorSystem;
    using KoiAI.Nav;
    using NaughtyAttributes;
    using System.Linq;
    using UnityEngine.AI;

    public abstract class EnemyFeatureExtensionData { }
    public abstract class EnemyFeatureValueData 
    {

    }

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

        public abstract EnemyFeatureProperty FeatureProperty { get; }
        public EnemyAI Owner { get; set; }
        public virtual void Init(EnemyFeatureValueData monsterFeatureValueData = null,
            EnemyFeatureExtensionData monsterFeatureExtensionData = null) { }
        public abstract void EnterFeature();
        public abstract void UpdateFeature();
        public abstract void ExitFeature();
    }


    [RequireComponent(typeof(Animator))]
    public class EnemyAI : MonoBehaviour
    {

        //State → 어떤 Feature를 활성화할지 결정
        //Feature → 실제 행동 수행
        [SerializeField]
        private NavigationController _agentController;
        [SerializeField]
        private EnemyData _enemyData;
        [SerializeField]
        private EnemyFeatureData _enemyFeatureData;

        private List<EnemyStateTransition> _enemyStateTransitions;
        private Dictionary<EnemyFeatureProperty, Func<EnemyFeature>> _dicEnemyFeatureCreator;
        private Dictionary<EnemyFeatureProperty, EnemyFeature> _dicEnemyFeatures;
        private HashSet<EnemyFeatureProperty> _existedPropertiesHashSet;
        private Animator _monsterAnimator;
        private void Awake()
        {
            _existedPropertiesHashSet = new();
            _dicEnemyFeatures = new();
            _dicEnemyFeatureCreator = new()
            {
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
            foreach(EnemyFeature feature in _dicEnemyFeatures.Values)
            {
                feature.UpdateFeature();
            }
        }

        private void InitFeatures()
        {
            for (int i = 0; i < _enemyStateTransitions.Count; i++)
            {
                List<EnemyFeatureProperty> allProperties = _enemyStateTransitions[i].GetAllProperties();
                for (int j = 0; j < allProperties.Count; j++)
                {
                    EnemyFeatureProperty property = allProperties[j];
                    if(_existedPropertiesHashSet.Contains(property))
                    {
                        continue;
                    }

                    EnemyFeatureValueData valueData = _enemyData.GetEnemyFeatureValueData(property);
                    EnemyFeatureExtensionData extensionData = _enemyData.GetEnemyFeatureExtensionData(property);

                    if (_dicEnemyFeatureCreator.TryGetValue(property, out Func<EnemyFeature> featureCreator))
                    {
                        EnemyFeature enemyFeature = featureCreator?.Invoke();
                        enemyFeature.Init(valueData, extensionData);
                        enemyFeature.Owner = this;

                        _existedPropertiesHashSet.Add(property);
                        _dicEnemyFeatures.Add(property, enemyFeature);
                    }
                }
            }
        }

        public async void Disab()
        {
            
        }


        public NavigationController AgentController => _agentController;
        public AnimatorData MonsterAnimatorData => _enemyData.AnimatorData;
        public Animator MonsterAnimator => _monsterAnimator;
    }
}