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

            List<INode> allNodes = storyGraph.GetNodes().ToList();
            (int Index, ISubgraphNode SubGraphNode)[] subgraphNodes = allNodes
                                        .OfType<ISubgraphNode>()
                                        .Select(node => (allNodes.IndexOf(node), node))
                                        .ToArray();

            if (subgraphNodes.Length > 0)
            {
                List<(int Index, List<INode> Nodes)> subgraphNodeIndexAndNodes = new();
                for (int i = 0; i < subgraphNodes.Length; i++)
                {
                    List<INode> subgraphNodeList = subgraphNodes[i].SubGraphNode?
                        .GetSubgraph()?
                        .GetNodes()
                        .Where(n => n is IRuntimeNodeCreatable).ToList();                                      
                    if (subgraphNodeList == null || subgraphNodeList.Count == 0)
                    {
                        continue;
                    }
                    List<INode> sortedsubgraphNodeList = new();
                    INode startNode = subgraphNodeList.OfType<StoryStart_Node>().FirstOrDefault();
                    if (startNode == null)
                    {
                        continue;
                    }

                    INode curNode = startNode;
                    sortedsubgraphNodeList.Add(curNode);
                    for (int j = 0; j < subgraphNodeList.Count; j++)
                    {
                        var exitPort = curNode.GetOutputPortByName(StoryFlow.EXIT);
                        if (exitPort == null)
                        {
                            continue;
                        }
                        if(!exitPort.IsConnected)
                        {
                            break;
                        }
                        var nextNode = exitPort.FirstConnectedPort?.GetNode();
                        if (nextNode == null)
                        {
                            continue;
                        }
                        sortedsubgraphNodeList.Add(nextNode);
                        curNode = nextNode;
                    }
                    subgraphNodeIndexAndNodes.Add((subgraphNodes[i].Index, sortedsubgraphNodeList));
                }

                int count = 0;
                for (int i = 0; i < subgraphNodeIndexAndNodes.Count; i++)
                {
                    int index = subgraphNodeIndexAndNodes[i].Index;
                    List<INode> subgraphNodeList = subgraphNodeIndexAndNodes[i].Nodes;
                    allNodes.InsertRange((index + 1) + count, subgraphNodeList);
                    count += subgraphNodeList.Count;
                }
            }

            List<IRuntimeNodeCreatable> runtimeNodeCreators = allNodes
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
                if(i == runtimeNodes.Count - 1)
                {
                    runtimeNodes[i].NextNodeIndex = -1;
                    break;
                }
                runtimeNodes[i].NextNodeIndex = runtimeNodes.IndexOf(runtimeNodes[i + 1]);
            }

            return runtimeNodes;
        }

        //보류
        private static int FindNextNodeIndex(INode currentNode, Dictionary<INode, int> dicRuntimeNodeIndices)
        {
            IPort exitPort = currentNode.GetOutputPortByName(StoryFlow.EXIT);
            if (exitPort == null || !exitPort.IsConnected)
            {
                return -1;
            }

            IPort nextPort = exitPort.FirstConnectedPort;
            INode nextNode = nextPort.GetNode();
            if (nextNode is ISubgraphNode subgraphNode)
            {
                var subgraph = subgraphNode.GetSubgraph();
                nextNode = subgraph?
                    .GetNodes()
                    .OfType<StoryStart_Node>().FirstOrDefault();
            }
            int nextIndex = dicRuntimeNodeIndices.GetValueOrDefault(nextNode, -1);

            return nextIndex;
        }
    }
}
