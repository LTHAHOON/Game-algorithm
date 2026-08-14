using System;
using UnityEngine;

namespace Story.GraphToolkit.Editor
{
    [Serializable]
    public class SetBackground_BlockNode : StoryBlockNode
    {
        public const string BACKGROUND = "Background";
        public const string BACKGROUND_COLOR = "Background Color";
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort<Sprite>(BACKGROUND)
                .WithDisplayName(BACKGROUND)
                .Build();

            context.AddInputPort<Color>(BACKGROUND_COLOR)
                .WithDisplayName(BACKGROUND_COLOR)
                .Build();
        }
    }
}
