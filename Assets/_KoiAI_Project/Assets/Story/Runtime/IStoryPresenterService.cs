using UnityEngine;

namespace Story.GraphToolkit.Runtime
{
    public interface IStoryPresenterService
    {
        public void SetBackground(Sprite background, Color backgroundColor);
        public void SetDialogue(string characterName, string dialogueDescription, Color dialogueBackgroundColor, 
                                StoryAnimationInfo charNameAnimationInfo, StoryAnimationInfo dialogueDescriptionAnimationInfo);
    }
}
