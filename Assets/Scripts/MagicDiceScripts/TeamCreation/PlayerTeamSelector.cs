using System;
using System.Collections.Generic;
using BattleFieldScripts;
using CharactersScripts;
using ScriptableScripts;
using TMPro;
using UnityEngine;

namespace TeamCreation
{
    public class PlayerTeamSelector : MonoBehaviour
    {
        public event Action<CharacterScriptable> CharacterSelected = delegate(CharacterScriptable scriptable) { };
        //[SerializeField]
       // private CharacterFactoryWithPool _characterFactoryWithPool;
       [SerializeField]
       private TMP_Dropdown _characterTypeDropdown;
       [SerializeField]
       private TMP_Dropdown _characterDropdown;
       [SerializeField]
       private Character _character;
       [SerializeField]
       private BattleFieldPosition _battleFieldPosition;
       [SerializeField]
       private TextMeshProUGUI _descriptionText;
       private CharacterScriptable _selectedCharacterScriptable;
       public CharacterScriptable SelectedCharacterScriptable => _selectedCharacterScriptable;
       
        
        private void Start()
        {
            _battleFieldPosition.AddCharacter(_character);
            _characterDropdown.onValueChanged.AddListener(CharacterListDropDownSelect);
            CharacterTypeDropDownSetup();
        }

        public void SetInteractable(bool value)
        {
            _characterTypeDropdown.interactable = value;
            _characterDropdown.interactable = value;
        }

        private void CharacterTypeDropDownSetup()
        {
            _characterTypeDropdown.ClearOptions();
            _characterTypeDropdown.options.Add(new TMP_Dropdown.OptionData("Close"));
            _characterTypeDropdown.options.Add(new TMP_Dropdown.OptionData("Distance"));
            _characterTypeDropdown.onValueChanged.AddListener(CharacterTypeDropDownSelected);
            CharacterTypeDropDownSelected(0);
            _characterTypeDropdown.RefreshShownValue();
        }

        private void CharacterTypeDropDownSelected(int index)
        {
            switch (index)
            {
                case 0:
                    CharacterListDropDownSetup(ScriptableCharactersList.GetMeleeCharactersName());
                    return;
                case 1:
                    CharacterListDropDownSetup(ScriptableCharactersList.GetDistanceCharactersName());
                    return;
            }
        }

        private void CharacterListDropDownSetup(List<string> variants)
        {
            _characterDropdown.ClearOptions();
            _characterDropdown.AddOptions(variants);
            CharacterListDropDownSelect(0);
        }
        
        private void CharacterListDropDownSelect(int index)
        {
            _selectedCharacterScriptable = ScriptableCharactersList.GetCharacterWithName(_characterDropdown.options[index].text);
            _character.Setup(_selectedCharacterScriptable);
            _descriptionText.text = _character.CharacterDescription();
            CharacterSelected.Invoke(_selectedCharacterScriptable);
        }
    }
}