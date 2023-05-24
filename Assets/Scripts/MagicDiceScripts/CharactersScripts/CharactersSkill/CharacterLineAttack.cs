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
    public class CharacterLineAttack : ICharacterAttack
    {
        private DamageDealer _baseDamageDealer;
        private List<Character> _targets;
        private bool _allInLine;

        public CharacterLineAttack(DamageDealer baseDamageDealer, bool allInLine)
        {
            _baseDamageDealer = baseDamageDealer;
            _allInLine = allInLine;
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
            if (_targets == null)
            {
                return;
            }
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
            var field = allyField;
            if (field.PositionDictionary.TryGetValue(new Vector2Int(characterPosition.x - 1, characterPosition.y), out var oneFront) && !oneFront.IsEmpty())
            {
                return false;
            }

            return !field.PositionDictionary.TryGetValue(new Vector2Int(characterPosition.x - 2, characterPosition.y), out var secondFront) || secondFront.IsEmpty();
        }

        public bool CanBeCharmedWithAdditionalDamage(DamageDealer additionalDamage)
        {
            return DamageDealer.CanBeCharmedWithAdditionalDamage(_baseDamageDealer, additionalDamage);
        }

        public Sprite GetAttackIcon()
        {
            return _allInLine ? CharacterAttackIcon.Instance.AllInLineAttackSprite : CharacterAttackIcon.Instance.FirstInLineAttackSprite;
        }

        public string GetAttackDescription()
        {
            string description = _allInLine ? "All line attack." : "First in row attack. ";
            description += "Can attack from any position if don't have other character in front. Damage:";
            description += $"\n{_baseDamageDealer.DamageDescription()}";
            return description;
        }

        private List<Character> FindTargets(Vector2Int characterPosition, TeamField enemyField)
        {
            var field = enemyField;

            List<Character> targets = new List<Character>();

            for (int i = 0; i < 3; i++)
            {
                if (field.PositionDictionary.TryGetValue(new Vector2Int(i, characterPosition.y), out var targetPosition) && !targetPosition.IsEmpty())
                {
                    targets.Add(targetPosition.CharacterOnPosition);
                    if (!_allInLine)
                    {
                        break;
                    }
                }
            }
            return targets;
        }
    }
}