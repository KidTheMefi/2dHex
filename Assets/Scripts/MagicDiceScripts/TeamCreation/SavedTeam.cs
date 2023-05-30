using System;
using System.Collections.Generic;
using System.IO;
using DefaultNamespace;
using ScriptableScripts;
using UnityEngine;

namespace TeamCreation
{
    [CreateAssetMenu(menuName = "Team/team", fileName = "Team")]
    public class SavedTeam : ScriptableObject
    {
        public Dictionary<Vector2Int, CharacterScriptable> CharactersInPosition { get; private set; }
        

        public void SaveTeam(Dictionary<Vector2Int, CharacterScriptable> team)
        {
            Debug.Log($"Saved team count: {team.Count} ");
            CharactersInPosition = team;
        }


        public bool SavePlayerTeamToJsonData()
        {
            if (File.Exists(Application.dataPath + "/Save/SavedData.json"))
            {
                var teamAsList = GetTeamAsList();
                string json = File.ReadAllText(Application.dataPath + "/Save/SavedData.json");
                SaveHandler.SaveData saveData = JsonUtility.FromJson<SaveHandler.SaveData>(json);
                saveData.PlayerGroupModel.PlayerSettings.SavedTeamList = teamAsList;
                json = JsonUtility.ToJson(saveData, true);
                File.WriteAllText(Application.dataPath + "/Save/SavedData.json", json);
            }
            return true;
        }

        public List<CharacterKeyValue> GetTeamAsList()
        {
            List<CharacterKeyValue> teamAsList = new List<CharacterKeyValue>();
            foreach (var character in CharactersInPosition)
            {
                teamAsList.Add(new CharacterKeyValue(character.Key, character.Value));
            }
            return teamAsList;
        }

        public void SetTeamFromList(List<CharacterKeyValue> teamAsList)
        {
            Dictionary<Vector2Int, CharacterScriptable> charactersInPosition = new Dictionary<Vector2Int, CharacterScriptable>();

            foreach (var character in teamAsList)
            {
                charactersInPosition.Add(character.Position, character.Character);
            }
            CharactersInPosition = charactersInPosition;
        }
        
        
        [Serializable]
        public class CharacterKeyValue
        {
            public Vector2Int Position;
            public CharacterScriptable Character;

            public CharacterKeyValue(Vector2Int position, CharacterScriptable characterScriptable)
            {
                Position = position;
                Character = characterScriptable;

            }
        }
    }
}