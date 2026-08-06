using System;
using System.Collections.Generic;
using UnityEngine;

namespace KoiAI.Utilities
{
    [Serializable]
    public class Wrapperlist<T>
    {
        [SerializeField]
        private List<T> _listValue;

        public List<T> ListValue => _listValue;
    }
}
