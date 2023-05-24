using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ScriptableScripts
{
    public class ScriptableCharactersList
    {
        private static readonly CharacterScriptable[] _regularMeleeCharacters = Resources.LoadAll<CharacterScriptable>("ScriptableObjects/Characters/RegularMeleeCharacters");
        private static readonly CharacterScriptable[] _regularDistanceCharacters = Resources.LoadAll<CharacterScriptable>("ScriptableObjects/Characters/RegularDistanceCharacters");
        private static readonly CharacterScriptable[] _strongMeleeCharacters = Resources.LoadAll<CharacterScriptable>("ScriptableObjects/Characters/StrongMeleeCharacters");
        private static readonly CharacterScriptable[] _strongDistanceCharacters = Resources.LoadAll<CharacterScriptable>("ScriptableObjects/Characters/StrongDistanceCharacters");
        
        private static Dictionary<string, CharacterScriptable> _allCharacters = AllCharactersDictionary();

        private static Dictionary<string, CharacterScriptable> AllCharactersDictionary()
        {
            Dictionary<string, CharacterScriptable> dictionary = new Dictionary<string, CharacterScriptable>();

            foreach (var character in _regularMeleeCharacters)
            {
                dictionary.Add(character.CharacterName, character);
            }
            foreach (var character in _regularDistanceCharacters)
            {
                dictionary.Add(character.CharacterName, character);
            }
            foreach (var character in _strongMeleeCharacters)
            {
                dictionary.Add(character.CharacterName, character);
            }
            foreach (var character in _strongDistanceCharacters)
            {
                dictionary.Add(character.CharacterName, character);
            }
            return dictionary;
        }

        public static List<string> GetMeleeCharactersName()
        {
            List<string> result = new List<string>();

            foreach (var character in _regularMeleeCharacters)
            {
                result.Add(character.CharacterName);
            }
            foreach (var character in _strongMeleeCharacters)
            {
                result.Add(character.CharacterName);
            }
            return result;
        }
        
        
        
        public static List<string> GetDistanceCharactersName()
        {
            List<string> result = new List<string>();

            foreach (var character in _regularDistanceCharacters)
            {
                result.Add(character.CharacterName);
            }
            foreach (var character in _strongDistanceCharacters)
            {
                result.Add(character.CharacterName);
            }
            return result;
        }
        
        public static CharacterScriptable GetCharacterWithName(string name)
        {
            return _allCharacters[name];
        }
        
        public CharacterScriptable GetRandomCharacterFromAllList()
        {
            var rand = Random.Range(0, _allCharacters.Values.Count);
            return _allCharacters.Values.ToArray()[rand];
        }

        public static CharacterScriptable GetRandomRegularMeleeCharacter()
        {
            var rand = Random.Range(0, _regularMeleeCharacters.Length);
            return _regularMeleeCharacters[rand];
        }
        
        public static CharacterScriptable GetRandomStrongMeleeCharacter()
        {
            var rand = Random.Range(0, _strongMeleeCharacters.Length);
            return _strongMeleeCharacters[rand];
        }
        
        public static CharacterScriptable GetRandomRegularDistanceCharacter()
        {
            var rand = Random.Range(0, _regularDistanceCharacters.Length);
            return _regularDistanceCharacters[rand];
        }
        
        public static CharacterScriptable GetRandomStrongDistanceCharacter()
        {
            var rand = Random.Range(0, _strongDistanceCharacters.Length);
            return _strongDistanceCharacters[rand];
        }
    }
}