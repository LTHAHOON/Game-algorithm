using System;
using UnityEngine;

namespace Story.GraphToolkit.Runtime
{
    public class StoryExecutionContext
    {
        private IStoryPresenterService _storyPresenterService;
        
        public StoryExecutionContext(IStoryPresenterService storyPresenterService)
        {
            _storyPresenterService = storyPresenterService;
        }

        public IStoryPresenterService StoryPresenterService => _storyPresenterService;
    }
}
