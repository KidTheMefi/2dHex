using CharactersScripts;
using UnityEngine;

namespace EffectDealers
{
    public class PositiveEffectDealer : IEffectDealer
    {
        private readonly int _hpHeal;
        private readonly int _additionalArmor;
        private readonly int _additionalMagicShield;

        public PositiveEffectDealer(int hpHeal = 0, int additionalArmor = 0, int additionalMagicShield = 0)
        {
            _hpHeal = hpHeal;
            _additionalArmor = additionalArmor;
            _additionalMagicShield = additionalMagicShield;
        }
        

        public void DealEffectTo(Character character, bool show)
        {
            if (_hpHeal > 0)
            {
                character.PositiveEffectReceiver.Heal(_hpHeal);
            }

            if (_additionalArmor > 0)
            {
                character.PositiveEffectReceiver.AddArmor(_additionalArmor);
            }
            if (_additionalMagicShield > 0)
            {
                character.PositiveEffectReceiver.AddMagicShield(_additionalMagicShield);
            }
        }
    }
}