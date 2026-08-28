using System;
using Story.GraphToolkit.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace Story.GraphToolkit.Editor
{
    [Serializable]
    public class SetDialogue_BlockNode : StoryBlockNode
    {
        public const string CHARACTER_NAME = "Character Name";
        public const string DIALOGUE_DESCRIPTION = "Dialogue Description";
        public const string DIALOGUE_BACKGROUND_COLOR = "Dialogue Background Color";
        public const string CHAR_NAME_ANIMATION_INFO = "Character Name Animation Info";
        public const string DIALOGUE_DESCRIPTION_ANIMATION_INFO = "Dialogue Description Animation Info";
        public const string DIALOGUE_BACKGROUND_ANIMATION_INFO = "Dialogue Background Animation Info";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort<string>(CHARACTER_NAME)
                .WithDisplayName(CHARACTER_NAME)
                .Build();

            context.AddInputPort<string>(DIALOGUE_DESCRIPTION)
                .WithDisplayName(DIALOGUE_DESCRIPTION)
                .Build();

            context.AddInputPort<Color>(DIALOGUE_BACKGROUND_COLOR)
                .WithDisplayName(DIALOGUE_BACKGROUND_COLOR)
                .WithDefaultValue(Color.clear)
                .Build();

            context.AddInputPort<StoryAnimationInfo>(CHAR_NAME_ANIMATION_INFO)
                .WithDisplayName(CHAR_NAME_ANIMATION_INFO)
                .Build();

            context.AddInputPort<StoryAnimationInfo>(DIALOGUE_DESCRIPTION_ANIMATION_INFO)
                .WithDisplayName(DIALOGUE_DESCRIPTION_ANIMATION_INFO)
                .Build();

            context.AddInputPort<StoryAnimationInfo>(DIALOGUE_BACKGROUND_ANIMATION_INFO)
                .WithDisplayName(DIALOGUE_BACKGROUND_ANIMATION_INFO)
                .Build();

        }

        public override StoryRuntimeBlockNode CreateRuntimeBlockInstance()
        {
            IPort characterNamePort = GetInputPortByName(CHARACTER_NAME);
            IPort dialogueDescriptionPort = GetInputPortByName(DIALOGUE_DESCRIPTION);
            IPort dialogueBackgroundColorPort = GetInputPortByName(DIALOGUE_BACKGROUND_COLOR);

            IPort charNameAnimationInfoPort = GetInputPortByName(CHAR_NAME_ANIMATION_INFO);
            IPort dialogueDescriptionAnimationInfoPort = GetInputPortByName(DIALOGUE_DESCRIPTION_ANIMATION_INFO);
            IPort dialogueBackgroundAnimationInfoPort = GetInputPortByName(DIALOGUE_BACKGROUND_ANIMATION_INFO);

            characterNamePort.TryGetValue_Extension(out string characterName);
            dialogueDescriptionPort.TryGetValue_Extension(out string dialogueDescription);
            dialogueBackgroundColorPort.TryGetValue_Extension(out Color dialogueBackgroundColor);

            charNameAnimationInfoPort.TryGetValue_Extension(out StoryAnimationInfo charNameAnimationInfo);
            dialogueDescriptionAnimationInfoPort.TryGetValue_Extension(out StoryAnimationInfo dialogueDescriptionAnimationInfo);
            dialogueBackgroundAnimationInfoPort.TryGetValue_Extension(out StoryAnimationInfo dialogueBackgroundAnimationInfo);
            return new SetDialogue_RuntimeBlockNode(characterName, dialogueDescription, dialogueBackgroundColor,
                                    charNameAnimationInfo, dialogueDescriptionAnimationInfo, dialogueBackgroundAnimationInfo);
        }
    }
}
