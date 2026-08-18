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
                .WithDefaultValue(new StoryAnimationInfo())
                .Build();
            context.AddInputPort<StoryAnimationInfo>(DIALOGUE_DESCRIPTION_ANIMATION_INFO)
                .WithDisplayName(DIALOGUE_DESCRIPTION_ANIMATION_INFO)
                .WithDefaultValue(new StoryAnimationInfo())
                .Build();
        }

        public override StoryRuntimeBlockNode CreateRuntimeBlockInstance()
        {
            IPort characterNamePort = GetInputPortByName(CHARACTER_NAME);
            IPort dialogueDescriptionPort = GetInputPortByName(DIALOGUE_DESCRIPTION);
            IPort dialogueBackgroundColorPort = GetInputPortByName(DIALOGUE_BACKGROUND_COLOR);

            characterNamePort.TryGetValue(out string characterName);
            dialogueDescriptionPort.TryGetValue(out string dialogueDescription);
            dialogueBackgroundColorPort.TryGetValue(out Color dialogueBackgroundColor);
            IPort charNameAnimationInfoPort = GetInputPortByName(CHAR_NAME_ANIMATION_INFO);
            IPort dialogueDescriptionAnimationInfoPort = GetInputPortByName(DIALOGUE_DESCRIPTION_ANIMATION_INFO);

            charNameAnimationInfoPort.TryGetValue(out StoryAnimationInfo charNameAnimationInfo);
            dialogueDescriptionAnimationInfoPort.TryGetValue(out StoryAnimationInfo dialogueDescriptionAnimationInfo);

            return new SetDialogue_RuntimeBlockNode(characterName, dialogueDescription, dialogueBackgroundColor, charNameAnimationInfo, dialogueDescriptionAnimationInfo);
        }
    }
}
