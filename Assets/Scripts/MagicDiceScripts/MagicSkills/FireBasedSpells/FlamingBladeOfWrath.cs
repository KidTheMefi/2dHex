using BattleFieldScripts;
using DefaultNamespace;
using EffectDealers;
using MagicSkills.SomeSpellsTypes;
using SpellFormScripts;

namespace MagicSkills.FireBasedSpells
{
    public class FlamingBladeOfWrath : SingleDamageSpellOnEnemy
    {
        public FlamingBladeOfWrath()
        {
            damageDealer = new DamageDealer();
            damageDealer.AddMagicDamage(MagicDamage.Fire, 8);
            form = new ColumnOneEffect(damageDealer);
        }

        public override PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.FirstColumnWithCharacter;
        }
        public override string[] GetCombination()
        {
            return new[] { "Fire", "Fire", "Blade", "Blade" };
        }
    }
}