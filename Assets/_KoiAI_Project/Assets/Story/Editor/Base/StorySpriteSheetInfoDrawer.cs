using System.Linq;
using Story.GraphToolkit.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Story.GraphToolkit.Editor
{
    [CustomPropertyDrawer(typeof(StorySpriteSheetInfo))]
    public class StorySpriteSheetInfoDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new();
            root.style.flexDirection = FlexDirection.Column;

            Foldout foldout = new();
            root.Add(foldout);
            SerializedProperty sheetToFindProperty = property.FindPropertyRelative(nameof(StorySpriteSheetInfo.SheetToFind));
            SerializedProperty framesProperty = property.FindPropertyRelative(nameof(StorySpriteSheetInfo.Frames));
            SerializedProperty frameRateProperty =  property.FindPropertyRelative(nameof(StorySpriteSheetInfo.FrameRate));

            foldout.Add(new PropertyField(sheetToFindProperty));
            foldout.Add(new PropertyField(framesProperty));
            foldout.Add(new PropertyField(frameRateProperty));
            foldout.style.marginRight = 100f;

            Button findFramesBtn = new() { text = "Find Frames" };
            findFramesBtn.clicked += () =>
            {
                Texture2D sheetTexture = sheetToFindProperty.objectReferenceValue as Texture2D;
                if (sheetTexture == null)
                {
                    return;
                }

                string assetPath = AssetDatabase.GetAssetPath(sheetTexture);
                if (string.IsNullOrEmpty(assetPath))
                {
                    return;
                }

                Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(assetPath)
                    .OfType<Sprite>()
                    .ToArray();

                if (sprites == null || sprites.Length == 0)
                {
                    return;
                }

                framesProperty.ClearArray();
                for (int i = 0; i < sprites.Length; i++)
                {
                    framesProperty.InsertArrayElementAtIndex(i);
                    SerializedProperty frameProperty = framesProperty.GetArrayElementAtIndex(i);
                    frameProperty.objectReferenceValue = sprites[i];
                }

                property.serializedObject.ApplyModifiedProperties();
            };

            foldout.Add(findFramesBtn);
            return root;
        }
    }
}
