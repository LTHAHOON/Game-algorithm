using System.Collections.Generic;
using UnityEngine;

namespace KoiAI.Utilities
{
    public abstract class ActivateRandomGroup<T> : MonoBehaviour where T: Component
    {
        [SerializeField]
        private List<ActivateRandomValue<T>> _activateTargets;


        public List<ActivateRandomValue<T>> ActivateTargets => _activateTargets;
    }
}
