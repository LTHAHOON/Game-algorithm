using KoiAI.Player;
using KoiAI.Utilities;
using System;
using UnityEngine;

namespace KoiAI.Enemy
{
    [Serializable]
    public class EnemyRotationExtensionData : EnemyFeatureExtensionData
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
    public class EnemyRotationValueData : EnemyFeatureValueData
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

    [RequireComponent(typeof(EnemySightCondition))]
    public class EnemyRotation : EnemyFeature
    {
        private EnemyRotationValueData _valueData;
        private EnemyRotationExtensionData _extensionData;

        private SurfaceAngleFinder _surfaceAngleFinder;
        private Vector3 _targetAngle = Vector3.zero;
        private GameObject _target;
        private bool _bHasTarget = false;

        public override void InitFeature(EnemyFeatureValueData enemyFeatureValueData = null,
            EnemyFeatureExtensionData enemyFeatureExtensionData = null)
        {
            if (enemyFeatureValueData is not EnemyRotationValueData valueData
             || enemyFeatureExtensionData is not EnemyRotationExtensionData extensionData)
            {
                return;
            }
            _valueData = valueData;
            _extensionData = extensionData;
            _surfaceAngleFinder = new(_valueData.SurfaceCheckDistance + _extensionData.SurfaceCheckDistanceMod);
        }

        public override void EnterFeature()
        {
            _bHasTarget = TryGetTarget(out _target);
        }

        public override void UpdateFeature()
        {
            if(_surfaceAngleFinder.TryGetLocalSurfaceAngle(out _targetAngle, Owner.transform))
            {
                if (_bHasTarget)
                {
                    _targetAngle.y = Owner.transform.eulerAngles.y;
                }
                else
                {
                    Vector3 dir = _target.transform.position;
                    float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
                    _targetAngle.y = angle;
                }

            }
            Quaternion quat = Quaternion.Euler(_targetAngle.x, _targetAngle.y, _targetAngle.z);
            Owner.transform.rotation = Quaternion.Slerp(Owner.transform.rotation, quat, Time.deltaTime * (_valueData.LookSpeed + _extensionData.LookSpeedMod));
        }

        public override void ExitFeature()
        {
        }
    }
}
