using System;
using Story.GraphToolkit.Runtime;
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
            visualView.BackgroundImage.sprite = background;
            visualView.BackgroundImage.tintColor = backgroundColor;
        }

        public bool IsInitialized => _isInitialized;
    }
}
