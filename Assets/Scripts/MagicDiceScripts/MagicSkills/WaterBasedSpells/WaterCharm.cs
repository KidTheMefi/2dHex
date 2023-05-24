using BattleFieldScripts;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.WaterBasedSpells
{
    public class WaterCharm : SinglePositiveSpellOnAlly
    {
        public WaterCharm()
        {
            positiveDealer = new WeaponCharmEffectDealer(MagicDamage.Water, 1);
            form = new SingleTargetForm(positiveDealer);
        }

        public override PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.AnyPositionWithCharacter;
        }
        public override string[] GetCombination()
        {
            return new[] { "Water" };
        }
    }
}