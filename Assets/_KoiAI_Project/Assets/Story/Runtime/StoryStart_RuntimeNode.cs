using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Story.GraphToolkit.Runtime
{
    [Serializable]
    public class StoryStart_RuntimeNode : StoryRuntimeNode
    {
        public override UniTask ExecuteAsync(StoryExecutionContext context)
        {
            return UniTask.CompletedTask;
        }
    }
}
