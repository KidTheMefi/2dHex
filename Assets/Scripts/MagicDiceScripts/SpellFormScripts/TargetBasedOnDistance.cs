using System;
using BattleFieldScripts;
using DefaultNamespace;
using EffectDealers;
using UnityEngine;

namespace SpellFormScripts
{
    public class TargetBasedOnDistance : TargetingForm, ITargetingDamage
    {
        public event Action<Vector2Int, IEffectDealer> ShowTargetEffect = delegate(Vector2Int vector2, IEffectDealer dealer) { };
        private IEffectDealer _firstPositionEffect;
        private IEffectDealer _secondPositionEffect;
        private IEffectDealer _thirdPositionEffect;
        private bool _allLineEffect;
        
        public TargetBasedOnDistance(IEffectDealer firstPositionEffect, IEffectDealer secondPositionEffect, IEffectDealer thirdPositionEffect)
        {
            _firstPositionEffect = firstPositionEffect;
            _secondPositionEffect = secondPositionEffect;
            _thirdPositionEffect = thirdPositionEffect;
        }
        
        public TargetBasedOnDistance(DamageDealer firstDamageDealer,  DamageDealer onDistanceDamageDealer, bool allLineEffect)
        {
            _allLineEffect = allLineEffect;
            _firstPositionEffect = firstDamageDealer;
            var secondDamageDealer = new DamageDealer(firstDamageDealer, onDistanceDamageDealer);
            _secondPositionEffect = secondDamageDealer;
            _thirdPositionEffect = new DamageDealer(secondDamageDealer, onDistanceDamageDealer);
        }

        private void AllLineEffected(Vector2Int positionInFieldArray)
        {
            ShowTargetEffect?.Invoke(new Vector2Int(0, positionInFieldArray.y), _firstPositionEffect);
            ShowTargetEffect?.Invoke(new Vector2Int(1, positionInFieldArray.y), _secondPositionEffect);
            ShowTargetEffect?.Invoke(new Vector2Int(2, positionInFieldArray.y), _thirdPositionEffect);
        }

        private void OneTargetEffected(Vector2Int positionInFieldArray)
        {
            IEffectDealer currentDealer = null;
            switch (positionInFieldArray.x)
            {
                case 0:
                    currentDealer = _firstPositionEffect;
                    break;
                case 1:
                    currentDealer = _secondPositionEffect;
                    break;
                case 2:
                    currentDealer = _thirdPositionEffect;
                    break;
            }
            
            if (currentDealer != null)
            {
                ShowTargetEffect?.Invoke(positionInFieldArray, currentDealer);
            }
        }
        
        public override void ShowEffectedPosition(Vector2Int positionInFieldArray)
        {
            if (_allLineEffect)
            {
                AllLineEffected(positionInFieldArray);
            }
            else
            {
                OneTargetEffected(positionInFieldArray);
            }
        }
    }
}