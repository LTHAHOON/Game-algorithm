using UnityEngine;
using UnityEngine.UIElements;

namespace KoiAI.UI
{
    public class StoryView : VisualView<StoryViewInfo>
    {
        private Image _backgroundImage;
        private VisualElement _dialogueBackground;
        private Label _dialogueCharacterName;
        private Label _dialogueDescription;
        public StoryView(VisualElement root, StoryViewInfo info) : base(root, info) { }

        protected override void Initalize(VisualElement root, StoryViewInfo info)
        {
            _backgroundImage = root.Q<Image>(info.BackgroundImageName);
            _dialogueBackground = root.Q<VisualElement>(info.DialogueBackgroundName);
            _dialogueCharacterName = root.Q<Label>(info.DialogueCharacterName);
            _dialogueDescription = root.Q<Label>(info.DialogueDescriptionName);
        }

        public Image BackgroundImage => _backgroundImage;
        public VisualElement DialogueBackground => _dialogueBackground;
        public Label DialogueCharacterName => _dialogueCharacterName;
        public Label DialogueDescription => _dialogueDescription;
    }
}
