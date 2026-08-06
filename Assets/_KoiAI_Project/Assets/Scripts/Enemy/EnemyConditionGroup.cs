using System.Collections.Generic;
using UnityEngine;

namespace KoiAI.Enemy
{
    public sealed class EnemyConditionGroup
    {
        private sealed class RuntimeRule
        {
            public EnemyTransitionCondition Condition;
            public bool Invert;
        }

        private readonly ConditionMatchMode _matchMode;
        private readonly List<RuntimeRule> _rules = new();

        public EnemyConditionGroup(
            EnemyConditionGroupData data)
        {
            if (data == null)
                return;

            _matchMode = data.MatchMode;

            foreach (EnemyConditionRule rule in data.Conditions)
            {
                EnemyTransitionCondition condition =
                    rule.CreateCondition();

                if (condition == null)
                    continue;

                _rules.Add(new RuntimeRule
                {
                    Condition = condition,
                    Invert = rule.Invert
                });
            }
        }

        public bool Check(
            EnemyAI owner,
            bool emptyResult)
        {
            if (_rules.Count == 0)
                return emptyResult;

            if (_matchMode == ConditionMatchMode.All)
            {
                foreach (RuntimeRule rule in _rules)
                {
                    bool result =
                        rule.Condition.Check(owner);

                    if (rule.Invert)
                        result = !result;

                    if (!result)
                        return false;
                }

                return true;
            }

            foreach (RuntimeRule rule in _rules)
            {
                bool result =
                    rule.Condition.Check(owner);

                if (rule.Invert)
                    result = !result;

                if (result)
                    return true;
            }

            return false;
        }
    }
}
