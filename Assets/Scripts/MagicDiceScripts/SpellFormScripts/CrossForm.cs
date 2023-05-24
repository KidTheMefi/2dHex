using System;
using BattleFieldScripts;
using DefaultNamespace;
using EffectDealers;
using UnityEngine;

namespace SpellFormScripts
{
    public class CrossForm : TargetingForm, ITargetingDamage
    {
        public event Action<Vector2Int, IEffectDealer> ShowTargetEffect;
        private IEffectDealer _effectDealer;

        public CrossForm(IEffectDealer effectDealer)
        {
            ShowTargetEffect = delegate(Vector2Int vector2, IEffectDealer dealer) { };
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
                centerPosition - Vector2Int.left,
            };

            foreach (var positionInArray in crossPositions)
            {
                ShowTargetEffect?.Invoke(positionInArray, _effectDealer);
            }
        }
    }
}