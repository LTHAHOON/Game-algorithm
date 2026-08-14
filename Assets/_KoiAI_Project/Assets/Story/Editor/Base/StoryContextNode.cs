using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace Story.GraphToolkit.Editor
{
    [Serializable]
    public abstract class StoryContextNode : ContextNode
    {
        public const string ENTER = "Enter";
        public const string EXIT = "Exit";  

        protected void AddInputOutputPort(IPortDefinitionContext context)
        {
            context.AddInputPort(ENTER)
                .WithDisplayName(ENTER)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();

            context.AddOutputPort(EXIT)
                .WithDisplayName(EXIT)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
    }
}
