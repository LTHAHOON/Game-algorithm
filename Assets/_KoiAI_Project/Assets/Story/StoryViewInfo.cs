using UnityEngine;

namespace KoiAI.UI
{
    [CreateAssetMenu(fileName =  "new StoryViewInfo", menuName = "KoiAI/UI/ViewInfo/StoryViewInfo")]
    public class StoryViewInfo : VisualViewInfo
    {
        [SerializeField] 
        private string _backgroundImageName;
        [SerializeField]
        private string _dialogueBackgroundName;
        [SerializeField]
        private string _backgroundSubImageName;
        [SerializeField]
        private string _backgroundOverlayName;
        [SerializeField]
        private string _dialogueCharacterName;
        [SerializeField]
        private string _dialogueDescriptionName;
        [SerializeField]
        private string _nextDialogueImageName;
        [SerializeField]
        private string _leftCharacterImageName;
        [SerializeField]
        private string _rightCharacterImageName;

        public string BackgroundImageName => _backgroundImageName;
        public string BackgroundSubImageName => _backgroundSubImageName;
        public string DialogueBackgroundName => _dialogueBackgroundName;
        public string BackgroundOverlayName => _backgroundOverlayName;

        public string DialogueCharacterName => _dialogueCharacterName;
        public string DialogueDescriptionName => _dialogueDescriptionName;
        public string NextDialogueImageName => _nextDialogueImageName;

        public string LeftCharacterImageName => _leftCharacterImageName;
        public string RightCharacterImageName => _rightCharacterImageName;
    }
}
