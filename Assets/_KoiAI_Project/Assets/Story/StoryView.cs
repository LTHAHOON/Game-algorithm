using UnityEngine;
using UnityEngine.UIElements;

namespace KoiAI.UI
{
    public class StoryView : VisualView<StoryViewInfo>
    {
        private Image _backgroundImage;
        public StoryView(VisualElement root, StoryViewInfo info) : base(root, info) { }

        protected override void Initalize(VisualElement root, StoryViewInfo info)
        {
            _backgroundImage = root.Q<Image>(info.BackgroundImageName);
        }

        public Image BackgroundImage => _backgroundImage;
    }
}
