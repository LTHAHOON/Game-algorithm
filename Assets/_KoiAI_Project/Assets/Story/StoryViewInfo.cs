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
        private string _backgroundOverlayName;
        [SerializeField]
        private string _dialogueCharacterName;
        [SerializeField]
        private string _dialogueDescriptionName;


        public string BackgroundImageName => _backgroundImageName;
        public string DialogueBackgroundName => _dialogueBackgroundName;
        public string BackgroundOverlayName => _backgroundOverlayName;

        public string DialogueCharacterName => _dialogueCharacterName;
        public string DialogueDescriptionName => _dialogueDescriptionName;
    }
}
