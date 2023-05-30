using System.IO;
using BattleFieldScripts;
using BuildingScripts;
using DefaultNamespace;
using MagicSkills;
using ScriptableScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TeamCreation
{
    public class PlayerTeamCreator : SpellExecutor
    {
        [SerializeField] private PlayerTeamSelector _playerTeamSelector;
        [SerializeField] private TeamField _teamField;
        [SerializeField] private SavedTeam _savedTeam;
        [SerializeField] private TextMeshProUGUI _characterValueText;
        [SerializeField] private Button _nextSceneButton;
        [SerializeField] private SceneScriptableData _sceneScriptableData;

        private ProgressionHandler _progressionHandler;
        private RecruitingCenterProperty _recruitingCenterProperty;
        private CharacterScriptable _currentSelectedCharacterToCreate;




        private void Start()
        {
            _progressionHandler = ProgressionHandler.GetInstance();
            _playerTeamSelector.CharacterSelected += PlayerTeamSelectorOnCharacterSelected;
            _recruitingCenterProperty = _sceneScriptableData.GetRecruitingCenterProperty();
            _playerTeamSelector.SetupSelector(_recruitingCenterProperty);
            SpellStateChange += SpellExecutorOnSpellStateChange;
            
            if (_savedTeam.CharactersInPosition != null)
            {
                _teamField.FillWithCharacters(_savedTeam.CharactersInPosition);
            }
            _nextSceneButton.interactable = _teamField.GetTeamScriptable().Count > 0;

            _nextSceneButton.onClick.AddListener(ToNextScene);
            
            if (SceneChanger.GetInstance() != null)
            {
                SceneChanger.GetInstance().LoadScreenEnabled(false);
            }


        }
        private void PlayerTeamSelectorOnCharacterSelected(CharacterScriptable characterScriptable)
        {

            _currentSelectedCharacterToCreate = characterScriptable;
            _characterValueText.text = "Hire cost: " + _currentSelectedCharacterToCreate.HireCost;
            SetSpell(new CreateCharacterSpell(characterScriptable));
            UpdateCreationButton();
        }

        private void UpdateCreationButton()
        {
            bool canCreate = _currentSelectedCharacterToCreate.HireCost <= _progressionHandler.PlayerMoneyValue;
            _characterValueText.color = canCreate ? Color.green : Color.red;
            _characterValueText.text = "Hire cost: " + _currentSelectedCharacterToCreate.HireCost;
            _characterValueText.text += canCreate ? " available." : " can't be afford.";
            _executeButton.interactable = canCreate;
        }

        private void SpellExecutorOnSpellStateChange(SpellState spellState)
        {
            switch (spellState)
            {
                case SpellState.SpellPlaying:
                    _playerTeamSelector.SetInteractable(false);
                    break;
                case SpellState.SpellPlayed:
                    _progressionHandler.ChangeMoneyValueBy(-_currentSelectedCharacterToCreate.HireCost);
                    goto case SpellState.SpellCanceled;
                case SpellState.SpellCanceled:
                    SetSpell(new CreateCharacterSpell(_currentSelectedCharacterToCreate));
                    _playerTeamSelector.SetInteractable(true);
                    UpdateCreationButton();
                    break;
            }
            _nextSceneButton.interactable = _teamField.GetTeamScriptable().Count > 0;
        }

        private void ToNextScene()
        {
            _nextSceneButton.interactable = false;


            var saved = SaveTeam();
            
            if (_recruitingCenterProperty == null)
            {
                SceneChanger.GetInstance().CreateNewMap();
            }
            else
            {
                SceneChanger.GetInstance().LoadGame();
            }
        }

        private bool SaveTeam()
        {
            _savedTeam.SaveTeam(_teamField.GetTeamScriptable());
            return _savedTeam.SavePlayerTeamToJsonData();
        }

        protected override MagicSpellHandler CreateMagicSpellHandler(IMagicSpell magicSpell)
        {
            return new MagicSpellHandler(magicSpell, _teamField);
        }
        private void OnDestroy()
        {
            _playerTeamSelector.CharacterSelected -= PlayerTeamSelectorOnCharacterSelected;
            SpellStateChange -= SpellExecutorOnSpellStateChange;
        }

    }
}