using KoiAI.AnimatorSystem;
using UnityEngine;

namespace KoiAI.Enemy
{
    public class EnemyReturn : EnemyFeature
    {
        private EnemyMovementValueData _valueData;
        private AnimatorParamData _animParamData;
        private EnemyMovementExtensionData _extensionData;
        public override void InitFeature(
            EnemyFeatureValueData enemyFeatureValueData = null,
            EnemyFeatureExtensionData enemyFeatureExtensionData = null)
        {
            if (enemyFeatureValueData is not EnemyMovementValueData valueData
                || enemyFeatureExtensionData is not EnemyMovementExtensionData extensionData)
            {
                return;
            }
            _valueData = valueData;
            _extensionData = extensionData;
            if (Owner.EnemyAnimatorData.IsValid())
            {
                //애니메이터 파라미터 데이터 초기화
                _animParamData = Owner.EnemyAnimatorData.AnimParamData;
            }
            else
            {
                Debug.Log("Check: EnemyAnimatorData is not valid.");
            }
        }

        public override void EnterFeature()
        {
            Owner.AgentController.ResetPath();
        }

        public override void UpdateFeature()
        {
            Owner.EnemyAnimator.SetBool(_animParamData.WalkParmID, true);
            Owner.AgentController.MoveToDest(Owner.OriginPosition, _valueData.MoveSpeed + _extensionData.MoveSpeedMod);
            if (Owner.AgentController.IsMoveStop())
            {
                Owner.EnemyAnimator.SetBool(_animParamData.WalkParmID, false);
                Owner.EnemyAnimator.SetBool(_animParamData.WalkParmID, false);
            }
        }

        public override void ExitFeature()
        {
            Owner.AgentController.ResetPath();
        }
    }
}
