using System;
using Story.GraphToolkit.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace Story.GraphToolkit.Editor
{
    [Serializable]
    public class StoryStart_Node : Node, IRuntimeNodeCreatable
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddOutputPort<StoryFlow>(StoryFlow.EXIT)
                .WithDisplayName("Execute")
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }

        public StoryRuntimeNode CreateRuntimeInstance()
        {
            return new StoryStart_RuntimeNode();
        }
    }
}
