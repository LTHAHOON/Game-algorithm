using System;
using Story.GraphToolkit.Runtime;
using Unity.GraphToolkit.Editor;
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
                .WithDefaultValue(Color.white)
                .Build();
        }

        public override StoryRuntimeBlockNode CreateRuntimeBlockInstance()
        {
            IPort backgroundPort = GetInputPortByName(BACKGROUND);
            backgroundPort.TryGetValue(out Sprite background);

            IPort backgroundColorPort = GetInputPortByName(BACKGROUND_COLOR);
            backgroundColorPort.TryGetValue(out Color backgroundColor);

            return new SetBackgorund_RuntimeBlockNode(background, backgroundColor);
        }
    }
}
