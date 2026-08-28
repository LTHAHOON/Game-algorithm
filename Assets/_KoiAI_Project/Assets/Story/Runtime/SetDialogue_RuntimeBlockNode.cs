using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Story.GraphToolkit.Runtime
{
    [Serializable]
    public class SetDialogue_RuntimeBlockNode : StoryRuntimeBlockNode
    {
        [SerializeField]
        private string _characterName;

        [SerializeField]
        private string _dialogueDescription;

        [SerializeField]
        private Color _dialogueBackgroundColor = Color.clear;
        [SerializeField]
        private StoryAnimationInfo _charNameAnimationInfo;
        [SerializeField]
        private StoryAnimationInfo _dialogueDescriptionAnimationInfo;
        [SerializeField]
        private StoryAnimationInfo _dialogueBackgroundAnimationInfo;

        public SetDialogue_RuntimeBlockNode(string characterName, string dialogueDescription, Color dialogueBackgroundColor, 
                                           StoryAnimationInfo charNameAnimationInfo, StoryAnimationInfo dialogueDescriptionAnimationInfo, StoryAnimationInfo dialogueBackgroundAnimationInfo)
        {
            _characterName = characterName;
            _dialogueDescription = dialogueDescription;
            _dialogueBackgroundColor = dialogueBackgroundColor;
            _charNameAnimationInfo = charNameAnimationInfo;
            _dialogueDescriptionAnimationInfo = dialogueDescriptionAnimationInfo;
            _dialogueBackgroundAnimationInfo = dialogueBackgroundAnimationInfo;
        }

        public override async UniTask ExecuteAsync(StoryExecutionContext context)
        {
            await context.StoryPresenterService.SetDialogue(_characterName, _dialogueDescription, _dialogueBackgroundColor,
                _charNameAnimationInfo, _dialogueDescriptionAnimationInfo, _dialogueBackgroundAnimationInfo);
        }
    }
}
