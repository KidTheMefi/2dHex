using CharactersScripts;
using UnityEngine;

namespace EffectDealers
{
    public class ShieldEffectDealer : IEffectDealer
    {
        private Shield _shield;
        
        public ShieldEffectDealer(Shield shield)
        {
            _shield = shield;
        }

        public void DealEffectTo(Character character, bool displayDamage = false)
        {
            character.CharacterEffectHandler.AddCharacterEffect(_shield);
        }
    }
}