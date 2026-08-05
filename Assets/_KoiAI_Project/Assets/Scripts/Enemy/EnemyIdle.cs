using UnityEngine;

namespace KoiAI.Enemy
{
    using KoiAI.AnimatorSystem;

    public class EnemyIdle : EnemyFeature
    {
        public override EnemyFeatureProperty FeatureProperty => EnemyFeatureProperty.Idle;

        private AnimatorParamData _animParamData;
        public override void InitFeature(EnemyFeatureValueData enemyFeatureValueData = null, EnemyFeatureExtensionData enemyFeatureExtensionData = null)
        {
            
            if (Owner.EnemyAnimatorData.IsValid())
            {
                _animParamData = Owner.EnemyAnimatorData.AnimParamData;
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
            if(Owner.AgentController.IsMoveStop())
            {
                Owner.EnemyAnimator.SetBool(_animParamData.IdleParmID, true);
            }
            else
            {
                Owner.EnemyAnimator.SetBool(_animParamData.IdleParmID, false);
            }
        }

      
    }
}
