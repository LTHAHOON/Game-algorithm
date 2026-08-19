using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace Story.GraphToolkit.Editor
{
    public static class StoryPortExtension
    {
        /// <summary>
        /// IPort에서 연결된 노드의 값을 가져옵니다. (연결된 노드가 없으면 기본값을 가져옵니다.)
        /// </summary>
        public static bool TryGetValue_Extension<T>(this IPort port, out T value)
        {
            bool hasValue = false;
            value = default;

            if (port == null)
            {
                return false;
            }

            if (port.FirstConnectedPort == null)
            {
                hasValue = port.TryGetValue(out value);
            }
            else
            {
                INode node = port.FirstConnectedPort.GetNode();
                hasValue = node switch
                {
                    IVariableNode variableNode => variableNode.Variable.TryGetDefaultValue(out value),
                    IConstantNode constantNode => constantNode.TryGetValue(out value),
                    _ => false,
                };
            }

            return hasValue;
        }
    }
}
