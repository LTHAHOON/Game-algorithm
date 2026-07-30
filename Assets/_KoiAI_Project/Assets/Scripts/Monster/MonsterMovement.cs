using NaughtyAttributes;
using System;
using UnityEngine;

namespace KoiAI.Monster
{
    using KoiAI.A_Star;
    using KoiAI.AnimatorSystem;
    using KoiAI.Audio;
    using KoiAI.CustomPhysics;

    [Serializable]
    public class MonsterMovementExtensionData : MonsterFeatureExtensionData
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

        #endregion

        #region 물리 데이터
        [SerializeField]
        private RigidbodyData _rigidData;
        [SerializeField]
        private CapsuleColliderData _colliderData;
        #endregion

        public RigidbodyData RigidData => _rigidData;
        public CapsuleColliderData ColliderData => _colliderData;
        public AudioData StepAuidoData => _stepAuidoData;
        public AudioData StopStepAudioData => _stopStepAudioData;
        public AudioData JumpAudioData => _jumpAudioData;
        public float MoveSpeedMod => _moveSpeedMod;
        public float JumpForceMod => _jumpForceMod;
        public int JumpMaxCountMod => _jumpMaxCountMod;
        public float StepAudioThresold => _stepAudioThresold;
    }

    [Serializable]
    public class MonsterMovementValueData : MonsterFeatureValueData
    {
        #region 이동 데이터

        [SerializeField]
        private float _moveSpeed = 10f;
        [SerializeField]
        private float _jumpForce = 10f;
        [SerializeField]
        private int _jumpMaxCount = 1;

        [SerializeField]
        private WayPointData _moveWapointData;

        #endregion

        public float MoveSpeed => _moveSpeed;
        public float JumpForce => _jumpForce;
        public int JumpMaxCount => _jumpMaxCount;
        public WayPointData MoveWayPointData => _moveWapointData;
    }

    [RequireComponent(typeof(EntitySight))]
    public class MonsterMovement : MonsterFeature
    {
        [ReadOnly]
        [SerializeField]
        private MonsterMovementValueData _valueData;
        [ReadOnly]
        [SerializeField]
        private MonsterMovementExtensionData _extensionData;
        [Header("Movement 최대 거리")]
        [Tooltip("Feature 변경할 탐색 거리")]
        [SerializeField]
        private float _maxMovementDistance;
        [Header("Movement 최소 거리")]
        [Tooltip("Feature 변경할 탐색 거리")]
        [SerializeField]
        private float _minMovementDistance;

        private Vector3 _originPoint;
        private EntitySight _entitySight;
        private AnimatorParamData _animParamData;
        private GameObject _target;
        public override MonsterFeatureProperty FeatureProperty => MonsterFeatureProperty.Movement;

        public override void Init(MonsterFeatureValueData monsterFeatureValueData = null,
            MonsterFeatureExtensionData monsterFeatureExtensionData = null)
        {
            if (monsterFeatureValueData is not MonsterMovementValueData valueData
                || monsterFeatureExtensionData is not MonsterMovementExtensionData extensionData)
            {
                return;
            }
            _valueData = valueData;
            _extensionData = extensionData;
            _entitySight = GetComponent<EntitySight>();
            if (Owner.MonsterAnimatorData.IsValid())
            {
                //애니메이터 파라미터 데이터 초기화
                _animParamData = Owner.MonsterAnimatorData.AnimParamData;
            }
            else
            {
                Debug.Log("Check: MonsterAnimatorData is not valid.");
            }
            _originPoint = transform.position;
        }

        public override void EnterFeature()
        {
            Debug.Log("dsad");
            Owner.MonsterAnimator.SetTrigger(_animParamData.Act1ParamID);
        }

        public override void ExitFeature()
        {
            Owner.MonsterAnimator.SetBool(_animParamData.WalkParmID, false);
        }



        public override void UpdateFeature()
        {
            _entitySight.Detect();
            _target = _entitySight.GetTargetToFind();
            Vector3 dir = Vector3.zero;
            if (_target)
            {
                dir = _target.transform.position - transform.position;
            }

            if (!_entitySight.IsFindTarget() || dir.sqrMagnitude > _maxMovementDistance * _maxMovementDistance)
            {
                Owner.AgentController.MoveToDest(_originPoint, _valueData.MoveSpeed);
                Owner.MonsterAnimator.SetBool(_animParamData.WalkParmID, true);
                if (Owner.AgentController.IsMoveStop())
                {
                    Owner.AgentController.ResetPath();
                    Owner.ChangeFeature(this);
                }
            }
            else if(dir.sqrMagnitude < _minMovementDistance * _minMovementDistance) 
            {
             //   Owner.ChangeFeature(this);
            }
            else
            {
                Owner.AgentController.MoveToDest(_target.transform.position, _valueData.MoveSpeed);
                Owner.MonsterAnimator.SetBool(_animParamData.WalkParmID, true);
                if (Owner.AgentController.IsMoveStop())
                {
                    Owner.MonsterAnimator.SetBool(_animParamData.WalkParmID, false);
                    Owner.AgentController.ResetPath();
                    Owner.ChangeFeature(this);
                }
            }
        }
    }
}
