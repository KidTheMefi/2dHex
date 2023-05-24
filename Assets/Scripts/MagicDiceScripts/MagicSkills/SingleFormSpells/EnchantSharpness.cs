using BattleFieldScripts;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.SingleFormSpells
{
    public class EnchantSharpness : SinglePositiveSpellOnAlly
    {
        public EnchantSharpness()
        {
            positiveDealer = new WeaponCharmEffectDealer(PhysicDamage.Piercing, 1);
            form = new SingleTargetForm(positiveDealer);
        }

        public override PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.AnyPositionWithCharacter;
        }
        public override string[] GetCombination()
        {
            return new[] { "Arrow" };
        }
    }
}