using BattleFieldScripts;
using DefaultNamespace;
using EffectDealers;
using MagicSkills.SomeSpellsTypes;
using SpellFormScripts;

namespace MagicSkills.FireBasedSpells
{
    public class DamagingFireBlade: SingleDamageSpellOnEnemy
    {
        public DamagingFireBlade()
        {
            damageDealer = new DamageDealer();
            damageDealer.AddMagicDamage(MagicDamage.Fire, 7);
            form = new SingleTargetForm(damageDealer);
        }

        public override PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.FirstColumnWithCharacter;
        }
        public override string[] GetCombination()
        {
            return new[] { "Fire", "Fire", "Blade" };
        }
    }
}