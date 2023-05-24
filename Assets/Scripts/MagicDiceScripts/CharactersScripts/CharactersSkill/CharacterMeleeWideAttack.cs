using System;
using System.Collections.Generic;
using BattleFieldScripts;
using Cysharp.Threading.Tasks;
using DamageDisplayScripts;
using EffectDealers;
using ScriptableScripts;
using UnityEngine;

namespace CharactersScripts.CharactersSkill
{
    public class CharacterMeleeWideAttack : ICharacterAttack
    {
        private DamageDealer _baseDamageDealer;
        private List<Character> _targets;

        public CharacterMeleeWideAttack(DamageDealer baseDamageDealer)
        {
            _baseDamageDealer = baseDamageDealer;
        }

        public void ShowAttackTarget(Vector2Int characterPosition, TeamField enemyField, DamageDealer additionalDamage)
        {
            _targets = FindTargets(characterPosition, enemyField);

            foreach (var target in _targets)
            {
                new DamageDealer(_baseDamageDealer, additionalDamage).DealEffectTo(target, true);
            }
        }

        public void HideAttackTarget()
        {
            foreach (var target in _targets)
            {
                target.ConfirmValuesChange(false);
                DamageDisplaySignals.GetInstance().InvokeHideDamage();
            }
        }

        public async UniTask ExecuteAttackAsync(Vector2Int characterPosition, TeamField enemyField, Action animationAction, DamageDealer additionalDamage)
        {
            await UniTask.Yield();
            _targets = FindTargets(characterPosition, enemyField);
            if (_targets.Count == 0)
            {
                return;
            }
            
            animationAction.Invoke();
            foreach (var target in _targets)
            {
                new DamageDealer(_baseDamageDealer, additionalDamage).DealEffectTo(target);
            }
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

            foreach (var target in _targets)
            { 
                target.ConfirmValuesChange(true);
            }
            
            _targets.Clear();
        }

        public bool CanAttack(Vector2Int characterPosition, TeamField allyField)
        {
            return characterPosition.x == 0;
        }
        
        public Sprite GetAttackIcon()
        {
            return CharacterAttackIcon.Instance.MelleWideAttackSprite;
        }
        public bool CanBeCharmedWithAdditionalDamage(DamageDealer additionalDamage)
        {
            return DamageDealer.CanBeCharmedWithAdditionalDamage(_baseDamageDealer, additionalDamage);
        }

        public string GetAttackDescription()
        {
            return $"Melee wide attack. Can attack only from first column. " +
                $"Attacks all characters in closest column with an enemy. Damage: \n{_baseDamageDealer.DamageDescription()}";
        }

        private List<Character> FindTargets(Vector2Int characterPosition, TeamField enemyField)
        {
            var field = enemyField;

            List<Character> targets = new List<Character>();
            
            var targetingPossibility = new TargetingPossibility(field);
            var possibleTargetsPosition = targetingPossibility.FirstColumnWithCharacters();
            
            for (int i = 0; i < possibleTargetsPosition.Count; i++)
            {
                if (field.PositionDictionary.TryGetValue(possibleTargetsPosition[i], out var targetPosition) && !targetPosition.IsEmpty())
                {
                    targets.Add(targetPosition.CharacterOnPosition);
                }
            }
            return targets;
        }
    }
}