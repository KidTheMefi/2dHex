using BattleFieldScripts;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.SingleFormSpells
{
    public class EnchantBlade : SinglePositiveSpellOnAlly
    {
        public EnchantBlade()
        {
            positiveDealer = new WeaponCharmEffectDealer(PhysicDamage.Slashing, 1);
            form = new SingleTargetForm(positiveDealer);
        }

        public override PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.AnyPositionWithCharacter;
        }
        public override string[] GetCombination()
        {
            return new[] { "Blade" };
        }
    }
}