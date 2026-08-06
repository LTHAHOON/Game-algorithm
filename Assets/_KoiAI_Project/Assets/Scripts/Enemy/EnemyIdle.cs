using UnityEngine;

namespace KoiAI.Enemy
{
    using KoiAI.AnimatorSystem;

    public class EnemyIdle : EnemyFeature
    {

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
            Owner.EnemyAnimator.SetBool(_animParamData.IdleParmID, false);
        }

        public override void UpdateFeature()
        {
  
        }

      
    }
}
