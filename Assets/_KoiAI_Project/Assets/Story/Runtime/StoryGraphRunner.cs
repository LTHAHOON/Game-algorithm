using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Story.GraphToolkit.Runtime
{
    public class StoryGraphRunner : MonoBehaviour
    {
        [SerializeField] 
        private StoryRuntimeGraph _storyRuntimeGraph;
        [SerializeField]
        private int _maxIterationCount = 500;

        private StoryExecutionContext _storyExecutionContext;
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
            _currentNodeIndex = runtimeNodes.FindIndex(runtimeNode => runtimeNode is StoryStart_RuntimeNode);
            _currentIterationCount = 0;

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
