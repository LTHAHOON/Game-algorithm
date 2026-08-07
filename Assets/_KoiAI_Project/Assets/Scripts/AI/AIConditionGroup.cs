using KoiAI.Enemy;
using System.Collections.Generic;
using UnityEngine;

namespace KoiAI.AI
{
    public sealed class AIConditionGroup
    {
        private sealed class RuntimeRule
        {
            public AITransitionCondition Condition;
            public bool Invert;
        }

        private readonly ConditionMatchMode _matchMode;
        private readonly List<RuntimeRule> _rules = new();

        public AIConditionGroup(AIConditionGroupData data)
        {
            if (data == null)
            {
                return;
            }

            _matchMode = data.MatchMode;

            foreach (AIConditionRule rule in data.Conditions)
            {
                AITransitionCondition condition = rule.CreateCondition();

                if (condition == null)
                {
                    continue;
                }

                _rules.Add(new RuntimeRule
                {
                    Condition = condition,
                    Invert = rule.Invert
                });
            }
        }

        public bool Check(AIBrain brain, bool emptyResult)
        {
            if (_rules.Count == 0)
            {
                return emptyResult;
            }

            if (_matchMode == ConditionMatchMode.All)
            {
                foreach (RuntimeRule rule in _rules)
                {
                    bool result = rule.Condition.Check(brain);

                    if (rule.Invert)
                    {
                        result = !result;
                    }

                    if (!result)
                    {
                        return false;
                    }
                }
                return true;
            }

            foreach (RuntimeRule rule in _rules)
            {
                bool result = rule.Condition.Check(brain);

                if (rule.Invert)
                {
                    result = !result;
                }

                if (result)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
