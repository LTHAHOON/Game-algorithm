using UnityEngine;
using UnityEngine.UIElements;

namespace KoiAI.UI
{
    public class StoryPresenter : VisualPresenter<StoryView, StoryViewInfo>
    {
        protected override void Initalize(UIDocument uiDocument, ref StoryView visualView, StoryViewInfo visualViewInfo)
        {
            visualView = new StoryView(uiDocument.rootVisualElement, visualViewInfo);
            
        }

        public void SetBackgorund(Sprite background, Color backgroundColor)
        {
            StoryView visualView = GetVisualView();
            visualView.BackgroundImage.sprite = background;
            visualView.BackgroundImage.tintColor = backgroundColor;
        }
    }
}
