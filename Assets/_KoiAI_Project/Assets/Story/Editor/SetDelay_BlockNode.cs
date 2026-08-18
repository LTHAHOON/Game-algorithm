using System;
using Story.GraphToolkit.Runtime;
using Unity.GraphToolkit.Editor;

namespace Story.GraphToolkit.Editor
{
    [Serializable]
    public class SetDelay_BlockNode : StoryBlockNode
    {
        public const string DELAYTIME = "Delay Time";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort<float>(DELAYTIME)
                .WithDisplayName(DELAYTIME)
                .WithDefaultValue(1f)
                .Build();
        }

        public override StoryRuntimeBlockNode CreateRuntimeBlockInstance()
        {
            IPort delayTimePort = GetInputPortByName(DELAYTIME);
            delayTimePort.TryGetValue(out float delayTime);

            return new SetDelay_RuntimeBlockNode(delayTime);
        }
    }
}
