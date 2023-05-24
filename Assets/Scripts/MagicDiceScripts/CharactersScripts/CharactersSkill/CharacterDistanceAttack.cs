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
    public class CharacterDistanceAttack : ICharacterAttack
    {
        private DamageDealer _baseDamageDealer;
        private Character _target;

        public CharacterDistanceAttack(DamageDealer baseDamageDealer)
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
            return true;
        }

        public bool CanBeCharmedWithAdditionalDamage(DamageDealer additionalDamage)
        {
            return DamageDealer.CanBeCharmedWithAdditionalDamage(_baseDamageDealer, additionalDamage);
        }

        public Sprite GetAttackIcon()
        {
            return CharacterAttackIcon.Instance.DistanceAttackSprite;
        }
        public string GetAttackDescription()
        {
            return $"Distance attack. Can attack from any position. " +
                $"Attacks the furthest character in the same row. Damage: \n {_baseDamageDealer.DamageDescription()}";
        }

        private Character FindTarget(Vector2Int characterPosition, TeamField enemyField)
        {
            var field = enemyField;
            var targetingPossibility = new TargetingPossibility(field);
            var possibleTargetsPosition = targetingPossibility.EveryEnemyInLine(characterPosition.y);

            BattleFieldPosition targetPosition;

            if (possibleTargetsPosition.Count != 0 && field.PositionDictionary.TryGetValue(possibleTargetsPosition[possibleTargetsPosition.Count - 1], out targetPosition))
            {
                return targetPosition.CharacterOnPosition;
            }
            return null;
            
            possibleTargetsPosition = targetingPossibility.GetPossibleTargets(PossibleTarget.AnyPositionWithCharacter);
            if (possibleTargetsPosition.Count == 0)
            {
                return null;
            }

            if (_target != null && _target.gameObject.activeSelf)
            {
                return _target;
            }
            var randPosition = possibleTargetsPosition[Random.Range(0, possibleTargetsPosition.Count)];

            if (field.PositionDictionary.TryGetValue(randPosition, out targetPosition) && !targetPosition.IsEmpty())
            {
                return targetPosition.CharacterOnPosition;
            }
            return null;
        }
    }
}