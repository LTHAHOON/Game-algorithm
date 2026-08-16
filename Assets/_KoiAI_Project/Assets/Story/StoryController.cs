using Cysharp.Threading.Tasks;
using KoiAI.UI;
using Story.GraphToolkit.Runtime;
using UnityEngine;

namespace KoiAI.Story
{
    public class StoryController : MonoBehaviour
    {
        [SerializeField]
        private StoryPresenter _storyPresenter;
        [SerializeField] 
        private StoryGraphRunner _storyGraphRunner;

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
