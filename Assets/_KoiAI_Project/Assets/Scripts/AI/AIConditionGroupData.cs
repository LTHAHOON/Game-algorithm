using System;
using System.Collections.Generic;
using UnityEngine;

namespace KoiAI.AI
{
    [Serializable]
    public class AIConditionGroupData
    {
        [SerializeField]
        private ConditionMatchMode _matchMode;

        [SerializeField]
        private List<AIConditionRule> _conditions = new();

        public ConditionMatchMode MatchMode => _matchMode;
        public IReadOnlyList<AIConditionRule> Conditions => _conditions;
    }
}
