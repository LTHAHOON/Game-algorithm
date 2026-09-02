using System;
using Unity.GraphToolkit.Editor;
using UnityEditor;
using static Unity.GraphToolkit.Editor.Node;

namespace Story.GraphToolkit.Editor
{
    [Graph("Story Sub Graph", GraphOptions.Default)]
    [Subgraph(typeof(StoryGraph))]
    [Serializable]
    public class StorySubGraph : Graph
    {
        [MenuItem("Assets/CustomGraph/Story/StorySubGraph")]
        private static void CreateStorySubGraph()
        {
            GraphDatabase.PromptInProjectBrowserToCreateNewAsset<StorySubGraph>("new StorySubGraph");
        }
    }
}
