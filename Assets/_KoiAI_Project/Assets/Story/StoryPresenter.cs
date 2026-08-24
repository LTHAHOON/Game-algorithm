using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using KoiAI.Input;
using Story.GraphToolkit.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
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
            visualView.BackgroundOverlay.style.backgroundColor = new(backgroundColor);
            visualView.BackgroundImage.style.backgroundImage = new(background);
        }

        public async UniTask SetDialogue(string characterName, string dialogueDescription, Color dialogueBackgroundColor,
                                StoryAnimationInfo charNameAnimationInfo, StoryAnimationInfo dialogueDescriptionAnimationInfo)
        {
            StoryView visualView = GetVisualView();
            visualView.DialogueBackground.style.backgroundColor = dialogueBackgroundColor;

            visualView.DialogueCharacterName.style.opacity = 0f;
            visualView.DialogueCharacterName.text = characterName;
            DOTween.To(() => visualView.DialogueCharacterName.style.opacity.value, x => visualView.DialogueCharacterName.style.opacity = x,
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
            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i];
                if (word == "\n")
                {
                    dialogueLine = OnAddDialogueLine();
                    continue;
                }

                Label wordLabel = new Label(word);
                wordLabel.style.opacity = 0f;
                dialogueLine.Add(wordLabel);
                Tween tween = DOTween.To(() => wordLabel.style.opacity.value, x => wordLabel.style.opacity = x,
                                1f, dialogueDescriptionAnimationInfo.Duration)
                                .SetDelay(i * dialogueDescriptionAnimationInfo.DelayTime)
                                .SetEase(dialogueDescriptionAnimationInfo.EaseType);

                if (i == words.Length - 1)
                {
                    await tween.AsyncWaitForCompletion();
                }
            }
        }

        public async UniTask WaitForInput()
        {
            bool onClick = false;
            StoryView visualView = GetVisualView();

            void WaitClick(InputAction.CallbackContext context)
            {
                if(context.control.device is Keyboard)
                {
                    Vector2 screenPosition = Mouse.current.position.ReadValue();
                    Vector2 panelPosition = RuntimePanelUtils.ScreenToPanel(visualView.Root.panel, screenPosition);
                    //UI ToolKit은 위치 기준이 Top-Left 방식이기 때문에 반전시켜줘야 합니다.
                    panelPosition.y = visualView.Root.layout.height - panelPosition.y;
                    if(!visualView.DialogueBackground.worldBound.Contains(panelPosition))
                    {
                        return;
                    }    
                }

                if (context.performed)
                {
                    onClick = true;
                    InputService.PlayerIA.Global.Click.performed -= WaitClick;
                }
            }
            InputService.PlayerIA.Global.Click.performed += WaitClick;
            await UniTask.WaitUntil(() => onClick);
        }

        public bool IsInitialized => _isInitialized;
    }
}
