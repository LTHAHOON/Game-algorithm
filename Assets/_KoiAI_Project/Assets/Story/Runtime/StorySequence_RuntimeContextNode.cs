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
            _runtimeBlockNodes = runtimeBlockNodes ?? new List<StoryRuntimeBlockNode>();
        }
        
        public override async UniTask ExecuteAsync(StoryExecutionContext context)
        {
            if (_runtimeBlockNodes == null)
            {
                Debug.LogError("RuntimeBlockNode 리스트가 역직렬화되지 않았습니다. StoryGraph를 다시 Import해주세요.");
                return;
            }

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
