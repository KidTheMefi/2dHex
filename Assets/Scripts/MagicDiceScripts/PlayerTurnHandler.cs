using System;
using BattleFieldScripts;
using Cysharp.Threading.Tasks;
using MovementScript;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class PlayerTurnHandler : MonoBehaviour
    {
        [SerializeField]
        private ThrowerScript _throwerScript;
        [SerializeField]
        private SpellCombinationSelector _spellCombinationSelector;
        [SerializeField]
        private SpellExecutor _spellExecutor;
        [SerializeField]
        private BattleField _battleField;
        [SerializeField]
        private CharacterAttacksDisplayHandler _characterAttacksDisplayHandler;
        [SerializeField]
        private Button _endTurnButton;
        [SerializeField]
        private ChangePosition _changePosition;
        [SerializeField]
        private Button _bookButton;


        private void Start()
        {
            GameBegin();
            _endTurnButton.interactable = false;
        }

        private void GameBegin()
        {
            _battleField.EndGame += BattleFieldOnEndGame;
            _changePosition.MoveStateChange += ChangePositionOnMoveStateChange;
            _spellExecutor.SpellStateChange += SpellExecutorOnSpellStateChange;
            _spellCombinationSelector.SpellSelected += _spellExecutor.SetSpell;
            _throwerScript.ThrowerStateChanges += ThrowerScriptOnThrowerStateChanges;
            _endTurnButton.onClick.AddListener(() => EndTurnAsync().Forget());

            _battleField.FillBattleField();
            //_throwerScript.SetRollButtonInteractable(false);
            _endTurnButton.interactable = false;
            //_changePosition.EnableMoveButtons(true);
            SceneChanger.GetInstance().LoadScreenEnabled(false);
        }
        private void BattleFieldOnEndGame()
        {
            _battleField.EndGame -= BattleFieldOnEndGame;
            _changePosition.MoveStateChange -= ChangePositionOnMoveStateChange;
            _spellExecutor.SpellStateChange -= SpellExecutorOnSpellStateChange;
            _spellCombinationSelector.SpellSelected -= _spellExecutor.SetSpell;
            _throwerScript.ThrowerStateChanges -= ThrowerScriptOnThrowerStateChanges;
            
            DisableAllInteraction();
        }


        private void ChangePositionOnMoveStateChange(MoveState moveState)
        {
            switch (moveState)
            {
                case MoveState.BeginMove:
                    _spellCombinationSelector.CleanCombination(false);
                    _spellCombinationSelector.SubscribeToDiceClick(false);
                    break;
                case MoveState.CanceledMove:
                    WaitingForSpellOrMove(false);
                    break;
                case MoveState.SuccessMove:
                    WaitingForSpellOrMove(false);
                    break;
            }
        }

        private async UniTask EndTurnAsync()
        {
            //_throwerScript.ReturnAllDiceToHand();
            DisableAllInteraction();
            _throwerScript.ResetDicesState();
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            var continued = await _battleField.EndTurnAsync();

            if (continued)
            {
                EnableInteraction();
            }
            else
            {
                Debug.Log("Write smth here");
            }
        }

        private void DisableAllInteraction()
        {
            _bookButton.interactable = false;
            _changePosition.DisableMove();
            _characterAttacksDisplayHandler.SubscribeToCharactersSignals(false);
            _endTurnButton.interactable = false;
            _spellCombinationSelector.CleanCombination(false);
            _spellCombinationSelector.SubscribeToDiceClick(false);
            _throwerScript.SetRollButtonInteractable(false);
        }

        private void EnableInteraction()
        {
            _bookButton.interactable = true;
            _characterAttacksDisplayHandler.SubscribeToCharactersSignals(true);
            _throwerScript.SetRollButtonInteractable(true);
        }
        private void ThrowerScriptOnThrowerStateChanges(ThrowerState state)
        {
            switch (state)
            {
                case ThrowerState.DiceInMotion:
                    _changePosition.DisableMove();
                    _spellCombinationSelector.CleanCombination(false);
                    _spellCombinationSelector.SubscribeToDiceClick(false);
                    break;
                case ThrowerState.DiceStopped:
                    _changePosition.EnableMove(true);
                    WaitingForSpellOrMove(false);
                    break;
            }
        }

        private void SpellExecutorOnSpellStateChange(SpellState spellState)
        {
            switch (spellState)
            {
                case SpellState.SpellPlaying:
                    SpellPlaying();
                    _changePosition.DisableMove();
                    break;
                case SpellState.SpellPlayed:
                    WaitingForSpellOrMove(true);
                    break;
                case SpellState.SpellCanceled:
                    WaitingForSpellOrMove(false);
                    break;
            }

        }

        private void WaitingForSpellOrMove(bool cleanCombination)
        {
            _battleField.UpdateAttacks();
            _changePosition.EnableMove();
            _spellCombinationSelector.CleanCombination(cleanCombination);
            _spellCombinationSelector.SubscribeToDiceClick(true);
            _endTurnButton.interactable = true;
            _characterAttacksDisplayHandler.SubscribeToCharactersSignals(true);
            //_throwerScript.SetRollButtonInteractable(false);
        }

        private void SpellPlaying()
        {
            _spellCombinationSelector.SubscribeToDiceClick(false);
            _endTurnButton.interactable = false;
            _characterAttacksDisplayHandler.SubscribeToCharactersSignals(false);
        }

        private void OnDestroy()
        {
            _battleField.EndGame -= BattleFieldOnEndGame;
            _changePosition.MoveStateChange -= ChangePositionOnMoveStateChange;
            _spellExecutor.SpellStateChange -= SpellExecutorOnSpellStateChange;
            _spellCombinationSelector.SpellSelected -= _spellExecutor.SetSpell;
            _throwerScript.ThrowerStateChanges -= ThrowerScriptOnThrowerStateChanges;
        }
    }
}