using UnityEngine;
using UnityEngine.UIElements;

namespace KoiAI.UI
{
    public class StoryView : VisualView<StoryViewInfo>
    {
        private Image _backgroundImage;
        private Image _backgroundSubImage;
        private VisualElement _backgroundOverlay;
        private VisualElement _dialogueBackground;
        private Label _dialogueCharacterName;
        private Label _dialogueDescription;
        private Image _nextDialogueImage;
        private Image _leftCharacterImage;
        private Image _rightCharacterImage;

        public StoryView(VisualElement root, StoryViewInfo info) : base(root, info) { }

        protected override void Initalize(VisualElement root, StoryViewInfo info)
        {
            _backgroundImage = root.Q<Image>(info.BackgroundImageName);
            _backgroundSubImage = root.Q<Image>(info.BackgroundSubImageName);
            _backgroundOverlay = root.Q<VisualElement>(info.BackgroundOverlayName);
            _dialogueBackground = root.Q<VisualElement>(info.DialogueBackgroundName);
            _dialogueCharacterName = root.Q<Label>(info.DialogueCharacterName);
            _dialogueDescription = root.Q<Label>(info.DialogueDescriptionName);
            _nextDialogueImage = root.Q<Image>(info.NextDialogueImageName);
            _leftCharacterImage = root.Q<Image>(info.LeftCharacterImageName);
            _rightCharacterImage = root.Q<Image>(info.RightCharacterImageName);
        }

        public Image BackgroundImage => _backgroundImage;
        public Image BackgroundSubImage => _backgroundSubImage;
        public VisualElement BackgroundOverlay => _backgroundOverlay;
        public VisualElement DialogueBackground => _dialogueBackground;
        public Label DialogueCharacterName => _dialogueCharacterName;
        public Label DialogueDescription => _dialogueDescription;
        public Image NextDialogueImage => _nextDialogueImage;
        public Image LeftCharacterImage => _leftCharacterImage;
        public Image RightCharacterImage => _rightCharacterImage;
    }
}
