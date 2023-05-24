using BattleFieldScripts;
using DefaultNamespace;
using EffectDealers;
using MagicSkills.SomeSpellsTypes;
using SpellFormScripts;

namespace MagicSkills.FireBasedSpells
{
    public class WideFireBlade : SingleDamageSpellOnEnemy
    {
        public WideFireBlade()
        {
            damageDealer = new DamageDealer();
            damageDealer.AddMagicDamage(MagicDamage.Fire, 4);
            form = new ColumnOneEffect(damageDealer);
        }

        public override PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.FirstColumnWithCharacter;
        }
        public override string[] GetCombination()
        {
            return new[] { "Fire", "Blade", "Blade" };
        }
    }
}