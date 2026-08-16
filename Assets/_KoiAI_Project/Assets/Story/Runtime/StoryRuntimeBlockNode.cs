using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Story.GraphToolkit.Runtime
{
    [Serializable]
    public abstract class StoryRuntimeBlockNode : IStoryExecutable
    {
        public abstract UniTask ExecuteAsync(StoryExecutionContext context);
    }
}
