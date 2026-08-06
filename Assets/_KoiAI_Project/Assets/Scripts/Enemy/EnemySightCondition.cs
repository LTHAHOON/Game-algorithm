using System;
using UnityEngine;
using static KoiAI.Enemy.EnemyFeatureTransition;

namespace KoiAI.Enemy
{

    [Serializable]
    public class EnemySightConditionData
    {
        [SerializeField]
        private bool _bUseGizmos = false;
        [SerializeField]
        private Color _gizmosColor = Color.white;
        [SerializeField]
        private Transform _entityTransform;
        [SerializeField]
        private float _maxDetectionDistance;
        [SerializeField]
        private float _minDetectionDistance;
        [SerializeField]
        private int _detectMaxCount = 1;
        [SerializeField]
        private float _sightAngle = 60f;
        [SerializeField]
        private float _sightDelayTime = 0.5f;
        [SerializeField]
        private LayerMask _targetLayerMask;

        public bool UseGizmo => _bUseGizmos;
        public Color GizmosColor => _gizmosColor;   
        public Transform EntityTransform => _entityTransform;
        public float MaxDetectionDistance => _maxDetectionDistance;
        public float MinDetectionDistance => _minDetectionDistance;
        public int DetectMaxCount => _detectMaxCount;
        public float SightAngle => _sightAngle;
        public float SightDelayTime => _sightDelayTime;
        public LayerMask TargetLayerMask => _targetLayerMask;
    }

    public class EnemySightCondition : EnemyTransitionCondition
    {

        private EnemySightConditionData _enemySightData;
        private float _curSightTime = 0f;
        private bool _isFindPlayer = false;
        private GameObject _target;
        private Collider[] _targetColliders;

        public override EnemyFeatureTransitionType TransitionType => EnemyFeatureTransitionType.EntitySight;

        public EnemySightCondition(EnemySightConditionData enemySightData)
        {
            _enemySightData = enemySightData;
            _targetColliders = new Collider[_enemySightData.DetectMaxCount];
        }
        public override bool Check(EnemyAI owner)
        {
            Detect();
            return _isFindPlayer;
        }

        public override GameObject GetTarget()
        {
            return GetTargetToFind();
        }
        public void Detect()
        {
            if(!IsInit())
            {
                return;
            }
            if (_target == null)
            {
                int count = Physics.OverlapSphereNonAlloc(_enemySightData.EntityTransform.position, _enemySightData.MaxDetectionDistance, _targetColliders, _enemySightData.TargetLayerMask);
                if (count <= 0)
                {
                    return;
                }
                _target = _targetColliders[count - 1].gameObject;
            }

            float distance = (_target.transform.position - _enemySightData.EntityTransform.position).sqrMagnitude;
            if (_curSightTime < _enemySightData.SightDelayTime)
            {

                _curSightTime += Time.deltaTime;
                return;
            }
            if (distance > _enemySightData.MaxDetectionDistance * _enemySightData.MaxDetectionDistance
                || distance < _enemySightData.MinDetectionDistance * _enemySightData.MinDetectionDistance)
            {
                _isFindPlayer = false;
            }
            else
            {
                Vector3 dirToPlayer = _target.transform.position - _enemySightData.EntityTransform.position;
                dirToPlayer.Normalize();
                float dot = Vector3.Dot(_enemySightData.EntityTransform.forward, dirToPlayer);
                float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

                _isFindPlayer = angle < _enemySightData.SightAngle;
                _target = angle < _enemySightData.SightAngle ? _target : null;
            }

            _curSightTime = 0;
       
        }

        public GameObject GetTargetToFind()
        {
            return _isFindPlayer ? _target : null;
        }

        public bool IsFindTarget() => _isFindPlayer;

        private bool IsInit() => _enemySightData != null;

    
    }
}
