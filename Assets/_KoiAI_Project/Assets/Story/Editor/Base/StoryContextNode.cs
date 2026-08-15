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
            context.AddInputPort(StoryBaseInputOutputName.ENTER)
                .WithDisplayName(StoryBaseInputOutputName.ENTER)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();

            context.AddOutputPort(StoryBaseInputOutputName.EXIT)
                .WithDisplayName(StoryBaseInputOutputName.EXIT)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }

        public abstract StoryRuntimeNode CreateRuntimeInstance();
    }
}
