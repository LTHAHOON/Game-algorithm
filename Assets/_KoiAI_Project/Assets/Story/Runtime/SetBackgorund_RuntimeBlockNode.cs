using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Story.GraphToolkit.Runtime
{
    public class SetBackgorund_RuntimeBlockNode : StoryRuntimeNode
    {
        private Sprite _background;
        private Color _backgroundColor;
        
        public override UniTask ExecuteAsync()
        {
            return UniTask.CompletedTask;
        }
    }
}
