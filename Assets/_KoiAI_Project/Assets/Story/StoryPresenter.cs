using System;
using System.Linq;
using System.Text.RegularExpressions;
using DG.Tweening;
using Story.GraphToolkit.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace KoiAI.UI
{
    public class StoryPresenter : VisualPresenter<StoryView, StoryViewInfo>, IStoryPresenterService
    {
        [SerializeField] 
        private StoryGraphRunner _storyGraphRunner;

        private bool _isInitialized = false;
        
        protected override void Initalize(UIDocument uiDocument, ref StoryView visualView, StoryViewInfo visualViewInfo)
        {
            visualView = new StoryView(uiDocument.rootVisualElement, visualViewInfo);
            _isInitialized = true;
        }

        public void SetBackground(Sprite background, Color backgroundColor)
        {
            StoryView visualView = GetVisualView();
            visualView.BackgroundImage.style.backgroundImage = new(background);
            visualView.BackgroundImage.style.backgroundColor = backgroundColor;
        }

        public void SetDialogue(string characterName, string dialogueDescription, Color dialogueBackgroundColor, 
                                StoryAnimationInfo charNameAnimationInfo, StoryAnimationInfo dialogueDescriptionAnimationInfo)
        {
            StoryView visualView = GetVisualView();
            visualView.DialogueBackground.style.backgroundColor = dialogueBackgroundColor;

            visualView.DialogueCharacterName.style.opacity = 0f;
            visualView.DialogueCharacterName.text = characterName;
            DOTween.To(()=> visualView.DialogueCharacterName.style.opacity.value, x => visualView.DialogueCharacterName.style.opacity = x, 
                1f, charNameAnimationInfo.Duration)
                .SetEase(charNameAnimationInfo.EaseType)
                .SetDelay(charNameAnimationInfo.DelayTime)
                .SetUpdate(true);

            visualView.DialogueDescription.text = string.Empty;
            visualView.DialogueDescription.Clear();
            
            
            Label OnAddDialogueLine()
            {
                Label dialogueLine = new Label();
                dialogueLine.style.marginBottom = -20f;
                dialogueLine.style.marginTop = 0f;
                dialogueLine.style.marginLeft = 0f;
                dialogueLine.style.marginRight = 0f;
                dialogueLine.style.flexDirection = FlexDirection.Row;
                visualView.DialogueDescription.Add(dialogueLine);
                return dialogueLine;
            }

            Label dialogueLine = OnAddDialogueLine();
            string fixedText = dialogueDescription.Replace("\\n", "\n");
            string[] words = fixedText.ToCharArray().Select(c => c.ToString()).ToArray();
            for(int i = 0; i < words.Length; i++)
            {
                string word = words[i];
                if(word == "\n")
                {
                    dialogueLine = OnAddDialogueLine();
                    continue;
                }

                Label wordLabel = new Label(word);
                wordLabel.style.opacity = 0f;
                dialogueLine.Add(wordLabel);

                DOTween.To(() => wordLabel.style.opacity.value, x => wordLabel.style.opacity = x,
                    1f, dialogueDescriptionAnimationInfo.Duration)
                    .SetDelay(i * dialogueDescriptionAnimationInfo.DelayTime)
                    .SetEase(dialogueDescriptionAnimationInfo.EaseType);
            }
        }
  
        public bool IsInitialized => _isInitialized;
    }
}
