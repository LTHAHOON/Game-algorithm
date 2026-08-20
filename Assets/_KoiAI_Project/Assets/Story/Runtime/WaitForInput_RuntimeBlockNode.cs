using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Story.GraphToolkit.Runtime
{
    [Serializable]
    public class WaitForInput_RuntimeBlockNode : StoryRuntimeBlockNode
    {
        public override async UniTask ExecuteAsync(StoryExecutionContext context)
        {
            await context.StoryPresenterService.WaitForInput();
        }

    }
}
