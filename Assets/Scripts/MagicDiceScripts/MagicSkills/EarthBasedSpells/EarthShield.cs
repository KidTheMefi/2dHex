using System.Collections.Generic;
using BattleFieldScripts;
using CharactersScripts;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.EarthBasedSpells
{
    public class EarthShield : SinglePositiveSpellOnAlly
    {
        public EarthShield()
        {
            
            var additionalDefence = new PositiveEffectDealer(additionalArmor:3, additionalMagicShield:1);

            Shield shield = new Shield("Earth Shield",new Dictionary<MagicDamage, int>(), new Dictionary<PhysicDamage, int>());
            
            shield.MagicDefence.Add(MagicDamage.Wind, 1);
            shield.PhysicDefence.Add(PhysicDamage.Slashing, 1);
            shield.PhysicDefence.Add(PhysicDamage.Bludgeoning, 1);

            var additionalResist = new ShieldEffectDealer(shield);
            
            positiveDealer = new DoubleEffectDealer(additionalResist, additionalDefence);
            form = new SingleTargetForm(positiveDealer);
        }

        public override TargetingForm GetTargetingForm()
        {
            return form;
        }
        public override PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.AnyPositionWithCharacter;
        }
        public override string[] GetCombination()
        {
            return new []{"Earth", "Shield"};
        }
    }
}