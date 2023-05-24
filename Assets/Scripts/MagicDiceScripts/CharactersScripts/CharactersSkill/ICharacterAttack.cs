using System;
using BattleFieldScripts;
using Cysharp.Threading.Tasks;
using EffectDealers;
using UnityEngine;

namespace CharactersScripts.CharactersSkill
{
    public interface ICharacterAttack
    {
        public void ShowAttackTarget(Vector2Int characterPosition, TeamField enemyField, DamageDealer additionalDamage);
        public void HideAttackTarget();
        public UniTask ExecuteAttackAsync(Vector2Int characterPosition, TeamField enemyField, Action animationAction, DamageDealer additionalDamage);
        public bool CanAttack(Vector2Int characterPosition, TeamField allyField);
        public Sprite GetAttackIcon();
        public bool CanBeCharmedWithAdditionalDamage(DamageDealer additionalDamage);
        public string GetAttackDescription();
    }
}