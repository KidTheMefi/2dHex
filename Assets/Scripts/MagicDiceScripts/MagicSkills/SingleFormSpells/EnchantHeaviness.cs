using BattleFieldScripts;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.SingleFormSpells
{
    public class EnchantHeaviness : SinglePositiveSpellOnAlly
    {
        public EnchantHeaviness()
        {
            positiveDealer = new WeaponCharmEffectDealer(PhysicDamage.Bludgeoning, 1);
            form = new SingleTargetForm(positiveDealer);
        }

        public override PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.AnyPositionWithCharacter;
        }
        public override string[] GetCombination()
        {
            return new[] { "Sphere" };
        }
    }
}