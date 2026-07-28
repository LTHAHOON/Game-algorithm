using DG.Tweening;
using KoiAI.A_Star;
using KoiAI.AnimatorSystem;
using KoiAI.Audio;
using KoiAI.CustomPhysics;
using KoiAI.Player;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.GridLayoutGroup;

namespace KoiAI.Monster
{
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
        private float _buildDelayTime = 1f;
        [SerializeField]
        private WayPointData _moveWapointData;

        #endregion

        public float BuildDelayTime => _buildDelayTime;
        public WayPointData MoveWayPointData => _moveWapointData;
        public float MoveSpeed => _moveSpeed;
        public float JumpForce => _jumpForce;
        public int JumpMaxCount => _jumpMaxCount;
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
        private float _detectDistanceToFeature;

        private GameObject _target;
        private EntitySight _entitySight;
        private AnimatorData _animatorData;
        private AnimatorParamData _animParamData;

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
                //애니메이터 데이터 초기화
                _animatorData = Owner.MonsterAnimatorData;
                //애니메이터 파라미터 데이터 초기화
                _animParamData = Owner.MonsterAnimatorData.AnimParamData;
            }
            else
            {
                Debug.Log("Check: MonsterAnimatorData is not valid.");
            }

            Owner.MonsterAgent.speed = _valueData.MoveSpeed;
        }

        public override void EnterFeature()
        {
        }

        public override void ExitFeature()
        {
            Owner.MonsterAnimator.SetBool(_animParamData.WalkParmID, false);
        }

        public bool DetectTargetAroundMonster()
        {
            _entitySight.Detect();
            if (_entitySight.IsFindTarget())
            {
                _target = _entitySight.GetTargetToFind();
                Vector3 dir = _target.transform.position - transform.position;
                if(dir.sqrMagnitude > _detectDistanceToFeature * _detectDistanceToFeature)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public override void UpdateFeature()
        {
            if(!DetectTargetAroundMonster())
            {
                Owner.MonsterAgent.ResetPath();
                Owner.ChangeFeature(this);
                return;
            }
            else
            {
                Owner.MonsterAgent.SetDestination(_target.transform.position);
                Owner.MonsterAnimator.SetBool(_animParamData.WalkParmID, true);
            }
        }
    }
}
