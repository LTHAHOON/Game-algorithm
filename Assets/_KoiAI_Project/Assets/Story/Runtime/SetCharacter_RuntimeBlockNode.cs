using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Story.GraphToolkit.Runtime
{
    public enum CharacterDireciton
    {
        LEFT,
        RIGHT
    }

    public enum CharacterAction
    {
        APPEAR,
        REMOVE
    }

    [Serializable]
    public class SetCharacter_RuntimeBlockNode : StoryRuntimeBlockNode
    {
        [SerializeField]
        private CharacterAction _characterAction;
        [SerializeField]
        private CharacterDireciton _characterDirection;
        [SerializeField]
        private Sprite _character;
        [SerializeField]
        private Vector2 _characterPosTranslate;
        [SerializeField]
        private Vector2 _characterScale;
        [SerializeField]
        private StoryAnimationInfo _characterAnimationInfo;
        [SerializeField]
        private StorySpriteSheetInfo _characterSpriteSheetInfo;

        public SetCharacter_RuntimeBlockNode(CharacterAction characterAction, CharacterDireciton characterDireciton, Sprite character, Vector2 characterPosTranslate, 
                                        Vector2 characterScale, StoryAnimationInfo characterAnimationInfo, StorySpriteSheetInfo characterSpriteSheetInfo)
        {
            _characterAction = characterAction;
            _characterDirection = characterDireciton;
            _character = character;
            _characterPosTranslate = characterPosTranslate;
            _characterScale = characterScale;
            _characterAnimationInfo = characterAnimationInfo;
            _characterSpriteSheetInfo = characterSpriteSheetInfo;
        }

        public override UniTask ExecuteAsync(StoryExecutionContext context)
        {
            context.StoryPresenterService.SetCharacter(_characterAction, _characterDirection, _character, _characterPosTranslate, _characterScale, _characterAnimationInfo, _characterSpriteSheetInfo);
            return UniTask.CompletedTask;
        }
    }
}
