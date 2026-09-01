using UnityEditor;
using Unity.GraphToolkit.Editor;
using System;

namespace Story.GraphToolkit.Editor
{

    [Graph(AssetExtension, GraphOptions.SupportsSubgraphs)]
    [Serializable]
    public class StoryGraph : Graph
    {
        public const string AssetExtension = "StoryGraph";

        [MenuItem("Assets/CustomGraph/Story/StoryGraph")]
        public static void CreateStoryGraph()
        {
            GraphDatabase.PromptInProjectBrowserToCreateNewAsset<StoryGraph>("new StoryGraph");
        }
    }

    [Subgraph(typeof(StoryGraph))]
    public class StorySubGraph : Graph
    {
        
    }
}
