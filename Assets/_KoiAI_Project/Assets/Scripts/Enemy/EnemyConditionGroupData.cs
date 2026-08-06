using System;
using System.Collections.Generic;
using UnityEngine;

namespace KoiAI.Enemy
{
    [Serializable]
    public class EnemyConditionGroupData
    {
        [SerializeField]
        private ConditionMatchMode _matchMode;

        [SerializeField]
        private List<EnemyConditionRule> _conditions = new();

        public ConditionMatchMode MatchMode => _matchMode;
        public IReadOnlyList<EnemyConditionRule> Conditions => _conditions;
    }
}
