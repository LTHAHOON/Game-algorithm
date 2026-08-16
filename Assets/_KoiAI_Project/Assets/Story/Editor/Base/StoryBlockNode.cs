using System;
using Story.GraphToolkit.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace Story.GraphToolkit.Editor
{
    [UseWithContext(typeof(StoryContextNode))]
    [Serializable]
    public abstract class StoryBlockNode : BlockNode, IRuntimeBlockNodeCreatable
    {
        public abstract StoryRuntimeBlockNode CreateRuntimeBlockInstance();
    }
}
