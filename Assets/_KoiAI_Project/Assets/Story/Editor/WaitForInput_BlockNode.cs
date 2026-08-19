using Story.GraphToolkit.Runtime;
using UnityEngine;

namespace Story.GraphToolkit.Editor
{
    public class WaitForInput_BlockNode : StoryBlockNode
    {
        
        public override StoryRuntimeBlockNode CreateRuntimeBlockInstance()
        {
            return new WaitForInput_RuntimeBlockNode();
        }
    }
}
