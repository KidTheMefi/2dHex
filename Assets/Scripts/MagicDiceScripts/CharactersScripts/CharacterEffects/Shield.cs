using System.Collections.Generic;
using EffectDealers;

namespace CharactersScripts
{
    public class Shield : ICharacterEffect
    {
        private string _shieldName;
        public Dictionary<MagicDamage, int> MagicDefence  { get; }
        public Dictionary<PhysicDamage, int> PhysicDefence { get; }
        
        
        public Shield(string shieldName, Dictionary<MagicDamage, int> magicDefence = null, Dictionary<PhysicDamage, int> physicDefence = null)
        {
            _shieldName = shieldName;
            MagicDefence = magicDefence ?? new Dictionary<MagicDamage, int>();
            PhysicDefence = physicDefence ?? new Dictionary<PhysicDamage, int>();
        }


        public string GetEffectName()
        {
            return _shieldName;
        }
        public string Description()
        {
            string shieldDescription = null;
            shieldDescription += $" {GetEffectName()}:\n";
            if (MagicDefence != null)
            {
                foreach (var magicDefence in MagicDefence)
                {
                    shieldDescription += $"{magicDefence.Key}: {magicDefence.Value} \n";
                }
            }

            if (PhysicDefence != null)
            {
                foreach (var physicDefence in PhysicDefence)
                {
                    shieldDescription += $"{physicDefence.Key}: {physicDefence.Value} \n";
                }
            }
            return shieldDescription;
        }
    }
}