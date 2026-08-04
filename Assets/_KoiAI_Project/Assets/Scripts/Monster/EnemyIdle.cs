
using UnityEngine;

namespace KoiAI.Enemy
{
    using KoiAI.AnimatorSystem;

    public class EnemyIdle : EnemyFeature
    {
        public override EnemyFeatureProperty FeatureProperty => EnemyFeatureProperty.Idle;

        private EntitySight _entitySight;
        private AnimatorParamData _animParamData;
        private float _curNextTime;
        public override void Init(EnemyFeatureValueData monsterFeatureValueData = null, EnemyFeatureExtensionData monsterFeatureExtensionData = null)
        {

            _entitySight = GetComponent<EntitySight>();
            if (Owner.MonsterAnimatorData.IsValid())
            {
                _animParamData = Owner.MonsterAnimatorData.AnimParamData;
            }
        }

        public override void EnterFeature()
        {
            Owner.MonsterAnimator.SetBool(_animParamData.IdleParmID, true);
        }

        public override void ExitFeature()
        {
            Owner.MonsterAnimator.SetBool(_animParamData.IdleParmID, false);
        }

        public override void UpdateFeature()
        {
            _entitySight.Detect();
            if (_entitySight.IsFindTarget())
            {
                Owner.ChangeState(this);
            }
        }
    }
}
