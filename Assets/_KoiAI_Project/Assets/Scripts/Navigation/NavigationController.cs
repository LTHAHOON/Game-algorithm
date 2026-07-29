using R3;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace KoiAI.Nav
{
    public class NavigationController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private NavigationData _navigationData;

        private NavMeshPath _navMeshPath;
        private Rigidbody _rigidBody;
        private Vector3[] _path;
        private readonly Subject<float> _rigidMoveSubject = new();
        private IDisposable _rigidMoveSubscription;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _navMeshPath = new();
            _navMeshAgent.agentTypeID = _navigationData.GetAgentTypeID();
            _navMeshAgent.avoidancePriority = _navigationData.AvoidancePriority;
            _navMeshAgent.speed = _navigationData.MoveSpeed;
            _navMeshAgent.angularSpeed = _navigationData.AngularSpeed;
            _navMeshAgent.acceleration = _navigationData.Acceleration;

            switch (_navigationData.AgentPhyscisType)
            {
                case AgentPhysicsType.AgentPhysicsUpdate:
                    _navMeshAgent.updatePosition = true;
                    _navMeshAgent.updateRotation = true;
                    break;

                case AgentPhysicsType.RigidPhysicsUpdate:
                    SetUpRigidMoveSubscription();
                    break;
            }
        }

        public void SetUpRigidMoveSubscription()
        {
            _navMeshAgent.updatePosition = false;
            _navMeshAgent.updateRotation = true;
            TryGetComponent(out _rigidBody);
            if (_rigidBody)
            {
                _rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
            }
            _rigidMoveSubscription = _rigidMoveSubject.Subscribe(moveSpeed => Observable.Interval(TimeSpan.Zero, UnityTimeProvider.FixedUpdate)
                            .Subscribe(_ =>
                            {
                                if (_navMeshAgent.pathPending)
                                {
                                    _rigidBody.linearVelocity = new Vector3(0, _rigidBody.linearVelocity.y, 0);
                                    return;
                                }

                                if (_navMeshAgent.pathPending)
                                {
                                    Vector3 forwardVelocity = _rigidBody.linearVelocity;
                                    forwardVelocity.y = _rigidBody.linearVelocity.y;
                                    _rigidBody.linearVelocity = forwardVelocity;
                                }
                                else
                                {
                                    Vector3 desiredVelocity = _navMeshAgent.desiredVelocity;

                                    Vector3 targetVelocity = desiredVelocity * moveSpeed;
                                    targetVelocity.y = _rigidBody.linearVelocity.y;

                                    _rigidBody.linearVelocity = targetVelocity;
                                }
               
                                _navMeshAgent.nextPosition = _rigidBody.position;

                                if (IsMoveStop())
                                {
                                    _rigidBody.linearVelocity = new Vector3(0, _rigidBody.linearVelocity.y, 0);
                                }
                            }).AddTo(this));
        }

        public void MoveToDest(Vector3 destination, float moveSpeed)
        {
            if (!CanMoveToDestination(out _path, destination))
            {
                return;
            }

            _navMeshAgent.SetDestination(destination);

            switch (_navigationData.AgentPhyscisType)
            {
                case AgentPhysicsType.RigidPhysicsUpdate:
                    if (_rigidBody)
                    {
                        Debug.Log("Move");
                        _rigidMoveSubject.OnNext(moveSpeed);
                    }
                    break;
            }
        }

        public void ResetPath()
        {
            _navMeshAgent.ResetPath();
            _rigidMoveSubscription?.Dispose();
            _rigidMoveSubscription = null;
        }

        public bool IsMoveStop()
        {
            if (!_navMeshAgent.pathPending)
            {
                if (_navMeshAgent.desiredVelocity.sqrMagnitude <= 0.05f || _navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid
                        || _navMeshAgent.hasPath == false)
                {
                    return true;
                }
            }

            return false;
        }

        public bool CanMoveToDestination(out Vector3[] path, Vector3 destination)
        {
            path = default;
            if (_navMeshPath == null) return false;

            if (_navMeshAgent.CalculatePath(destination, _navMeshPath))
            {
                if (_navMeshPath.status == NavMeshPathStatus.PathInvalid)
                {
                    return false;
                }
                path = _navMeshPath.corners;
                return true;
            }
            return false;
        }
    }
}