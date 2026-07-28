using KoiAI.Player;
using KoiAI.Utilities;
using System;
using UnityEngine;

namespace KoiAI.Monster
{
    [Serializable]
    public class MonsterRotationExtensionData : MonsterFeatureExtensionData
    {
        #region 보정값 및 추가 회전 데이터

        [SerializeField]
        private float _lookSpeedMod;
        [SerializeField]
        private float _surfaceCheckDistanceMod;
        [SerializeField]
        private LayerMask _surfaceLayerMask;

        #endregion
        public float LookSpeedMod => _lookSpeedMod;
        public float SurfaceCheckDistanceMod => _surfaceCheckDistanceMod;
        public LayerMask SurfaceLayerMask => _surfaceLayerMask;
    }

    [Serializable]
    public class MonsterRotationValueData : MonsterFeatureValueData
    {
        #region 회전 데이터

        [SerializeField]
        private float _lookSpeed = 10f;
        [SerializeField]
        private float _surfaceCheckDistance = 3f;

        #endregion
        public float LookSpeed => _lookSpeed;
        public float SurfaceCheckDistance => _surfaceCheckDistance;
    }

    [RequireComponent(typeof(EntitySight))]
    public class MonsterRotation : MonsterFeature
    {
        [SerializeField]
        private float _lookSpeed = 10f;
        [SerializeField]
        private float _surfaceCheckDistance = 3f;
        [Header("Rotation 최소 거리")]
        [Tooltip("Feature 변경할 탐색 거리")]
        [SerializeField]
        private float _detectDistanceToFeature;

        private SurfaceAngleFinder _surfaceAngleFinder;
        private EntitySight _entitySight;
        private Vector3 _targetAngle = Vector3.zero;

        public override MonsterFeatureProperty FeatureProperty => MonsterFeatureProperty.Rotation;
        public override void Init(MonsterFeatureValueData monsterFeatureValueData = null,
            MonsterFeatureExtensionData monsterFeatureExtensionData = null)
        {
            _entitySight = GetComponent<EntitySight>();
            _surfaceAngleFinder = new(_surfaceCheckDistance);
        }

        public override void EnterFeature()
        {
            if (!_entitySight || _surfaceAngleFinder == null)
            {
                Owner.ChangeFeature(this);
            }
        }

        public override void UpdateFeature()
        {
            if (_entitySight == null || _surfaceAngleFinder == null)
            {
                return;
            }
            _entitySight.Detect();
            _surfaceAngleFinder.TryGetLocalSurfaceAngle(out _targetAngle, transform);
            bool isFindPlayer = _entitySight.IsFindTarget();
            if (isFindPlayer)
            {
                GameObject target = _entitySight.GetTargetToFind();
                Vector3 dir = target.transform.position - transform.position;
                float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
                _targetAngle.y = angle;
                if(dir.sqrMagnitude <= _detectDistanceToFeature * _detectDistanceToFeature)
                {
                    Owner.ChangeFeature(this);

                    return;
                }
            }
            else
            {
                _targetAngle.y = transform.eulerAngles.y;
            }
            Quaternion quat = Quaternion.Euler(_targetAngle.x, _targetAngle.y, _targetAngle.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, quat, Time.deltaTime * _lookSpeed);
        }

        public override void ExitFeature()
        {
        }


  
    
        private void Update()
        {
 
        }
    }
}
