using BattleFieldScripts;
using DefaultNamespace;
using EffectDealers;
using MagicSkills.SomeSpellsTypes;
using SpellFormScripts;

namespace MagicSkills.WindBasedSpells
{
    public class WindVortex : SingleDamageSpellOnEnemy
    {
        public WindVortex()
        {
            damageDealer = new DamageDealer();
            damageDealer.AddMagicDamage(MagicDamage.Wind, 2);
            damageDealer.AddPhysicDamage(PhysicDamage.Bludgeoning, 1);
            form = new SingleTargetForm(damageDealer);
        }

        public override PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.AnyPositionWithCharacter;
        }
        public override string[] GetCombination()
        {
            return new[] { "Wind", "Sphere" };
        }
    }
}