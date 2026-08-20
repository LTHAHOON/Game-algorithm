using Cysharp.Threading.Tasks;

using Story.GraphToolkit.Runtime;
using UnityEngine;

namespace KoiAI.Story
{
    using KoiAI.Input;
    using KoiAI.UI;

    public class StoryController : MonoBehaviour
    {
        [SerializeField]
        private StoryPresenter _storyPresenter;
        [SerializeField] 
        private StoryGraphRunner _storyGraphRunner;

        private void Awake()
        {
            InputService.ReconnectInputAction();
            InputService.SetEnableActionMap(InputActionMapContext.Global);
        }

        private void Start()
        {
            StartStory().Forget();
        }
        
        private async UniTask StartStory()
        {
            await UniTask.WaitUntil(() => _storyPresenter.IsInitialized, cancellationToken: destroyCancellationToken);
            
            _storyGraphRunner.Initialize(_storyPresenter);
            
            await _storyGraphRunner.RunGraphAsync();
        }
    }
}
