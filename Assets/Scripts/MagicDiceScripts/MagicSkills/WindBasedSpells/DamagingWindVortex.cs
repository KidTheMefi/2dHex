using BattleFieldScripts;
using DefaultNamespace;
using EffectDealers;
using MagicSkills.SomeSpellsTypes;
using SpellFormScripts;

namespace MagicSkills.WindBasedSpells
{
    public class DamagingWindVortex : SingleDamageSpellOnEnemy
    {
        public DamagingWindVortex()
        {
            damageDealer = new DamageDealer();
            damageDealer.AddMagicDamage(MagicDamage.Wind, 4);
            damageDealer.AddPhysicDamage(PhysicDamage.Bludgeoning, 2);
            form = new SingleTargetForm(damageDealer);
        }

        public override PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.AnyPositionWithCharacter;
        }
        public override string[] GetCombination()
        {
            return new[] { "Wind", "Wind", "Sphere" };
        }

    }
}