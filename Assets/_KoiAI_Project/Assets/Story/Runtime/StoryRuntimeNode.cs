using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Story.GraphToolkit.Runtime
{
    [Serializable]
    public abstract class StoryRuntimeNode
    {
        [SerializeField]
        private int _nextNodeIndex;

        public int NextNodeIndex
        {
            get => _nextNodeIndex;
            set => _nextNodeIndex = value;
        }

        public abstract UniTask ExecuteAsync();
    }
}
