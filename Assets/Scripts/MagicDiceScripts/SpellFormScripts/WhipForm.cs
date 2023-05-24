using System;
using BattleFieldScripts;
using EffectDealers;
using UnityEngine;

namespace SpellFormScripts
{
    public class WhipForm : TargetingForm, ITargetingDamage
    {
        private IEffectDealer _effectDealer;
        public event Action<Vector2Int, IEffectDealer> ShowTargetEffect;
        
        public WhipForm(IEffectDealer effectDealer)
        {
            ShowTargetEffect = delegate(Vector2Int i, IEffectDealer dealer) { };
            _effectDealer = effectDealer;
        }
        
        public override void ShowEffectedPosition(Vector2Int positionInFieldArray)
        {
            var centerPosition = positionInFieldArray;

            Vector2Int[] crossPositions =
            {
                centerPosition,
                centerPosition - Vector2Int.up,
                centerPosition + Vector2Int.up,
                centerPosition + Vector2Int.left,
                centerPosition + 2*Vector2Int.left,
            };

            foreach (var positionInArray in crossPositions)
            {
                ShowTargetEffect?.Invoke(positionInArray, _effectDealer);
            }
        }

    }
}