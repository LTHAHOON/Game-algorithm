using NaughtyAttributes;
using System;
using UnityEngine;

namespace KoiAI.Enemy
{
    using Cysharp.Threading.Tasks;
    using KoiAI.A_Star;
    using KoiAI.AnimatorSystem;
    using KoiAI.Audio;
    using KoiAI.CustomPhysics;
    using System.Threading;

    [Serializable]
    public class EnemyMovementExtensionData : EnemyFeatureExtensionData
    {
        #region 보정값 및 추가 이동 데이터

        [SerializeField]
        private AudioData _stepAuidoData;
        [SerializeField]
        private AudioData _stopStepAudioData;
        [SerializeField]
        private AudioData _jumpAudioData;
        [SerializeField]
        private float _moveSpeedMod = 10f;
        [SerializeField]
        private float _jumpForceMod = 10f;
        [SerializeField]
        private int _jumpMaxCountMod = 1;
        [SerializeField]
        private float _stepAudioThresold;
        [SerializeField]
        private float _sizeForMoveStopMod;

        #endregion

        #region 물리 데이터
        [SerializeField]
        private RigidbodyData _rigidData;
        [SerializeField]
        private CapsuleColliderData _colliderData;
        [SerializeField]
        private float _moveDelayTime;
        #endregion

        public RigidbodyData RigidData => _rigidData;
        public CapsuleColliderData ColliderData => _colliderData;
        public AudioData StepAuidoData => _stepAuidoData;
        public AudioData StopStepAudioData => _stopStepAudioData;
        public AudioData JumpAudioData => _jumpAudioData;
        public float MoveDelayTime => _moveDelayTime;
        public float MoveSpeedMod => _moveSpeedMod;
        public float JumpForceMod => _jumpForceMod;
        public int JumpMaxCountMod => _jumpMaxCountMod;
        public float StepAudioThresold => _stepAudioThresold;
        public float SizeForMoveStopMod => _sizeForMoveStopMod;
    }

    [Serializable]
    public class EnemyMovementValueData : EnemyFeatureValueData
    {
        #region 이동 데이터

        [SerializeField]
        private float _moveSpeed = 10f;
        [SerializeField]
        private float _jumpForce = 10f;
        [SerializeField]
        private int _jumpMaxCount = 1;
        [SerializeField]
        private float _sizeForMoveStop = 3f;

        [SerializeField]
        private WayPointData _moveWapointData;

        #endregion

        public float MoveSpeed => _moveSpeed;
        public float JumpForce => _jumpForce;
        public int JumpMaxCount => _jumpMaxCount;
        public float SizeForMoveStop => _sizeForMoveStop;
        public WayPointData MoveWayPointData => _moveWapointData;
    }

    public class EnemyMovement : EnemyFeature
    {
        private EnemyMovementValueData _valueData;
        private EnemyMovementExtensionData _extensionData;
        
        private Vector3 _originPoint;
        private AnimatorParamData _animParamData;
        private GameObject _target;
        private bool _bHasTarget = false;

        public override void InitFeature(EnemyFeatureValueData monsterFeatureValueData = null,
            EnemyFeatureExtensionData monsterFeatureExtensionData = null)
        {
            if (monsterFeatureValueData is not EnemyMovementValueData valueData
                || monsterFeatureExtensionData is not EnemyMovementExtensionData extensionData)
            {
                return;
            }
            _valueData = valueData;
            _extensionData = extensionData;
            if (Owner.EnemyAnimatorData.IsValid())
            {
                //애니메이터 파라미터 데이터 초기화
                _animParamData = Owner.EnemyAnimatorData.AnimParamData;
            }
            else
            {
                Debug.Log("Check: EnemyAnimatorData is not valid.");
            }
            _originPoint = Owner.transform.position;
        }

        public override void EnterFeature()
        {
            _bHasTarget = TryGetTarget(out _target);
            StartMoveDelay().Forget();
        }

        public override void ExitFeature()
        {
            if(!_isReturning)
            {
                MoveToOrigin().Forget();
            }
            _bHasTarget = false;
        }



        public override void UpdateFeature()
        {
            if(_isStartMoveDelay)
            {
                return;
            }
            if (_bHasTarget)
            {
                float stopDistance = _valueData.SizeForMoveStop + _extensionData.SizeForMoveStopMod;
                Vector3 targetPos = _target.transform.position + Vector3.forward * stopDistance;

                Owner.AgentController.MoveToDest(targetPos, _valueData.MoveSpeed);

                Owner.EnemyAnimator.SetBool(_animParamData.WalkParmID, true);
            }

            if (Owner.AgentController.IsMoveStop())
            {
                Owner.EnemyAnimator.SetBool(_animParamData.WalkParmID, false);
                Owner.AgentController.ResetPath();
            }
        }

        private bool _isStartMoveDelay = false;
        private async UniTask StartMoveDelay()
        {
            _isStartMoveDelay = true;
            await UniTask.Delay(TimeSpan.FromSeconds(_extensionData.MoveDelayTime));
            _isStartMoveDelay = false;
        }

        private bool _isReturning = false;
        public async UniTask MoveToOrigin()
        {
            _isReturning = true;
            Owner.EnemyAnimator.SetBool(_animParamData.WalkParmID, true);

            Owner.AgentController.MoveToDest(_originPoint, _valueData.MoveSpeed);

            await UniTask.WaitForEndOfFrame();
            await UniTask.WaitUntil(() => Owner.AgentController.IsMoveStop(), cancellationToken: Owner.destroyCancellationToken);

            Owner.EnemyAnimator.SetBool(_animParamData.WalkParmID, false);
            Owner.AgentController.ResetPath();
            Owner.EnemyAnimator.SetBool(_animParamData.IdleParmID, true);
            _isReturning = false;
        }
    }
}
