using BattleFieldScripts;
using DefaultNamespace;
using EffectDealers;
using MagicSkills.SomeSpellsTypes;
using SpellFormScripts;

namespace MagicSkills.EarthBasedSpells
{
    public class EarthArrow : SingleDamageSpellOnEnemy
    {
        public EarthArrow() 
        {
            damageDealer = new DamageDealer();
            damageDealer.AddMagicDamage(MagicDamage.Earth, 1);
            damageDealer.AddPhysicDamage(PhysicDamage.Piercing, 2);
            form = new SingleTargetForm(damageDealer);
        }

        public override string[] GetCombination()
        {
            return new[] { "Earth", "Arrow" };
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