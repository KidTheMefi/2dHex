using System;
using System.Collections.Generic;
using BattleFieldScripts;
using Cysharp.Threading.Tasks;
using EffectDealers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CharactersScripts.CharactersSkill
{
    public class CharacterAttackHandler : MonoBehaviour
    {
        private List<ICharacterAttack> _possibleAttacks = new List<ICharacterAttack>();
        [SerializeField]
        private CharacterBody _characterBody;
        [SerializeField]
        private SpriteRenderer _onAttackSprite;
        private ICharacterAttack _currentAttack;
        private Vector2Int _characterPosition;
        private TeamField _allyField;
        private DamageDealer _additionalDamage = new DamageDealer();
        private DamageDealer _additionalDamageTemp = new DamageDealer();

        public string Description()
        {
            string description = "PossibleAttack: \n";

            if (_possibleAttacks.Count == 0)
            {
                return "";
            }
            foreach (var attack in _possibleAttacks)
            {
                description += $" {attack.GetAttackDescription()}";
            }

            var additionalDamage = _additionalDamageTemp?.DamageDescription();
            if (additionalDamage != null)
            {
                description += $"Additional Damage: \n{additionalDamage}";
            }

            return description;
        }

        public bool CanBeCharmedWithAdditionalDamage(DamageDealer damageDealer)
        {
            if (_currentAttack == null)
            {
                return false;
            }
            return _currentAttack.CanBeCharmedWithAdditionalDamage(damageDealer) ;
        }
        
        public void AddAdditionalDamage(DamageDealer damageDealer)
        {
            _additionalDamageTemp = new DamageDealer(_additionalDamage, damageDealer);
        }

        public void ConfirmAdditionalDamage(bool value)
        {
            if (value)
            {
                _additionalDamage = _additionalDamageTemp;
            }
            else
            {
                _additionalDamageTemp = _additionalDamage;
            }
        }
        private void RemoveAdditionalDamage()
        {
            _additionalDamage?.RemoveAllDamage();
            _additionalDamageTemp?.RemoveAllDamage();
        }

        public void SetAttack(ICharacterAttack iCharacterAttack)
        {
            if (iCharacterAttack == null)
            {
                return;
            }
            _possibleAttacks = new List<ICharacterAttack> { iCharacterAttack };
        }

        public void SetPosition(Vector2Int pos, TeamField allyField)
        {
            _characterPosition = pos;
            _allyField = allyField;
            SelectAttack();
        }

        public async UniTask AttackAsync()
        {
            /*await UniTask.Yield();
            if (_possibleAttacks.Count == 0)
            {
                return;
            }
            var attack = _possibleAttacks[Random.Range(0, _possibleAttacks.Count)];
            if (attack == null || !attack.CanAttack(_characterPosition))
            {
                return;
            }*/
            if (_currentAttack == null)
            {
                return;
            }
            _onAttackSprite.color = Color.red;
            await _currentAttack.ExecuteAttackAsync(_characterPosition, BattleField.Instance.GetEnemyFieldFor(_allyField), _characterBody.Attack, _additionalDamage);
            RemoveAdditionalDamage();
            _onAttackSprite.color = Color.white;
            _onAttackSprite.enabled = false;
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
        }

        public void SelectAttack()
        {
            _currentAttack = null;
            if (_onAttackSprite == null)
            {
                return;
            }
            _onAttackSprite.enabled = false;
            
                    
            if (_possibleAttacks.Count == 0)
            {
                return;
            }
            var attack = _possibleAttacks[Random.Range(0, _possibleAttacks.Count)];
            if (attack == null || !attack.CanAttack(_characterPosition, _allyField))
            {
                return;
            }
            _currentAttack = attack;
            _onAttackSprite.sprite = attack.GetAttackIcon();
            _onAttackSprite.enabled = true;
        }

        public void ShowAttack()
        {
            _currentAttack?.ShowAttackTarget(_characterPosition, BattleField.Instance.GetEnemyFieldFor(_allyField), _additionalDamage);
        }

        public void HideAttack()
        {
            _currentAttack?.HideAttackTarget();
        }
    }
}