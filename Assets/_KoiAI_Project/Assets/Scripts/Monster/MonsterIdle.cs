
using KoiAI.AnimatorSystem;
using KoiAI.Monster;
using R3;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

namespace KoiAI
{
    public class MonsterIdle : MonsterFeature
    {
        public override MonsterFeatureProperty FeatureProperty => MonsterFeatureProperty.Idle;

        private EntitySight _entitySight;
        private Vector3 _idlePoint;
        private AnimatorParamData _animParamData;
        private Subject<MonsterAI> _idleSubject = new();
        public override void Init(MonsterFeatureValueData monsterFeatureValueData = null, MonsterFeatureExtensionData monsterFeatureExtensionData = null)
        {
            _idleSubject.Subscribe((owner) =>
            {
                owner.MonsterAgent.SetDestination(_idlePoint);
                owner.MonsterAnimator.SetBool(_animParamData.WalkParmID, true);
            }).AddTo(this);

            _entitySight = GetComponent<EntitySight>();
            _idlePoint = transform.position;
            if (Owner.MonsterAnimatorData.IsValid())
            {
                _animParamData = Owner.MonsterAnimatorData.AnimParamData;
            }
        }

        public override void EnterFeature()
        {
        }

        public override void ExitFeature()
        {
        }

        public override void UpdateFeature()
        {
            if (_entitySight == null)
            {
                return;
            }
            _entitySight.Detect();
            if (_entitySight.IsFindTarget())
            {
                Owner.ChangeFeature(this);
            }
            else
            {
                _idleSubject.OnNext(Owner);

                if (Owner.IsMonsterAgentStop())
                {
                    owner.MonsterAnimator.SetBool(_animParamData.WalkParmID, true);
                }
            }
        }
    }
}
