using System;
using Story.GraphToolkit.Runtime;

namespace Story.GraphToolkit.Editor
{
    [Serializable]
    public class WaitForInput_BlockNode : StoryBlockNode
    {
        public override StoryRuntimeBlockNode CreateRuntimeBlockInstance()
        {
            return new WaitForInput_RuntimeBlockNode();
        }
    }
}
