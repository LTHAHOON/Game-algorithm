using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Story.GraphToolkit.Runtime
{
    public interface IStoryPresenterService
    {
        public void InitStorySequence();
        public void SetBackground(Sprite background, Color backgroundColor);
        public UniTask SetDialogue(string characterName, string dialogueDescription, Color dialogueBackgroundColor, 
                                StoryAnimationInfo charNameAnimationInfo, StoryAnimationInfo dialogueDescriptionAnimationInfo);
        public UniTask WaitForInput();
    }
}
