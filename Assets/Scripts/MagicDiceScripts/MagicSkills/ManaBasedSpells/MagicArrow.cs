using BattleFieldScripts;
using DefaultNamespace;
using EffectDealers;
using MagicSkills.SomeSpellsTypes;
using SpellFormScripts;

namespace MagicSkills.ManaBasedSpells
{
    public class MagicArrow : SingleDamageSpellOnEnemy
    {
        public MagicArrow()
        {
            damageDealer = new DamageDealer();
            damageDealer.AddMagicDamage(MagicDamage.Fire, 1);
            damageDealer.AddMagicDamage(MagicDamage.Water, 1);
            damageDealer.AddMagicDamage(MagicDamage.Earth, 1);
            damageDealer.AddMagicDamage(MagicDamage.Wind, 1);
            form = new SingleTargetForm(damageDealer);
        }

        public override string[] GetCombination()
        {
            return new[] { "Mana", "Arrow" };
        }

        public override PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.AnyPositionWithCharacter;
        }
    }
}