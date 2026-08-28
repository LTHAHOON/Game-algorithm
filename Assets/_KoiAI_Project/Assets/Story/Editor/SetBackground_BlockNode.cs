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
        public const string BACKGROUND_ANIMATION_INFO = "Background Animation Info";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort<Sprite>(BACKGROUND)
                .WithDisplayName(BACKGROUND)
                .Build();

            context.AddInputPort<Color>(BACKGROUND_COLOR)
                .WithDisplayName(BACKGROUND_COLOR)
                .WithDefaultValue(Color.white)
                .Build();
            
            context.AddInputPort<StoryAnimationInfo>(BACKGROUND_ANIMATION_INFO)
                .WithDisplayName(BACKGROUND_ANIMATION_INFO)
                .Build();
        }

        public override StoryRuntimeBlockNode CreateRuntimeBlockInstance()
        {
            IPort backgroundPort = GetInputPortByName(BACKGROUND);
            IPort backgroundColorPort = GetInputPortByName(BACKGROUND_COLOR);
            IPort backgroundAnimationInfoPort = GetInputPortByName(BACKGROUND_ANIMATION_INFO);

            backgroundColorPort.TryGetValue_Extension(out Color backgroundColor);
            backgroundPort.TryGetValue_Extension(out Sprite background);
            backgroundAnimationInfoPort.TryGetValue_Extension(out StoryAnimationInfo backgroundAnimationInfo);
            
            return new SetBackgorund_RuntimeBlockNode(background, backgroundColor, backgroundAnimationInfo);
        }
    }
}
