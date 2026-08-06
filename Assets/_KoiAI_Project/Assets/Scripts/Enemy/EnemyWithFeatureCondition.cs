using KoiAI.Utilities;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using static KoiAI.Enemy.EnemyFeature;
using static KoiAI.Enemy.EnemyFeatureTransition;

namespace KoiAI.Enemy
{
    [Serializable]
    public class EnemyWithFeatureConditionData
    {
        [SerializeField]
        private List<CompareEnumCondition<EnemyFeatureProperty>> _enemyFeaturePropertyConditions;

        public List<CompareEnumCondition<EnemyFeatureProperty>> EnemyFeaturePropertyCondition => _enemyFeaturePropertyConditions;
    }

    public class EnemyWithFeatureCondition : EnemyTransitionCondition
    {
        private EnemyWithFeatureConditionData _enemyWithOutFeatureData;
        private CompareEnumCondition<EnemyFeatureProperty> emptyCondition;
        public override EnemyFeatureTransitionType TransitionType => EnemyFeatureTransitionType.WithFeature;

        public EnemyWithFeatureCondition(EnemyWithFeatureConditionData enemyWithOutFeatureData)
        {
            _enemyWithOutFeatureData = enemyWithOutFeatureData;
            emptyCondition = new(EnemyFeatureProperty.None, ComparisonType.None, true);
        }

        public override bool Check(EnemyAI owner)
        {
            bool isOn = false;
            if(owner.EnabledPropertiesHashSet.Count <= 0)
            {
                for (int i = 0; i < _enemyWithOutFeatureData.EnemyFeaturePropertyCondition.Count; i++)
                {
                    CompareEnumCondition<EnemyFeatureProperty> conditionData = _enemyWithOutFeatureData.EnemyFeaturePropertyCondition[i];
                    isOn = emptyCondition.CompareValue.CompareEnumWithCondition<EnemyFeatureProperty, int>(conditionData);
                    if(isOn)
                    {
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < _enemyWithOutFeatureData.EnemyFeaturePropertyCondition.Count; i++)
                {
                    CompareEnumCondition<EnemyFeatureProperty> conditionData = _enemyWithOutFeatureData.EnemyFeaturePropertyCondition[i];
                    foreach (CompareEnumCondition<EnemyFeatureProperty> enumCondition in owner.EnabledPropertiesHashSet)
                    {
                        isOn = enumCondition.CompareValue.CompareEnumWithCondition<EnemyFeatureProperty, int>(conditionData);
                        if (isOn)
                        {
                            break;
                        }
                    }

                }
            }
            return isOn;
        }
    }
}
