using UnityEngine;

namespace KoiAI.UI
{
    [CreateAssetMenu(fileName =  "new StoryViewInfo", menuName = "KoiAI/UI/ViewInfo/StoryViewInfo")]
    public class StoryViewInfo : VisualViewInfo
    {
        [SerializeField] 
        private string _backgroundImageName;

        public string BackgroundImageName => _backgroundImageName;
    }
}
