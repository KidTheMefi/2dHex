using System;
using System.Collections.Generic;
using System.Linq;
using BattleFieldScripts;
using BuildingScripts;
using BuildingScripts.RecruitingBuildings;
using CharactersScripts;
using DefaultNamespace;
using ScriptableScripts;
using TMPro;
using UnityEngine;

namespace TeamCreation
{
    public class PlayerTeamSelector : MonoBehaviour
    {
        public event Action<CharacterScriptable> CharacterSelected = delegate(CharacterScriptable scriptable) { };
       
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

       private RecruitingCenterProperty _recruitingCenterProperty;
       
        
        private void Start()
        {
            _battleFieldPosition.AddCharacter(_character);
            
        }


        public void SetupSelector(RecruitingCenterProperty recruitingCenterProperty)
        {
            _recruitingCenterProperty = recruitingCenterProperty;
            
            if (_recruitingCenterProperty  == null)
            {
                _characterDropdown.onValueChanged.AddListener(AllCharacterListDropDownSelect);
                AllCharacterTypeDropDownSetup();
            }
            else
            {
                CharacterListDropDownSetup();
                _characterTypeDropdown.enabled = false;
            }
        }

        public void SetInteractable(bool value)
        {
            _characterTypeDropdown.interactable = value;
            _characterDropdown.interactable = value;
        }

        private void AllCharacterTypeDropDownSetup()
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
            AllCharacterListDropDownSelect(0);
        }
        
        private void AllCharacterListDropDownSelect(int index)
        {
            var selectedCharacterScriptable = ScriptableCharactersList.GetCharacterWithName(_characterDropdown.options[index].text);
            _character.Setup(selectedCharacterScriptable);
            _descriptionText.text = _character.CharacterDescription();
            if (selectedCharacterScriptable != null)
            {
                CharacterSelected.Invoke(selectedCharacterScriptable);
            }
        }

        private void CharacterListDropDownSetup()
        {
            List<string> options = _recruitingCenterProperty.Recruits.Select(recruit => recruit.CharacterName).ToList();
            _characterDropdown.ClearOptions();
            _characterDropdown.AddOptions(options);
            _characterDropdown.RefreshShownValue();
            _characterDropdown.onValueChanged.AddListener(CharacterFromRecruitCenterSelect);
            CharacterFromRecruitCenterSelect(0);
        }
        
        private void CharacterFromRecruitCenterSelect(int index)
        {
            var selectedCharacterScriptable = _recruitingCenterProperty.Recruits[index];
            _character.Setup(selectedCharacterScriptable);
            _descriptionText.text = _character.CharacterDescription();
            if (selectedCharacterScriptable != null)
            {
                CharacterSelected.Invoke(selectedCharacterScriptable);
            }
            
        }
    }
}