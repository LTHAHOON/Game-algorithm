using System;
using Story.GraphToolkit.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace Story.GraphToolkit.Editor
{
    [Serializable]
    public abstract class StoryNode : Node, IRuntimeNodeCreatable
    {
        protected void AddInputOutputPort(IPortDefinitionContext context)
        {
            context.AddInputPort(StoryFlow.ENTER)
                .WithDisplayName(StoryFlow.ENTER)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();

            context.AddOutputPort(StoryFlow.EXIT)
                .WithDisplayName(StoryFlow.EXIT)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }

        public abstract StoryRuntimeNode CreateRuntimeInstance();
    }
}
