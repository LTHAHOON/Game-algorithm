using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using Story.GraphToolkit.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace KoiAI.UI
{
    using KoiAI.Input;
    using KoiAI.KoiCursor;

    public class StoryPresenter : VisualPresenter<StoryView, StoryViewInfo>, IStoryPresenterService
    {
        [SerializeField]
        private StoryGraphRunner _storyGraphRunner;

        private UIScaleTransition _nextDialogueImage_Transition;
        private bool _isInitialized = false;

        protected override void Initalize(UIDocument uiDocument, ref StoryView visualView, StoryViewInfo visualViewInfo)
        {
            visualView = new StoryView(uiDocument.rootVisualElement, visualViewInfo);
            _nextDialogueImage_Transition = new UIScaleTransition(gameObject, Ease.Linear, visualView.NextDialogueImage, 1f, new Vector2(1f, 1f), new Vector2(0.7f, 0.7f), -1, LoopType.Yoyo);
            _isInitialized = true;
        }

        public void InitStorySequence()
        {
            StoryView visualView = GetVisualView();
            visualView.NextDialogueImage.style.opacity = 0f;
            visualView.DialogueBackground.style.display = DisplayStyle.None;
            visualView.BackgroundImage.style.backgroundColor = Color.black;
            visualView.BackgroundSubImage.style.backgroundColor = Color.black;
        }

        public void SetCharacter(CharacterAction characterAction, CharacterDireciton characterDireciton, Sprite character, Vector2 characterPosTranslate,
                                    Vector2 characterScale, StoryAnimationInfo characterAnimationInfo, StorySpriteSheetInfo characterSpriteSheetInfo)
        {
            StoryView visualView = GetVisualView();
            Image characterImage = characterDireciton switch
            {
                CharacterDireciton.LEFT => visualView.LeftCharacterImage,
                CharacterDireciton.RIGHT => visualView.RightCharacterImage,
                _ => null
            };
            if (characterImage == null)
            {
                return;
            }

            if(characterSpriteSheetInfo.IsValid())
            {
                int index = 0;
                Tween spriteSheetSequence = DOTween.Sequence()
                    .SetLoops(characterSpriteSheetInfo.Frames.Length)
                    .AppendCallback(()=>{
                        characterImage.style.backgroundImage = new(characterSpriteSheetInfo.Frames[index]);
                        index = (index + 1) % characterSpriteSheetInfo.Frames.Length;
                    })
                    .SetDelay(characterSpriteSheetInfo.FrameRate)
                    .OnComplete(() => {
                        if(character)
                        {
                            characterImage.style.backgroundImage = new(character);
                        }
                    });
            }

            float targetOpacity = 0f;
            switch (characterAction)
            {
                case CharacterAction.APPEAR:
                    targetOpacity = 1f;
                    characterImage.style.opacity = 0f;
                    characterImage.style.width = character.texture.width * characterScale.x;
                    characterImage.style.height = character.texture.height * characterScale.y;
                    if(!characterSpriteSheetInfo.IsValid())
                    {
                        characterImage.style.backgroundImage = new(character);
                    }
                    characterImage.style.translate = characterPosTranslate;
                    break;
                case CharacterAction.REMOVE:
                    targetOpacity = 0f;
                    break;
            };
            FadeToVisual(() => characterImage.style.opacity.value, x => characterImage.style.opacity = x,
                targetOpacity, characterAnimationInfo);
        }
        
        public void SetBackground(Sprite background, Color backgroundColor, StoryAnimationInfo backgroundAnimationInfo)
        {
            StoryView visualView = GetVisualView();
            visualView.BackgroundOverlay.style.backgroundColor = new(backgroundColor);
            visualView.BackgroundSubImage.style.backgroundImage = visualView.BackgroundImage.style.backgroundImage;
            visualView.BackgroundImage.style.backgroundImage = new(background);
            visualView.BackgroundImage.style.opacity = 0f;

            FadeToVisual(() => visualView.BackgroundImage.style.opacity.value, x => visualView.BackgroundImage.style.opacity = x,
                1f, backgroundAnimationInfo);
        }


        public async UniTask SetDialogue(string characterName, string dialogueDescription, Color dialogueBackgroundColor,
                                    StoryAnimationInfo charNameAnimationInfo, StoryAnimationInfo dialogueDescriptionAnimationInfo, StoryAnimationInfo dialogueBackgroundAnimationInfo)
        {
            StoryView visualView = GetVisualView();
            _nextDialogueImage_Transition?.StopTransition();
            visualView.DialogueBackground.style.display = DisplayStyle.Flex;
            visualView.DialogueBackground.style.backgroundColor = dialogueBackgroundColor;

            visualView.DialogueCharacterName.style.opacity = 0f;
            visualView.DialogueCharacterName.text = characterName;

            FadeToVisual(() => visualView.DialogueBackground.style.opacity.value, 
                            (x) => visualView.DialogueBackground.style.opacity = x, 1f,dialogueBackgroundAnimationInfo);

            FadeToVisual(() => visualView.DialogueCharacterName.style.opacity.value, (x) =>
                {
                    visualView.DialogueCharacterName.style.opacity = x;
                    visualView.NextDialogueImage.style.opacity = x;
                }, 1f, charNameAnimationInfo);

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
                Tween dialogueTween = FadeToVisual(() => wordLabel.style.opacity.value, x => wordLabel.style.opacity = x, 
                                                    1f, dialogueDescriptionAnimationInfo);
                if (i == words.Length - 1)
                {
                    await dialogueTween.AsyncWaitForCompletion();
                    _nextDialogueImage_Transition?.ActivateTransition();
                }
            }
        }

        public async UniTask WaitForInput()
        {
            bool onClick = false;
            StoryView visualView = GetVisualView();
            CursorService.RegisterElementCursor<PointerLeaveEvent>(visualView.DialogueBackground, CursorType.Base, CursorMode.Auto);
            CursorService.RegisterElementCursor<PointerEnterEvent>(visualView.DialogueBackground, CursorType.Hover, CursorMode.Auto);
            bool bPointerOver = CursorService.CheckPointerOverElement(visualView, visualView.DialogueBackground);
            if(bPointerOver)
            {
                CursorService.SetCursor(CursorType.Hover);
            }
            void WaitClick(InputAction.CallbackContext context)
            {
                bool bPointerOver = CursorService.CheckPointerOverElement(visualView, visualView.DialogueBackground);
                if (context.performed && bPointerOver)
                {
                    onClick = true;
                    CursorService.UnregisterElementCursor<PointerLeaveEvent>(visualView.DialogueBackground);
                    CursorService.UnregisterElementCursor<PointerEnterEvent>(visualView.DialogueBackground);
                    CursorService.SetCursor(CursorType.Base);
                    InputService.PlayerIA.Global.Click.performed -= WaitClick;
                }
            }
            InputService.PlayerIA.Global.Click.performed += WaitClick;
            await UniTask.WaitUntil(() => onClick);
        }

        

        private Tween FadeToVisual(DOGetter<float> dOGetter, DOSetter<float> dOSetter, float targetOpacity, StoryAnimationInfo animationInfo)
        {
            return DOTween.To(dOGetter, dOSetter,
                    targetOpacity, animationInfo.Duration)
                    .SetEase(animationInfo.EaseType)
                    .SetDelay(animationInfo.DelayTime)
                    .SetUpdate(UpdateType.Late, true);
        }
        
        public bool IsInitialized => _isInitialized;
    }
}
