using System;
using Story.GraphToolkit.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace Story.GraphToolkit.Editor
{
    [Serializable]
    public abstract class StoryContextNode : ContextNode, IRuntimeNodeCreatable
    {

        protected void AddInputOutputPort(IPortDefinitionContext context)
        {
            context.AddInputPort<StoryFlow>(StoryFlow.ENTER)
                .WithDisplayName(StoryFlow.ENTER)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();

            context.AddOutputPort<StoryFlow>(StoryFlow.EXIT)
                .WithDisplayName(StoryFlow.EXIT)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }

        public abstract StoryRuntimeNode CreateRuntimeInstance();
    }
}
