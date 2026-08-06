using UnityEngine;
using System;

namespace KoiAI.Enemy
{
    using KoiAI.AnimatorSystem;
    using KoiAI.Item;
    using KoiAI.Utilities;

    [Serializable]
    public class EnemyAttackExtensionData : EnemyFeatureExtensionData
    {
        #region 공격 데이터

        [Header("몬스터 무기")]
        [SerializeField]
        private ActivateRandomValue<WeaponControllerBase>[] _randomWeaponContorllers;
        [SerializeField]
        private float _attackDelayTime = 1f;

        #endregion

        public ActivateRandomValue<WeaponControllerBase>[] RandomWeaponControllers;
        public float AttackDelayTime => _attackDelayTime;
    }


    [RequireComponent (typeof(EnemySightCondition))]
    public class EnemyAttack : EnemyFeature
    {
        private EnemyAttackExtensionData _extensionData;
        private WeaponControllerBase _weaponController;
        private float _curAttackTime = 0f;
        private AnimatorParamData _animParamData;


        public override void InitFeature(EnemyFeatureValueData enemyFeatureValueData = null,
            EnemyFeatureExtensionData enemyFeatureExtensionData = null)
        {
            if(enemyFeatureExtensionData is not EnemyAttackExtensionData extensionData)
            {
                return;
            }
            _extensionData = extensionData;

            for (int i = 0; i < _extensionData.RandomWeaponControllers.Length; i++)
            {
                _extensionData.RandomWeaponControllers[i].ActivateTarget.Init(null);
            }

            if (Owner.EnemyAnimatorData.IsValid())
            {
                _animParamData = Owner.EnemyAnimatorData.AnimParamData;
            }
        }

        public override void EnterFeature()
        {
            _weaponController.StartAiming();
        }

        public override void UpdateFeature()
        {
            Debug.Log("Attacking");
            _weaponController = ActivateRandom.GetRandomActivateTarget(_extensionData.RandomWeaponControllers);
            if(_weaponController == null)
            {
                return;
            }

            if(_curAttackTime < _extensionData.AttackDelayTime)
            {
                _curAttackTime += Time.deltaTime;
                return;
            }
            _curAttackTime = 0f;
     
            _weaponController.Activate();
        }

        public override void ExitFeature()
        {
            _weaponController.EndAiming();
        }
    }
}
