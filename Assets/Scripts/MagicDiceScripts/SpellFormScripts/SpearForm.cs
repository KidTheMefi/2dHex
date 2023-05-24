using System;
using BattleFieldScripts;
using EffectDealers;
using UnityEngine;

namespace SpellFormScripts
{
    public class SpearForm : TargetingForm, ITargetingDamage
    {
        public event Action<Vector2Int, IEffectDealer> ShowTargetEffect;
        private IEffectDealer _effectDealer;
        
        public SpearForm(IEffectDealer effectDealer)
        {
            _effectDealer = effectDealer;
            ShowTargetEffect = delegate(Vector2Int i, IEffectDealer dealer) { };
        }
        public override void ShowEffectedPosition(Vector2Int positionInFieldArray)
        {
            ShowTargetEffect?.Invoke(positionInFieldArray, _effectDealer);
            ShowTargetEffect?.Invoke(positionInFieldArray + Vector2Int.right, _effectDealer);
        }
    }
}