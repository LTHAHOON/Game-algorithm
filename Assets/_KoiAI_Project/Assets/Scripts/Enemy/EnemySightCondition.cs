using System;
using UnityEngine;
using static KoiAI.Enemy.EnemyFeatureTransition;

namespace KoiAI.Enemy
{

    [Serializable]
    public class EnemySightConditionData
    {
        [Header("감지 위치")]
        [SerializeField]
        private Transform _eyeTransform;

        [Header("거리")]
        [Min(0f)]
        [SerializeField]
        private float _detectionDistance = 12f;

        [Tooltip("한번 발견한 타깃을 놓치는 거리")]
        [Min(0f)]
        [SerializeField]
        private float _loseDistance = 13f;

        [Header("시야각")]
        [Range(0f, 360f)]
        [SerializeField]
        private float _sightAngle = 120f;

        [Header("탐색")]
        [Min(0.01f)]
        [SerializeField]
        private float _scanInterval = 0.1f;

        [Min(1)]
        [SerializeField]
        private int _maxDetectionCount = 16;

        [SerializeField]
        private LayerMask _targetLayerMask;

        [SerializeField]
        private LayerMask _obstacleLayerMask;

        [Header("Gizmo")]
        [SerializeField]
        private bool _useGizmos = true;

        [SerializeField]
        private Color _gizmosColor = Color.yellow;

        public Transform EyeTransform => _eyeTransform;
        public float DetectionDistance => _detectionDistance;

        public float LoseDistance =>
            Mathf.Max(_loseDistance, _detectionDistance);

        public float SightAngle => _sightAngle;
        public float ScanInterval => _scanInterval;

        public int MaxDetectionCount =>
            Mathf.Max(1, _maxDetectionCount);

        public LayerMask TargetLayerMask => _targetLayerMask;
        public LayerMask ObstacleLayerMask => _obstacleLayerMask;

        public bool UseGizmos => _useGizmos;
        public Color GizmosColor => _gizmosColor;
    }

    public sealed class EnemySightCondition
    {
        private readonly EnemySightConditionData _data;
        private readonly Collider[] _overlapResults;

        private Collider _targetCollider;
        private GameObject _target;

        private float _distance = float.MaxValue;
        private float _scanTimer;

        public EnemySightCondition(
            EnemySightConditionData data)
        {
            _data = data;

            int bufferSize = data?.MaxDetectionCount ?? 1;

            _overlapResults = new Collider[bufferSize];
        }


        public GameObject Target => _target;

        /// <summary>
        /// EnemyAI에서 프레임당 한 번만 호출합니다.
        /// </summary>
        public void Tick(EnemyAI owner)
        {
            if (_data == null || !owner)
            {
                ClearTarget();
                return;
            }

            UpdateCurrentTargetDistance(owner);

            // LoseDistance를 벗어나면 즉시 타깃 해제
            if (_target &&
                _distance > _data.LoseDistance)
            {
                ClearTarget();
            }

            _scanTimer -= Time.deltaTime;

            if (_scanTimer > 0f)
                return;

            _scanTimer = _data.ScanInterval;

            // 기존 타깃이 여전히 유효한지 확인
            if (_targetCollider &&
                IsVisibleTarget(
                    owner,
                    _targetCollider,
                    _data.LoseDistance))
            {
                SetTarget(owner, _targetCollider);
                return;
            }

            ClearTarget();
            FindTarget(owner);
        }

        private void FindTarget(EnemyAI owner)
        {
            Vector3 eyePosition = GetEyePosition(owner);

            int count = Physics.OverlapSphereNonAlloc(eyePosition, _data.DetectionDistance, _overlapResults, _data.TargetLayerMask, QueryTriggerInteraction.Ignore);

            Collider nearestCollider = null;
            float nearestSqrDistance = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                Collider candidate = _overlapResults[i];

                if (!candidate)
                {
                    continue;
                }

                // 자기 자신의 Collider 제외
                if (candidate.transform.root == owner.transform.root)
                {
                    continue;
                }

                if (!IsVisibleTarget(owner, candidate, _data.DetectionDistance))
                {
                    continue;
                }

                Vector3 targetPosition = candidate.bounds.center;

                float sqrDistance = (targetPosition - eyePosition).sqrMagnitude;

                if (sqrDistance >= nearestSqrDistance)
                {
                    continue;
                }

                nearestSqrDistance = sqrDistance;
                nearestCollider = candidate;
            }

            if (nearestCollider)
            {
                SetTarget(owner, nearestCollider);
            }
        }

        private bool IsVisibleTarget(EnemyAI owner, Collider candidate, float maxDistance)
        {
            if (!candidate)
            {
                return false;
            }

            Transform eyeTransform = GetEyeTransform(owner);
            Vector3 eyePosition = eyeTransform.position;
            Vector3 targetPosition = candidate.bounds.center;

            Vector3 direction =
                targetPosition - eyePosition;

            float distance = direction.magnitude;

            if (distance <= Mathf.Epsilon || distance > maxDistance)
            {
                return false;
            }

            direction /= distance;

            // SightAngle을 전체 각도로 사용하므로 절반과 비교
            float halfSightAngle = _data.SightAngle * 0.5f;

            float dot = Mathf.Clamp(Vector3.Dot(eyeTransform.forward, direction), -1f, 1f);

            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

            if (angle > halfSightAngle)
            {
                return false;
            }

            // 타깃과 눈 사이에 장애물이 있으면 실패
            bool blocked = Physics.Raycast(eyePosition, direction, distance, _data.ObstacleLayerMask, QueryTriggerInteraction.Ignore);

            return !blocked;
        }

        private void SetTarget(EnemyAI owner, Collider targetCollider)
        {
            _targetCollider = targetCollider;
            _target = targetCollider.gameObject;

            Vector3 eyePosition = GetEyePosition(owner);

            _distance = Vector3.Distance(eyePosition, targetCollider.bounds.center);
        }

        private void UpdateCurrentTargetDistance(EnemyAI owner)
        {
            if (!_targetCollider)
            {
                ClearTarget();
                return;
            }

            _distance = Vector3.Distance(GetEyePosition(owner), _targetCollider.bounds.center);
        }

        private void ClearTarget()
        {
            _targetCollider = null;
            _target = null;
            _distance = float.MaxValue;
        }

        private Transform GetEyeTransform(EnemyAI owner)
        {
            return _data.EyeTransform ? _data.EyeTransform : owner.transform;
        }

        private Vector3 GetEyePosition(EnemyAI owner)
        {
            return GetEyeTransform(owner).position;
        }
  
    }
}
