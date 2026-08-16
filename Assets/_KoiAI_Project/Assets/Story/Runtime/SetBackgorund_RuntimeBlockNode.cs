using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Story.GraphToolkit.Runtime
{
    [Serializable]
    public class SetBackgorund_RuntimeBlockNode : StoryRuntimeBlockNode
    {
        [SerializeField]
        private Sprite _background;
        [SerializeField]
        private Color _backgroundColor;

        public SetBackgorund_RuntimeBlockNode(Sprite background, Color backgroundColor)
        {
            _background = background;
            _backgroundColor = backgroundColor;
        }
        
        public override UniTask ExecuteAsync(StoryExecutionContext context)
        {
            if (context == null || context.StoryPresenterService == null)
            {
                Debug.LogError("IStoryPresentationService가 없습니다.");

                return UniTask.CompletedTask;
            }

            context.StoryPresenterService.SetBackground(_background, _backgroundColor);
            return UniTask.CompletedTask;
        }
    }
}
