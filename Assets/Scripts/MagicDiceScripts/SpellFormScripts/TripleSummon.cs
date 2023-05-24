using System;
using System.Collections.Generic;
using BattleFieldScripts;
using CharactersScripts;
using ScriptableScripts;
using UnityEngine;

namespace SpellFormScripts
{
    public class TripleSummon : TargetingForm, ITargetingSummon
    {
        public event Action<Vector2Int, Character> ShowTargetSummon;

        private CharacterScriptable _summonedScriptable;

        private List<Character> _summonedCharacters;

        public TripleSummon(CharacterScriptable character) 
        {
            _summonedScriptable = character;
            ShowTargetSummon = delegate(Vector2Int vector2, Character character1) { };
        }
        
        
        public void UndoSummon()
        {
            foreach (var character in _summonedCharacters)
            {
                character.RemoveCharacter();
            }
        }
        
        
        public override void ShowEffectedPosition(Vector2Int positionInFieldArray)
        {
            _summonedCharacters = new List<Character>();
            var centerPosition = positionInFieldArray;
            
            List<Vector2Int> effectedPositions = new List<Vector2Int>();
            effectedPositions.Add(centerPosition);
            effectedPositions.Add(centerPosition - Vector2Int.right + Vector2Int.up);
            effectedPositions.Add(centerPosition - Vector2Int.right - Vector2Int.up);
            if (positionInFieldArray.y != 1)
            {
                effectedPositions.Add(centerPosition - Vector2Int.right);
            }

            foreach (var positionInArray in effectedPositions)
            {
                var character = CharacterFactoryWithPool.Instance.CreateCharacter(_summonedScriptable);
                _summonedCharacters.Add(character);
                ShowTargetSummon?.Invoke(positionInArray, character);
            }
        }
    }
}