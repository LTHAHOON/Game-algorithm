using System;
using System.Collections.Generic;
using System.Linq;
using Story.GraphToolkit.Runtime;

namespace Story.GraphToolkit.Editor
{
    [Serializable]
    public class StorySequence_ContextNode : StoryContextNode
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputOutputPort(context);
        }

        public override StoryRuntimeNode CreateRuntimeInstance()
        {
            List<StoryBlockNode> storyBlockNodes =  BlockNodes.OfType<StoryBlockNode>().ToList();
            List<StoryRuntimeBlockNode> storyRuntimeBlockNodes = StoryGraphRuntimeCreator.CreateRuntimeBlockNodes(storyBlockNodes);
            return new StorySequence_RuntimeContextNode(storyRuntimeBlockNodes);
        }
    }
}
