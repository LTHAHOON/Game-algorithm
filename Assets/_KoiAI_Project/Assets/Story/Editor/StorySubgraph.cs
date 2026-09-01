using System;
using Unity.GraphToolkit.Editor;

namespace Story.GraphToolkit.Editor
{
    [Subgraph(typeof(StoryGraph))]
    [Serializable]
    public class StorySubGraph : Graph
    {
    }
}
