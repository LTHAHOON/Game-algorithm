using System;
using System.Collections.Generic;
using UnityEngine;
using static KoiAI.Monster.MonsterFeature;

namespace KoiAI.Monster
{
    using Cysharp.Threading.Tasks;
    using KoiAI.AnimatorSystem;
    using KoiAI.Nav;
    using NaughtyAttributes;
    using System.Linq;
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

        public int MainHandlerIndex { get; set; }
        public int NextHandlerIndex { get; set; }
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
        [Header("메인 인덱스")]
        [SerializeField]
        private int _mainHandlerIndex;

        [Header("변경 전 Feature")]
        [SerializeField]
        private MonsterFeature _fromFeature;
      
        [Header("변경 후 Feature")]
        [AllowNesting]
        [SerializeField]
        private int _nextHandlerIndex;
        [SerializeField]
        private MonsterFeature _toFeature;
        
        [Header("변경 딜레이 시간")]
        [SerializeField]
        private float _nextDelayTime;

        public readonly MonsterFeature FromFeature => _fromFeature;
        public readonly MonsterFeature ToFeature => _toFeature;
        public readonly int MainHandlerIndex => _mainHandlerIndex;
        public readonly int NextHandlerIndex => _nextHandlerIndex;
        public readonly float NextDelayTime => _nextDelayTime;
    }

    [RequireComponent(typeof(Animator))]
    public class MonsterAI : MonoBehaviour
    {
        [SerializeField]
        private NavigationController _agentController;
        [SerializeField]
        private MonsterFeatureHandler[] _allFeaturesHandler;
        [SerializeField]
        private MonsterFeature _curMonsterFeature;
        [SerializeField]
        private MonsterData _monsterData;
        
        private Dictionary<int, MonsterFeatureHandler> _dicFeatureHanlder;
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

                int mainHanlderIndex = _allFeaturesHandler[i].MainHandlerIndex;
                int nextHandlerIndex = _allFeaturesHandler[i].NextHandlerIndex;
                MonsterFeatureHandler nextHandler = default;
                int count = _allFeaturesHandler.Where(x => x.MainHandlerIndex == nextHandlerIndex).Select(x => 
                {
                    nextHandler = x;
                    return x.MainHandlerIndex; 
                }).Count();

                bool isExistIndex = count == 1;
                if(!isExistIndex)
                {
                    Debug.Log("존재하지 않는 NextHandlerIndex가 있습니다.");
                    continue;
                }

                _allFeaturesHandler[i].FromFeature.MainHandlerIndex = mainHanlderIndex;
                _allFeaturesHandler[i].FromFeature.NextHandlerIndex = nextHandlerIndex;
                _dicFeatureHanlder.Add(mainHanlderIndex, nextHandler);
                _allFeaturesHandler[i].FromFeature.Owner = this;
                MonsterFeatureValueData valueData = _monsterData.GetMonsterFeatureValueData(featureProperty);
                MonsterFeatureExtensionData extensionData = _monsterData.GetMonsterFeatureExtensionData(featureProperty);
                _allFeaturesHandler[i].FromFeature.Init(valueData, extensionData);
            }
        }
    
        private bool TryGetMonsterFeatureHandler(out MonsterFeatureHandler handler, int index)
        {
            if(_dicFeatureHanlder.TryGetValue(index, out handler))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 변경 후 Feature로 바꾸기
        /// </summary>
        public async void ChangeFeature(MonsterFeature callerFeature)
        {
            int nextHandlerIndex = callerFeature.NextHandlerIndex;

            if (callerFeature != null)
            {
                if (TryGetMonsterFeatureHandler(out MonsterFeatureHandler monsterFeatureHandler, callerFeature.MainHandlerIndex))
                {
                    await UniTask.WaitForSeconds(monsterFeatureHandler.NextDelayTime);

                    callerFeature.ExitFeature();
                    _curMonsterFeature = monsterFeatureHandler.FromFeature;
                    _curMonsterFeature.EnterFeature();
                }
            }
        }


        public NavigationController AgentController => _agentController;
        public AnimatorData MonsterAnimatorData => _monsterData.AnimatorData;
        public Animator MonsterAnimator => _monsterAnimator;
    }
}