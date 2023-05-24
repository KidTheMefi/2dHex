using BattleFieldScripts;
using DefaultNamespace;
using MagicSkills;
using ScriptableScripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TeamCreation
{
    public class PlayerTeamCreator : SpellExecutor
    {
        [SerializeField]
        private PlayerTeamSelector _playerTeamSelector;
        [SerializeField]
        private TeamField _teamField;
        [SerializeField]
        private SavedTeam _savedTeam;
        [SerializeField]
        private TextMeshProUGUI _characterValueText;
        private ProgressionHandler _progressionHandler;

        private CharacterScriptable _currentSelectedCharacterToCreate;

        private void Start()
        {
            _playerTeamSelector.CharacterSelected += PlayerTeamSelectorOnCharacterSelected;
            SpellStateChange += SpellExecutorOnSpellStateChange;
            _progressionHandler = ProgressionHandler.GetInstance();
            if (_savedTeam.CharactersInPosition != null)
            {
                _teamField.FillWithCharacters(_savedTeam.CharactersInPosition);
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
        }

        protected override MagicSpellHandler CreateMagicSpellHandler(IMagicSpell magicSpell)
        {
           return new MagicSpellHandler(magicSpell,_teamField);
        }
        private void OnDestroy()
        {
            _playerTeamSelector.CharacterSelected -= PlayerTeamSelectorOnCharacterSelected;
            SpellStateChange -= SpellExecutorOnSpellStateChange;
        }

        public void GoToFightScene()
        {
            _savedTeam.SaveTeam(_teamField.GetTeamScriptable());
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }
    }
}