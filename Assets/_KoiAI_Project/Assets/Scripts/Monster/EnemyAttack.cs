using KoiAI.Utilities;
using UnityEngine;

namespace KoiAI.Enemy
{
    using KoiAI.AnimatorSystem;
    using KoiAI.Item;
    
    [RequireComponent (typeof(EntitySight))]
    public class EnemyAttack : EnemyFeature
    {
        [Header("몬스터 무기")]
        [SerializeField]
        private ActivateRandomValue<WeaponControllerBase>[] _randomWeaponContorllers;
        [Header("Attack 최대 거리")]
        [Tooltip("Feature 변경할 탐색 거리")]
        [SerializeField]
        private float _detectDistanceToFeature;
        [SerializeField]
        private float _attackDelayTime = 1f;

        private EntitySight _entitySight;
        private GameObject _target;
        private float _curAttackTime = 0f;
        private AnimatorParamData _animParamData;

        public override EnemyFeatureProperty FeatureProperty => EnemyFeatureProperty.Attack;

        public override void Init(EnemyFeatureValueData monsterFeatureValueData = null,
            EnemyFeatureExtensionData monsterFeatureExtensionData = null)
        {
            _entitySight = GetComponent<EntitySight>();
            for (int i = 0; i < _randomWeaponContorllers.Length; i++)
            {
                _randomWeaponContorllers[i].ActivateTarget.Init(null);
            }
            if (Owner.MonsterAnimatorData.IsValid())
            {
                _animParamData = Owner.MonsterAnimatorData.AnimParamData;
            }
        }

        public override void EnterFeature()
        {
            _target = _entitySight.GetTargetToFind();
            if(!_target)
            {
                Owner.ChangeState(this);
            }
        }

        public override void UpdateFeature()
        {
            Debug.Log("Attacking");
            _entitySight.Detect();
            WeaponControllerBase weaponController = ActivateRandom.GetRandomActivateTarget(_randomWeaponContorllers);
            if(weaponController == null)
            {
                Owner.ChangeState(this);
                return;
            }

            if (!_entitySight.IsFindTarget())
            {
                Owner.ChangeState(this);
                weaponController.EndAiming();
                return;
            }
            Vector3 dir = _target.transform.position - transform.position;
            if (dir.sqrMagnitude >= _detectDistanceToFeature * _detectDistanceToFeature)
            {
                Owner.ChangeState(this);
                weaponController.EndAiming();
                return;
            }
        
            if(_curAttackTime < _attackDelayTime )
            {
                _curAttackTime += Time.deltaTime;
                return;
            }
            _curAttackTime = 0f;
        
            weaponController.StartAiming();
            weaponController.Activate();
        }

        public override void ExitFeature()
        {

        }
    }
}
