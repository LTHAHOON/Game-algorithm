using System.Collections.Generic;
using System.Linq;
using Story.GraphToolkit.Runtime;
using Unity.GraphToolkit.Editor;

namespace Story.GraphToolkit.Editor
{
    public interface IRuntimeNodeCreatable
    {
        public StoryRuntimeNode CreateRuntimeInstance();
    }
    
    public static class StoryGraphRuntimeCreator
    {
        public static List<StoryRuntimeNode> CreateRuntimeNodes(StoryGraph storyGraph)
        {
            Dictionary<INode, int> dicRuntimeNodeIndices = new();

            int index = 0;
            List<IRuntimeNodeCreatable> runtimeNodeCreators = storyGraph.GetNodes()
                .Select(node =>
                {
                    dicRuntimeNodeIndices.TryAdd(node, index);
                    ++index;
                    return node;
                }).OfType<IRuntimeNodeCreatable>().ToList();
            
            List<StoryRuntimeNode> runtimeNodes = runtimeNodeCreators
                .Select(creator =>
                {
                    StoryRuntimeNode storyRuntimeNode = creator.CreateRuntimeInstance();
                    if (creator is INode node && storyRuntimeNode != null)
                    {
                        int nextIndex = FindNextNodeIndex(node, dicRuntimeNodeIndices);
                        storyRuntimeNode.NextNodeIndex = nextIndex;
                    }
                    return storyRuntimeNode;
                })
                .Where(creator => creator != null).ToList();
            
            return runtimeNodes;
        }

        private static int FindNextNodeIndex(INode currentNode, Dictionary<INode, int> dicRuntimeNodeIndices)
        {
            IPort exitPort = currentNode.GetOutputPortByName(StoryBaseInputOutputName.EXIT);
            if (exitPort == null || !exitPort.IsConnected)
            {
                return -1;
            }
                
            IPort nextPort = exitPort.FirstConnectedPort;
            INode nextNode = nextPort.GetNode();
            int nextIndex = dicRuntimeNodeIndices.GetValueOrDefault(nextNode, -1);

            return nextIndex;
        }
    }
}
