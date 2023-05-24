using System;
using BattleFieldScripts;
using CharactersScripts;
using ScriptableScripts;
using UnityEngine;

namespace SpellFormScripts
{
    public class SingleTargetSummonForm : TargetingForm, ITargetingSummon
    {
        public event Action<Vector2Int, Character> ShowTargetSummon ;
        private CharacterScriptable _characterScriptable;
        private Character _character;
        public SingleTargetSummonForm(CharacterScriptable character) 
        {
            _characterScriptable = character;
            ShowTargetSummon = delegate(Vector2Int vector2, Character character1) { };
        }
        
        public override void ShowEffectedPosition(Vector2Int positionInFieldArray)
        {
            if (_character == null)
            {
                _character = CharacterFactoryWithPool.Instance.CreateCharacter(_characterScriptable);
            }
            /*else
            {
                _character.Setup(_characterScriptable);
            }*/
            ShowTargetSummon?.Invoke(positionInFieldArray, _character);
        }
        
        public void UndoSummon()
        {
            if (_character != null)
            {
                _character.RemoveCharacter();
            }
        }
    }
}