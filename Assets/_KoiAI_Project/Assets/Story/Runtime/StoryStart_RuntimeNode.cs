using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Story.GraphToolkit.Runtime
{
    public class StoryStart_RuntimeNode : StoryRuntimeNode
    {
        public override UniTask ExecuteAsync()
        {
            return UniTask.CompletedTask;
        }
    }
}
