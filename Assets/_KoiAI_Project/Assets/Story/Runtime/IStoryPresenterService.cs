using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Story.GraphToolkit.Runtime
{
    public interface IStoryPresenterService
    {
        public void InitStorySequence();
        public void SetCharacter(CharacterAction characterAction, CharacterDireciton characterDireciton, Sprite character, Vector2 characterPosTranslate, 
                                Vector2 characterScale, StoryAnimationInfo characterAnimationInfo);
        public void SetBackground(Sprite background, Color backgroundColor, StoryAnimationInfo backgroundAnimationInfo);
        public UniTask SetDialogue(string characterName, string dialogueDescription, Color dialogueBackgroundColor, 
                                StoryAnimationInfo charNameAnimationInfo, StoryAnimationInfo dialogueDescriptionAnimationInfo, StoryAnimationInfo dialogueBackgroundAnimationInfo);
        public UniTask WaitForInput();
    }
}
