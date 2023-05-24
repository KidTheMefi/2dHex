using System.Collections.Generic;
using ScriptableScripts;
using UnityEngine;

namespace TeamCreation
{
    public class ComputerTeam
    {
        private List<Vector2Int> _allPosition;
        private Dictionary<Vector2Int, CharacterScriptable> CharactersInPosition;
        
        public ComputerTeam(int lvl)
        {
            _allPosition = new List<Vector2Int>();
            CharactersInPosition = new Dictionary<Vector2Int, CharacterScriptable>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    _allPosition.Add(new Vector2Int(i,j));
                }
            }
            
            lvl = lvl < 1 ? 1 : lvl > 11 ? 11 : lvl;

            int regularCharactersCount = lvl % 2 == 0 ? 3 : 4;
            int strongCharactersCount = lvl / 2;


            int highLvl = lvl - 8;
            if (highLvl > 0)
            {
                strongCharactersCount = 4 + highLvl;
                regularCharactersCount = 3 - highLvl;
            }
            
            for (int i = 0; i < strongCharactersCount; i++)
            {
                CreateStrongCharacter(GetEmptyPosition());
            }
            
            for (int i = 0; i < regularCharactersCount; i++)
            {
                CreateRegularCharacter(GetEmptyPosition());
            }
            
            SaveCharacters();
        }

        private void CreateRegularCharacter(Vector2Int pos)
        {
            var character = pos.x switch
            {
                0 => ScriptableCharactersList.GetRandomRegularMeleeCharacter(),
                2 => ScriptableCharactersList.GetRandomRegularDistanceCharacter(),
                _ => ScriptableCharactersList.GetRandomRegularMeleeCharacter()
            };
            
            CharactersInPosition.Add(pos, character);
        }
        
        private void CreateStrongCharacter(Vector2Int pos)
        {
            var character = pos.x switch
            {
                0 => ScriptableCharactersList.GetRandomStrongMeleeCharacter(),
                2 => ScriptableCharactersList.GetRandomStrongDistanceCharacter(),
                _ => ScriptableCharactersList.GetRandomStrongDistanceCharacter()
            };
            
            CharactersInPosition.Add(pos, character);
        }

        private void SaveCharacters()
        {
            var compSavedTeam = Resources.Load<SavedTeam>("ScriptableObjects/ComputerTeam");
            compSavedTeam.SaveTeam(CharactersInPosition);
        }
        
        private Vector2Int GetEmptyPosition()
        {
            var pos = _allPosition[Random.Range(0, _allPosition.Count)];
            _allPosition.Remove(pos);
            return pos;
        }
    }
}