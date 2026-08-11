using UnityEngine;
using System;

namespace KoiAI.AI
{
    using KoiAI.AnimatorSystem;
    using KoiAI.Item;
    using KoiAI.Utilities;
    using NaughtyAttributes;
    using System.Collections.Generic;

    [Serializable]
    public class AIAttackExtensionData : AIFeatureExtensionData
    {

        #region 공격 데이터
        [SerializeField]
        private ulong _weaponID;
        [SerializeField]
        private ActivateRandomValue<WeaponData>[] _randomWeaponData;
        [SerializeField]
        private float _attackDelayTime = 1f;

        #endregion

        public ActivateRandomValue<WeaponData>[] RandomWeaponData => _randomWeaponData;
        public float AttackDelayTime => _attackDelayTime;
    }

    public class AIAttack : AIFeature
    {
        private List<ActivateRandomValue<WeaponControllerBase>> _randomWeaponData;
        private AIAttackExtensionData _extensionData;
        private WeaponControllerBase _weaponController;
        private float _curAttackTime = 0f;
        private AnimatorParamData _animParamData;


        public override void InitFeature(AIFeatureValueData enemyFeatureValueData = null,
            AIFeatureExtensionData enemyFeatureExtensionData = null)
        {
            if(enemyFeatureExtensionData is not AIAttackExtensionData extensionData)
            {
                return;
            }
            _extensionData = extensionData;

            for (int i = 0; i < _extensionData.RandomWeaponData.Length; i++)
            {
                WeaponData weaponData = _extensionData.RandomWeaponData[i].ActivateTargetData;
                WeaponControllerBase weaponController = WeaponControlRegistry.GetWeaponController(weaponData);
                weaponController.Init(null);
            }

            if (Brain.EnemyAnimatorData.IsValid())
            {
                _animParamData = Brain.EnemyAnimatorData.AnimParamData;
            }
        }

        public override void EnterFeature()
        {
            _weaponController = ActivateRandom.GetRandomActivateTarget(_extensionData.RandomWeaponData);
            _weaponController.StartAiming();
        }

        public override void UpdateFeature()
        {
            Debug.Log("Attacking");
            if(_weaponController == null)
            {
                _weaponController = ActivateRandom.GetRandomActivateTarget(_extensionData.RandomWeaponData);
            }

            if(_curAttackTime < _extensionData.AttackDelayTime)
            {
                _curAttackTime += Time.deltaTime;
                return;
            }
            _curAttackTime = 0f;
     
            _weaponController.Activate();
            _weaponController = null;
        }

        public override void ExitFeature()
        {
            _weaponController.EndAiming();
        }
    }
}
