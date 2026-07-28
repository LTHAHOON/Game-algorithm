using System;
using System.Collections.Generic;
using UnityEngine;
using static KoiAI.Monster.MonsterFeature;

namespace KoiAI.Monster
{
    using KoiAI.AnimatorSystem;
    using UnityEngine.AI;

    public abstract class MonsterFeatureExtensionData { }
    public abstract class MonsterFeatureValueData { }

    public abstract class MonsterFeature : MonoBehaviour
    {
        public enum MonsterFeatureProperty
        {
            None,
            Idle,
            Movement,
            Rotation,
            Attack,
        }

        public abstract MonsterFeatureProperty FeatureProperty { get; }
        public MonsterAI Owner { get; set; }
        public virtual void Init(MonsterFeatureValueData monsterFeatureValueData = null,
            MonsterFeatureExtensionData monsterFeatureExtensionData = null) { }
        public abstract void EnterFeature();
        public abstract void UpdateFeature();
        public abstract void ExitFeature();
    }

    [Serializable]
    public struct MonsterFeatureHandler
    {
        [Header("변경 전 Feature")]
        [SerializeField]
        private MonsterFeature _fromFeature;
        [Header("변경 후 Feature")]
        [SerializeField]
        private MonsterFeature _toFeature;
        public readonly MonsterFeature FromFeature => _fromFeature;
        public readonly MonsterFeature ToFeature => _toFeature;
    }

    [RequireComponent(typeof(Animator))]
    public class MonsterAI : MonoBehaviour
    {
        [SerializeField]
        private NavMeshAgent _monsterAgent;
        [SerializeField]
        private MonsterFeatureHandler[] _allFeaturesHandler;
        [SerializeField]
        private MonsterFeature _curMonsterFeature;
        [SerializeField]
        private MonsterData _monsterData;
        
        private Dictionary<ulong, MonsterFeatureHandler> _dicFeatureHanlder;
        private Animator _monsterAnimator;
        private void Awake()
        {
            _dicFeatureHanlder = new();
            _monsterAnimator = GetComponent<Animator>();
        }

        private void Start()
        {
            InitFeatures();
            if (_curMonsterFeature != null)
            {
                _curMonsterFeature.EnterFeature();
            }
        }

        private void Update()
        {
            if (_curMonsterFeature != null)
            {
                _curMonsterFeature.UpdateFeature();
            }
        }

        private void InitFeatures()
        {
            for (int i = 0; i < _allFeaturesHandler.Length; i++)
            {
                MonsterFeatureProperty featureProperty = _allFeaturesHandler[i].FromFeature.FeatureProperty;

                EntityId featureID = _allFeaturesHandler[i].FromFeature.GetEntityId();
                ulong instanceID = EntityId.ToULong(featureID);
                _dicFeatureHanlder.Add(instanceID, _allFeaturesHandler[i]);
                _allFeaturesHandler[i].FromFeature.Owner = this;
                MonsterFeatureValueData valueData = _monsterData.GetMonsterFeatureValueData(featureProperty);
                MonsterFeatureExtensionData extensionData = _monsterData.GetMonsterFeatureExtensionData(featureProperty);
                _allFeaturesHandler[i].FromFeature.Init(valueData, extensionData);

            }
        }
    
        /// <summary>
        /// 변경 후 Feature 구하기
        /// </summary>
        private bool TryGetToFeature(MonsterFeature fromFeature, out MonsterFeature toFeature)
        {
            EntityId featureID = fromFeature.GetEntityId();
            ulong instanceID = EntityId.ToULong(featureID);
            if(_dicFeatureHanlder.TryGetValue(instanceID, out var featureHandler))
            {
                toFeature = featureHandler.ToFeature;
                return true;
            }
            toFeature = null;
            return false;
        
        }

        /// <summary>
        /// 변경 후 Feature로 바꾸기
        /// </summary>
        public void ChangeFeature(MonsterFeature callerFeature)
        {
            if (callerFeature != null)
            {
                bool bGet = TryGetToFeature(callerFeature, out MonsterFeature toFeature);
                if(bGet)
                {
                    callerFeature.ExitFeature();
                    _curMonsterFeature = toFeature;
                    _curMonsterFeature.EnterFeature();
                }
            }
        }

        public bool IsMonsterAgentStop()
        {
            if(_monsterAgent.desiredVelocity.sqrMagnitude <= 0.05f || _monsterAgent.pathStatus == NavMeshPathStatus.PathInvalid || _monsterAgent.hasPath == false)
            {
                return true;
            }
            return false;
        }

        private NavMeshPath _navMeshPath = new();
        public bool CanMoveToDestination(Vector3 destination)
        {
            if(NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, _navMeshPath))
            {
                if(_navMeshPath.status == NavMeshPathStatus.PathInvalid)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public NavMeshAgent MonsterAgent => _monsterAgent;
        public AnimatorData MonsterAnimatorData => _monsterData.AnimatorData;
        public Animator MonsterAnimator => _monsterAnimator;
    }
}