
using UnityEngine;

namespace KoiAI.Monster
{
    using KoiAI.AnimatorSystem;

    public class MonsterIdle : MonsterFeature
    {
        public override MonsterFeatureProperty FeatureProperty => MonsterFeatureProperty.Idle;

        private EntitySight _entitySight;
        private AnimatorParamData _animParamData;
        private float _curNextTime;
        public override void Init(MonsterFeatureValueData monsterFeatureValueData = null, MonsterFeatureExtensionData monsterFeatureExtensionData = null)
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
                Owner.ChangeFeature(this);
            }
        }
    }
}
