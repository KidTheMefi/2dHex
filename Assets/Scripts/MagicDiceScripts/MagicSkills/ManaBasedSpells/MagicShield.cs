using System.Collections.Generic;
using BattleFieldScripts;
using CharactersScripts;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.ManaBasedSpells
{
    public class MagicShield : SinglePositiveSpellOnAlly
    {
        public MagicShield()
        {
            var additionalDefence = new PositiveEffectDealer(additionalMagicShield: 2);

            Shield shield = new Shield("Magic Shield", new Dictionary<MagicDamage, int>());
            
            shield.MagicDefence.Add(MagicDamage.Wind, 1);
            shield.MagicDefence.Add(MagicDamage.Water, 1);
            shield.MagicDefence.Add(MagicDamage.Fire, 1);
            shield.MagicDefence.Add(MagicDamage.Earth, 1);

            var additionalResist = new ShieldEffectDealer(shield);

            positiveDealer = new DoubleEffectDealer(additionalResist, additionalDefence);
            form = new SingleTargetForm(positiveDealer);
        }

        public override PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.AnyPositionWithCharacter;
        }
        public override string[] GetCombination()
        {
            return new[] { "Mana", "Mana", "Shield" };
        }
    }
}