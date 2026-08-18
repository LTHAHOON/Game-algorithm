using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Story.GraphToolkit.Runtime
{
    [Serializable]
    public class SetDelay_RuntimeBlockNode : StoryRuntimeBlockNode
    {
        [SerializeField] 
        private float _delay = 1f;

        public SetDelay_RuntimeBlockNode(float delay)
        {
            _delay = delay;
        }
        
        public override UniTask ExecuteAsync(StoryExecutionContext context)
        {
            return UniTask.Delay(TimeSpan.FromSeconds(_delay));
        }
    }
}
