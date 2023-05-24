using System;
using BattleFieldScripts;
using CharactersScripts;
using UnityEngine;

namespace DefaultNamespace
{
    public class CharacterAttacksDisplayHandler :  MonoBehaviour
    {
        private bool _isSubscribed;
        private Character _character;

        private void Start()
        {
            SubscribeToCharactersSignals(true);
        }

        public void SubscribeToCharactersSignals(bool value)
        {
            if (_isSubscribed == value)
            {
                return;
            }

            if (value)
            {
                BattleFieldPositionSignal.GetInstance().MouseEnterCharacter += OnMouseEnterCharacter;
                BattleFieldPositionSignal.GetInstance().MouseExitCharacter += OnMouseExitCharacter;
            }
            else
            {
                BattleFieldPositionSignal.GetInstance().MouseEnterCharacter -= OnMouseEnterCharacter;
                BattleFieldPositionSignal.GetInstance().MouseExitCharacter -= OnMouseExitCharacter;
            }
            _isSubscribed = value;
        }
        
        
        private void OnMouseExitCharacter()
        {
            if (_character!= null)
            {
                _character.CharacterAttackHandler.HideAttack();
            }
           
        }
        
        private void OnMouseEnterCharacter(Character character)
        {
            character.CharacterAttackHandler.ShowAttack();
            _character = character;
        }

        private void OnDestroy()
        {
            BattleFieldPositionSignal.GetInstance().MouseEnterCharacter -= OnMouseEnterCharacter;
            BattleFieldPositionSignal.GetInstance().MouseExitCharacter -= OnMouseExitCharacter;
        }
    }
}