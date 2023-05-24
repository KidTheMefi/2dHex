using System;
using BattleFieldScripts;
using Cysharp.Threading.Tasks;
using DamageDisplayScripts;
using EffectDealers;
using ScriptableScripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CharactersScripts.CharactersSkill
{
    public class CharacterDistancePointAttack : ICharacterAttack
    {
        private DamageDealer _baseDamageDealer;
        private Character _target;

        public CharacterDistancePointAttack(DamageDealer baseDamageDealer)
        {
            _baseDamageDealer = baseDamageDealer;
        }

        public void ShowAttackTarget(Vector2Int characterPosition, TeamField enemyField, DamageDealer additionalDamage)
        {
            _target = FindTarget(characterPosition, enemyField);
            new DamageDealer(_baseDamageDealer, additionalDamage).DealEffectTo(_target, true);

            //_baseDamageDealer.DealEffectTo(_target, true);
        }

        public void HideAttackTarget()
        {
            if (_target != null)
            {
                _target.ConfirmValuesChange(false);
                DamageDisplaySignals.GetInstance().InvokeHideDamage();
            }
        }


        public async UniTask ExecuteAttackAsync(Vector2Int characterPosition, TeamField enemyField, Action animationAction, DamageDealer additionalDamage)
        {
            await UniTask.Yield();
            _target = FindTarget(characterPosition, enemyField);
            if (_target == null)
            {
                return;
            }
            animationAction.Invoke();
            new DamageDealer(_baseDamageDealer, additionalDamage).DealEffectTo(_target);

            //_baseDamageDealer.DealEffectTo(_target);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            _target.ConfirmValuesChange(true);
            _target = null;
        }

        public bool CanAttack(Vector2Int characterPosition, TeamField allyField)
        {
            if (allyField == null || BattleField.Instance == null)
            {
                return true;
            }
            return (FindTarget(characterPosition, BattleField.Instance.GetEnemyFieldFor(allyField)) != null);

        }

        public bool CanBeCharmedWithAdditionalDamage(DamageDealer additionalDamage)
        {
            return DamageDealer.CanBeCharmedWithAdditionalDamage(_baseDamageDealer, additionalDamage);
        }
        
        public Sprite GetAttackIcon()
        {
            return CharacterAttackIcon.Instance.DistanceOnPointAttackSprite;
        }
        public string GetAttackDescription()
        {
            return "Distance point attack. Can attack from any position. " +
                "Attacks the character who is placed three position further away in the same row. " +
                $"\n {_baseDamageDealer.DamageDescription()}";
        }

        private Character FindTarget(Vector2Int characterPosition, TeamField enemyField)
        {
            if (enemyField == null)
            {
                return null;
            }
            
            var field = enemyField;

            int xTargetPos = characterPosition.x switch
            {
                0 => 2,
                1 => 1,
                2 => 0,
                _ => 0
            };

            Vector2Int targetVector = new Vector2Int(xTargetPos,characterPosition.y);
            
            if (field.PositionDictionary.TryGetValue(targetVector, out var targetPosition))
            {
                return targetPosition.CharacterOnPosition;
            }

            return null;
        }
    }
}