using System;
using BattleFieldScripts;
using Cysharp.Threading.Tasks;
using DamageDisplayScripts;
using DefaultNamespace;
using EffectDealers;
using ScriptableScripts;
using UnityEngine;

namespace CharactersScripts.CharactersSkill
{
    public class CharacterCloseAttack : ICharacterAttack
    {
        private DamageDealer _baseDamageDealer;
        private Character _target;
        
        public CharacterCloseAttack(DamageDealer baseDamageDealer)
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
            //_baseDamageDealer.DealEffectTo(_target);
            new DamageDealer(_baseDamageDealer, additionalDamage).DealEffectTo(_target);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            _target.ConfirmValuesChange(true);
            _target = null;
        }

        public bool CanAttack(Vector2Int characterPosition, TeamField allyField)
        {
            var field = allyField;
            if (field.PositionDictionary.TryGetValue(new Vector2Int(characterPosition.x - 1, characterPosition.y), out var oneFront) && !oneFront.IsEmpty())
            {
                return false;
            }

            return !field.PositionDictionary.TryGetValue(new Vector2Int(characterPosition.x - 2, characterPosition.y), out var secondFront) || secondFront.IsEmpty();

        }
        public Sprite GetAttackIcon()
        {
            return CharacterAttackIcon.Instance.CloseAttackSprite;
        }

        public string GetAttackDescription()
        {
            return $"Close attack. Can attack from any position if don't have other character in front. " +
                $"Attacks a character in closest column with an enemy. Damage: \n{_baseDamageDealer.DamageDescription()}";
        }

        public bool CanBeCharmedWithAdditionalDamage(DamageDealer additionalDamage)
        {
            return DamageDealer.CanBeCharmedWithAdditionalDamage(_baseDamageDealer, additionalDamage);
        }
        
        private Character FindTarget(Vector2Int characterPosition, TeamField enemyField)
        {
            var field = enemyField;
            var targetingPossibility = new TargetingPossibility(field);
            var possibleTargetsPosition = targetingPossibility.FirstColumnWithCharacters();

            BattleFieldPosition targetPosition;

            if (possibleTargetsPosition.Count == 1 && field.PositionDictionary.TryGetValue(possibleTargetsPosition[0], out targetPosition))
            {
                return targetPosition.CharacterOnPosition;
            }

            foreach (var pos in possibleTargetsPosition)
            {
                if (pos.y == characterPosition.y && field.PositionDictionary.TryGetValue(pos, out targetPosition))
                {
                    return targetPosition.CharacterOnPosition;
                }
            }

            foreach (var pos in possibleTargetsPosition)
            {
                if (pos.y == characterPosition.y + 1 && field.PositionDictionary.TryGetValue(pos, out targetPosition))
                {
                    return targetPosition.CharacterOnPosition;
                }

                if (pos.y == characterPosition.y - 1 && field.PositionDictionary.TryGetValue(pos, out targetPosition))
                {
                    return targetPosition.CharacterOnPosition;
                }
            }
            return null;
        }
    }
}