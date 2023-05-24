using System;
using BattleFieldScripts;
using EffectDealers;
using UnityEngine;

namespace SpellFormScripts
{
    public class SingleTargetForm : TargetingForm, ITargetingDamage
    {
        public event Action<Vector2Int, IEffectDealer> ShowTargetEffect;
        private IEffectDealer _effectDealer;
        

        public SingleTargetForm(IEffectDealer effectDealer)
        {
            _effectDealer = effectDealer;
            ShowTargetEffect = delegate(Vector2Int vector2, IEffectDealer dealer) { };
        }

        public override void ShowEffectedPosition(Vector2Int positionInFieldArray)
        {
            if (_effectDealer != null)
            {
                ShowTargetEffect?.Invoke(positionInFieldArray, _effectDealer);
            }
        }
    }
}