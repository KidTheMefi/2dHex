using System;
using System.Collections.Generic;
using BattleFieldScripts;
using DefaultNamespace;
using EffectDealers;
using UnityEngine;

namespace SpellFormScripts
{
    public class TripleForm: TargetingForm, ITargetingDamage
    {
        public event Action<Vector2Int, IEffectDealer> ShowTargetEffect;
        private IEffectDealer _effectDealer;

        public TripleForm(IEffectDealer effectDealer)
        {
            ShowTargetEffect = delegate(Vector2Int vector2, IEffectDealer dealer) { };
            _effectDealer = effectDealer;
        }
        
        public override void ShowEffectedPosition(Vector2Int positionInFieldArray)
        {
            var centerPosition = positionInFieldArray;
            
            List<Vector2Int> effectedPositions = new List<Vector2Int>();
            effectedPositions.Add(centerPosition);
            effectedPositions.Add(centerPosition + Vector2Int.right + Vector2Int.up);
            effectedPositions.Add(centerPosition + Vector2Int.right - Vector2Int.up);
            if (positionInFieldArray.y != 1)
            {
                effectedPositions.Add(centerPosition + Vector2Int.right);
            }

            foreach (var positionInArray in effectedPositions)
            {
                ShowTargetEffect?.Invoke(positionInArray, _effectDealer);
            }
        }
    }
}