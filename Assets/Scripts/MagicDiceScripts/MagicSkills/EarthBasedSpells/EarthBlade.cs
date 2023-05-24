using BattleFieldScripts;
using DefaultNamespace;
using EffectDealers;
using MagicSkills.SomeSpellsTypes;
using SpellFormScripts;

namespace MagicSkills.EarthBasedSpells
{
    public class EarthBlade : SingleDamageSpellOnEnemy
    {
        public EarthBlade()
        {
            damageDealer = new DamageDealer();
            damageDealer.AddMagicDamage(MagicDamage.Earth, 1);
            damageDealer.AddPhysicDamage(PhysicDamage.Slashing, 3);
            form = new SingleTargetForm(damageDealer);
        }

        public override TargetingForm GetTargetingForm()
        {
            return form;
        }
        public override PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.FirstColumnWithCharacter;
        }

        public override string[] GetCombination()
        {
            return new[] { "Earth", "Blade" };
        }
    }
}