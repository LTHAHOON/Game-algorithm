using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace Story.GraphToolkit.Editor
{
    [Serializable]
    public class StorySequence_ContextNode : StoryContextNode
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputOutputPort(context);
        }
    }
}
