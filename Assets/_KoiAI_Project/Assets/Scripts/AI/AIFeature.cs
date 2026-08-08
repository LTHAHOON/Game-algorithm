using UnityEngine;

namespace KoiAI.AI
{
    /// <summary>
    /// AI 행동 클래스
    /// </summary>
    public abstract class AIFeature
    {
        public enum AIFeatureProperty
        {
            None,
            Idle,
            Movement,
            Rotation,
            Attack,
            Return
        }

        public AIFeatureProperty FeatureProperty => _aiTransition.FeatureProperty;

        public AIBrain Brain { get; set; }
        private AIConditionGroup _enableConditions;
        private AIConditionGroup _disableConditions;
        private AIFeatureTransition _aiTransition;

        public void Init(AIFeatureTransition aiTransition, AIFeatureValueData enemyFeatureValueData = null, AIFeatureExtensionData enemyFeatureExtensionData = null)
        {
            InitFeature(enemyFeatureValueData, enemyFeatureExtensionData);
            InitCondition(aiTransition);
        }

        public virtual void InitFeature(AIFeatureValueData enemyFeatureValueData = null, AIFeatureExtensionData enemyFeatureExtensionData = null) { }

        private void InitCondition(AIFeatureTransition aiTransition)
        {
            _aiTransition = aiTransition;

            _enableConditions = new AIConditionGroup(aiTransition.EnableConditions);

            _disableConditions = new AIConditionGroup(aiTransition.DisableConditions);
        }

        public bool CheckEnable()
        {
            bool isEnable = _enableConditions.Check(Brain, true);
            return isEnable;
        }

        public bool CheckDisable()
        {
            bool isDisable = _disableConditions.Check(Brain, false);
            return isDisable;
        }

        public bool TryGetTarget(out GameObject target)
        {
            Transform targetTransform =
                Brain.TargetContext.Target;

            target = targetTransform
                ? targetTransform.gameObject
                : null;

            return target != null;
        }

        public abstract void EnterFeature();
        public abstract void UpdateFeature();
        public abstract void ExitFeature();

        public AIFeatureTransition AIFeatureTransition => _aiTransition;
    }
}
