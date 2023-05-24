using System;
using CharactersScripts;

namespace EffectDealers
{
    public class DoubleEffectDealer : IEffectDealer
    {
        private IEffectDealer _firstEffectDealer;
        private IEffectDealer _secondEffectDealer;
        
        public DoubleEffectDealer(IEffectDealer firstEffectDealer, IEffectDealer secondEffectDealer)
        {
            _firstEffectDealer = firstEffectDealer;
            _secondEffectDealer = secondEffectDealer;
        }
        public void DealEffectTo(Character character, bool displayDamage = false)
        {
            if (character != null)
            {
                _firstEffectDealer.DealEffectTo(character);
                _secondEffectDealer.DealEffectTo(character);
            }
        }
    }
}