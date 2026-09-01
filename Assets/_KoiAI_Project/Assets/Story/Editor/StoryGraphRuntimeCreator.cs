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

    public interface IRuntimeBlockNodeCreatable
    {
        public StoryRuntimeBlockNode CreateRuntimeBlockInstance();
    }
    
    public static class StoryGraphRuntimeCreator
    {
        public static List<StoryRuntimeBlockNode> CreateRuntimeBlockNodes(List<StoryBlockNode> storyBlockNodes)
        {
            List<StoryRuntimeBlockNode> storyRuntimeBlockNodes = storyBlockNodes.OfType<IRuntimeBlockNodeCreatable>()
                .Select(node => node.CreateRuntimeBlockInstance())
                .Where(node => node != null)
                .ToList();

            return storyRuntimeBlockNodes;
        }
        
        public static List<StoryRuntimeNode> CreateRuntimeNodes(Graph storyGraph)
        {
            List<INode> editorNodes = new();
            List<StoryRuntimeNode> runtimeNodes = new();
            Dictionary<INode, int> dicRuntimeNodeIndices = new();

            if (storyGraph == null)
            {
                return runtimeNodes;
            }

            List<IRuntimeNodeCreatable> runtimeNodeCreators = storyGraph.GetNodes()
                .OfType<IRuntimeNodeCreatable>()
                .ToList();

            for (int i = 0; i < runtimeNodeCreators.Count; i++)
            {
                IRuntimeNodeCreatable creator = runtimeNodeCreators[i];
                if (creator is not INode editorNode)
                {
                    continue;
                }

                StoryRuntimeNode runtimeNode = creator.CreateRuntimeInstance();
                if (runtimeNode == null)
                {
                    continue;
                }

                int runtimeNodeIndex = runtimeNodes.Count;
                editorNodes.Add(editorNode);
                runtimeNodes.Add(runtimeNode);
                dicRuntimeNodeIndices.Add(editorNode, runtimeNodeIndex);
            }

            for (int i = 0; i < runtimeNodes.Count; i++)
            {
                runtimeNodes[i].NextNodeIndex = FindNextNodeIndex(editorNodes[i], dicRuntimeNodeIndices);
            }
            
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
