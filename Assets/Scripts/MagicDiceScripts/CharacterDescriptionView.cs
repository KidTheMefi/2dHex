using System;
using BattleFieldScripts;
using CharactersScripts;
using ScriptableScripts;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public static class NamesArray
    {
        private static readonly string[] _names = 
        {
            "Andrew", "Garold", "Alex", "Kendall", "Kinsley", "Madison", "Marley",
            "Presley", "Sutton", "Willow", "Winter", "Wren", "Archer", "Brooks",
            "Fletcher", "Graham", "Huxley", "Mason", "Reed", "Sawyer", "Wilder"
        };

        private static readonly string[] _lastNames = 
        {
            "Jones", "O'Kelly", "Johnson", "Williams", "O'Sullivan", "O'Sullivan", "Williams",
            "Brown", "Walsh", "Brown", "Taylor", "Smith", "Smith", "Murphy", "Smith",
            "Jones", "Davies", "O'Brien", "Miller", "Wilson", "Davis", "Evans", "Rodriguez",
            "Thomas", "Roberts", "O'Connor", "Byrne", "Garcia", "O'Neill"
        };

        public static string GetRandomName()
        {
            return $"{_names[Random.Range(0, _names.Length)]} {_lastNames[Random.Range(0, _lastNames.Length)]}";
        }
    }
    
    public class CharacterDescriptionView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro _characterDescribe;
        
        private void Start()
        {
            _characterDescribe.text = null;
            BattleFieldPositionSignal.GetInstance().MouseEnterCharacter += OnMouseEnterCharacter;
            BattleFieldPositionSignal.GetInstance().MouseExitCharacter += OnMouseExitCharacter;
        }
        private void OnMouseEnterCharacter(Character character)
        {
            //transform.position = character.transform.position;
            _characterDescribe.text = character.CharacterDescription();
        }

        private void OnMouseExitCharacter()
        {
            _characterDescribe.text = null;
        }

        private void OnDestroy()
        {
            BattleFieldPositionSignal.GetInstance().MouseEnterCharacter -= OnMouseEnterCharacter;
            BattleFieldPositionSignal.GetInstance().MouseExitCharacter -= OnMouseExitCharacter;
        }

    }
}