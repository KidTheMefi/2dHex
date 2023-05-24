using BattleFieldScripts;
using EffectDealers;
using MagicSkills.SomeSpellsTypes;
using SpellFormScripts;

namespace MagicSkills.WindBasedSpells
{
    public class PiercingWindVortex: SingleDamageSpellOnEnemy
    {
        public PiercingWindVortex()
        {
            damageDealer = new DamageDealer();
            damageDealer.AddMagicDamage(MagicDamage.Wind, 2);
            damageDealer.AddPhysicDamage(PhysicDamage.Piercing, 2);
            form = new CrossForm(damageDealer);
        }

        public override PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.AnyPositionWithCharacter;
        }
        public override string[] GetCombination()
        {
            return new[] { "Wind", "Sphere", "Arrow" };
        }
    }
}