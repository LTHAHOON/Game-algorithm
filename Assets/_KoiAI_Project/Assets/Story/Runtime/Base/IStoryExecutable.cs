using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Story.GraphToolkit.Runtime
{
    public interface IStoryExecutable
    {
        public UniTask ExecuteAsync(StoryExecutionContext context);
    }
}
