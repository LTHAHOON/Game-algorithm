using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace Story.GraphToolkit.Editor
{
    [UseWithContext(typeof(StoryContextNode))]
    [Serializable]
    public abstract class StoryBlockNode : BlockNode
    {

        protected void AddInputOutputPort(IPortDefinitionContext context)
        {
        }
    }
}
