using Story.GraphToolkit.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Story.GraphToolkit.Editor
{
    [CustomPropertyDrawer(typeof(StoryAnimationInfo))]
    public class StoryAnimationInfoDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            Foldout foldout = new Foldout { text = string.Empty, value = false };
            root.Add(foldout);

            SerializedProperty easeTypeProperty = property.FindPropertyRelative(nameof(StoryAnimationInfo.EaseType));
            SerializedProperty delayTimeProperty = property.FindPropertyRelative(nameof(StoryAnimationInfo.DelayTime));
            SerializedProperty durationProperty = property.FindPropertyRelative(nameof(StoryAnimationInfo.Duration));

            foldout.Add(new PropertyField(easeTypeProperty, "Ease Type"));
            foldout.Add(new PropertyField(delayTimeProperty, "Delay Time"));
            foldout.Add(new PropertyField(durationProperty, "Duration"));
            return root;
        }
        
    }
}
