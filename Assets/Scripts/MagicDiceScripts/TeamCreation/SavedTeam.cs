using System.Collections.Generic;
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
            CharactersInPosition = team;
        }
    }
}