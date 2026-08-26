using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Story.GraphToolkit.Runtime
{
    [Serializable]
    public class StorySequence_RuntimeContextNode : StoryRuntimeNode
    {
        [SerializeReference] 
        private List<StoryRuntimeBlockNode> _runtimeBlockNodes = new();

        public StorySequence_RuntimeContextNode(List<StoryRuntimeBlockNode> runtimeBlockNodes)
        {
            _runtimeBlockNodes = runtimeBlockNodes;
        }
        
        public override async UniTask ExecuteAsync(StoryExecutionContext context)
        {
            if (_runtimeBlockNodes == null)
            {
                Debug.LogError("_runtimeBlockNodes is null in StorySequence_RuntimeContextNode. Please check the configuration.");
                return;
            }

            context.StoryPresenterService.InitStorySequence();
            for (int i = 0; i < _runtimeBlockNodes.Count; i++)
            {
                if (_runtimeBlockNodes[i] == null)
                {
                    continue;
                }
                await _runtimeBlockNodes[i].ExecuteAsync(context);
            }
        }
    }
}
