using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Story.GraphToolkit.Runtime
{
    public class StoryGraphRunner : MonoBehaviour
    {
        [SerializeField] 
        private StoryRuntimeGraph _storyRuntimeGraph;

        private StoryExecutionContext _storyExecutionContext;
        private const int _maxIterationCount = 500;
        private int _currentIterationCount = 0;
        private int _currentNodeIndex = -1;
        public void Initialize(IStoryPresenterService storyPresenterService)
        {
            _storyExecutionContext = new(storyPresenterService);
        }

        public async UniTask RunGraphAsync()
        {
            if (_storyRuntimeGraph == null || _storyExecutionContext == null)
            {
                Debug.LogError("StoryGraphRunner가 초기화되지 않았습니다.");
                return;
            }

            List<StoryRuntimeNode> runtimeNodes = _storyRuntimeGraph.StoryRuntimeNodes;
            if (runtimeNodes == null || runtimeNodes.Count == 0)
            {
                Debug.LogError("실행할 StoryRuntimeNode가 없습니다.");
                return;
            }

            _currentIterationCount = 0;
            _currentNodeIndex = runtimeNodes.FindIndex(runtimeNode => runtimeNode is StoryStart_RuntimeNode);
            if (_currentNodeIndex < 0)
            {
                Debug.LogError("StoryStart_RuntimeNode를 찾을 수 없습니다.");
                return;
            }

            while (_currentNodeIndex >= 0 && _currentNodeIndex < runtimeNodes.Count)
            {
                StoryRuntimeNode runtimeNode = runtimeNodes[_currentNodeIndex];
                if (runtimeNode == null)
                {
                    Debug.LogError($"StoryRuntimeNode가 null입니다. Index: {_currentNodeIndex}");
                    break;
                }

                await runtimeNode.ExecuteAsync(_storyExecutionContext);
                _currentNodeIndex = runtimeNode.NextNodeIndex;
                ++_currentIterationCount;
                if (_currentIterationCount >= _maxIterationCount)
                {
                    break;
                }
            }
        }

        public StoryExecutionContext StoryExecutionContext => _storyExecutionContext;
    }
}
