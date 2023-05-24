using BattleFieldScripts;
using DefaultNamespace;
using EffectDealers;
using MagicSkills.SomeSpellsTypes;
using SpellFormScripts;

namespace MagicSkills.WaterBasedSpells
{
    public class WaterBlade : SingleDamageSpellOnEnemy
    {
        public WaterBlade()
        {
            damageDealer = new DamageDealer();
            damageDealer.AddMagicDamage(MagicDamage.Water, 2);
            damageDealer.AddPhysicDamage(PhysicDamage.Slashing, 2);
            form = new SingleTargetForm(damageDealer);
        }

        public override PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.FirstColumnWithCharacter;
        }
        public override string[] GetCombination()
        {
            return new[] { "Water", "Blade" };
        }
    }
}