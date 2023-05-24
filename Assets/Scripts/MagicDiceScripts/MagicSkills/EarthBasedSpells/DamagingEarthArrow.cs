using BattleFieldScripts;
using DefaultNamespace;
using EffectDealers;
using MagicSkills.SomeSpellsTypes;
using SpellFormScripts;

namespace MagicSkills.EarthBasedSpells
{
    public class DamagingEarthArrow : SingleDamageSpellOnEnemy
    {
        public DamagingEarthArrow() 
        {
            damageDealer = new DamageDealer();
            damageDealer.AddMagicDamage(MagicDamage.Earth, 3);
            damageDealer.AddPhysicDamage(PhysicDamage.Piercing, 3);
            form = new SingleTargetForm(damageDealer);
        }

        public override string[] GetCombination()
        {
            return new[] { "Earth", "Earth", "Arrow" };
        }
        public override TargetingForm GetTargetingForm()
        {
            return form;
        }
        public override PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.AnyPositionWithCharacter;
        }
    }
}