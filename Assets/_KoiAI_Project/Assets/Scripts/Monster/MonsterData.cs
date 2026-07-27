
using KoiAI.AnimatorSystem;
using NaughtyAttributes;
using UnityEngine;

namespace KoiAI.Monster
{
    [CreateAssetMenu(fileName = "new MonsterData", menuName = "KoiAI/Monster/MonsterData")]
    public class MonsterData : ScriptableObject
    {
        [SerializeField]
        private string _characterBaseName;

        [Space(10)]
        [HorizontalLine(5, EColor.Gray)]
        [Space(10)]
        [SerializeField]
        private AnimatorData _animatorData;

        public AnimatorData AnimatorData => _animatorData;

    }
}
