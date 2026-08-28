using System;
using Story.GraphToolkit.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace Story.GraphToolkit.Editor
{
    [Serializable]
    public class SetCharacter_BlockNode : StoryBlockNode
    {
        public const string CHARACTER_ACTION = "Character Action";
        public const string CHARACTER_DIRECTION = "Character Direction";
        public const string CHARACTER = "Character";
        public const string CHARACTER_POS_TRANSLATE = "Character Pos Translate";
        public const string CHARACTER_SCALE = "Character Scale";
        public const string CHARACTER_ANIMATION_INFO = "Character Animation Info";

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<CharacterAction>(CHARACTER_ACTION)
                .WithDisplayName(CHARACTER_ACTION)
                .WithDefaultValue(CharacterAction.APPEAR)
                .Build();

            context.AddOption<CharacterDireciton>(CHARACTER_DIRECTION)
                .WithDisplayName(CHARACTER_DIRECTION)
                .WithDefaultValue(CharacterDireciton.LEFT)
                .Build();
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            INodeOption chatacterActionOption = GetNodeOptionByName(CHARACTER_ACTION);
            chatacterActionOption.TryGetValue(out CharacterAction characterAction);

            if (characterAction == CharacterAction.APPEAR)
            {
                context.AddInputPort<Sprite>(CHARACTER)
                .WithDisplayName(CHARACTER)
                .Build();

                context.AddInputPort<Vector2>(CHARACTER_POS_TRANSLATE)
                .WithDisplayName(CHARACTER_POS_TRANSLATE)
                .WithDefaultValue(Vector2.zero)
                .Build();

                context.AddInputPort<Vector2>(CHARACTER_SCALE)
                .WithDisplayName(CHARACTER_SCALE)
                .WithDefaultValue(Vector2.one)
                .Build();
            }


            context.AddInputPort<StoryAnimationInfo>(CHARACTER_ANIMATION_INFO)
                .WithDisplayName(CHARACTER_ANIMATION_INFO)
                .Build();
        }

        public override StoryRuntimeBlockNode CreateRuntimeBlockInstance()
        {
            INodeOption chatacterActionOption = GetNodeOptionByName(CHARACTER_ACTION);
            INodeOption chatacterDirectionOption = GetNodeOptionByName(CHARACTER_DIRECTION);
            IPort characterAnimationInfoPort = GetInputPortByName(CHARACTER_ANIMATION_INFO);
            chatacterActionOption.TryGetValue(out CharacterAction characterAction);
            chatacterDirectionOption.TryGetValue(out CharacterDireciton characterDireciton);
            characterAnimationInfoPort.TryGetValue_Extension(out StoryAnimationInfo characterAnimationInfo);
            if (characterAction == CharacterAction.APPEAR)
            {
                IPort characterPort = GetInputPortByName(CHARACTER);
                IPort characterPosTranslatePort = GetInputPortByName(CHARACTER_POS_TRANSLATE);
                IPort characterScalePort = GetInputPortByName(CHARACTER_SCALE);
                characterPort.TryGetValue_Extension(out Sprite character);
                characterPosTranslatePort.TryGetValue_Extension(out Vector2 characterPosTranslate);
                characterScalePort.TryGetValue_Extension(out Vector2 characterScale);
                return new SetCharacter_RuntimeBlockNode(characterAction, characterDireciton, character, characterPosTranslate, characterScale, characterAnimationInfo);
            }
            return new SetCharacter_RuntimeBlockNode(characterAction, characterDireciton, null, Vector2.zero, Vector2.one, characterAnimationInfo);
        }
    }
}
