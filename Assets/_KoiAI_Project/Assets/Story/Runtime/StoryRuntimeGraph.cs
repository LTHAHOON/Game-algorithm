using System;
using System.Collections.Generic;
using UnityEngine;

namespace Story.GraphToolkit.Runtime
{
    public class StoryRuntimeGraph : ScriptableObject
    {
        [SerializeReference]
        private List<StoryRuntimeNode> _storyRuntimeNodes = new();

        public void Initialize(List<StoryRuntimeNode> storyRuntimeNodes)
        {
            _storyRuntimeNodes = storyRuntimeNodes;
        }
        
        public List<StoryRuntimeNode> StoryRuntimeNodes => _storyRuntimeNodes;
    }
}
