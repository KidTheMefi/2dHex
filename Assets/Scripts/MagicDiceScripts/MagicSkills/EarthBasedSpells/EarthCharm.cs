using BattleFieldScripts;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.EarthBasedSpells
{
    public class EarthCharm : SinglePositiveSpellOnAlly
    {
        public EarthCharm()
        {
            positiveDealer = new WeaponCharmEffectDealer(MagicDamage.Earth,1);
            form = new SingleTargetForm(positiveDealer);
        }

        public override PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.AnyPositionWithCharacter;
        }
        public override string[] GetCombination()
        {
            return new[] { "Earth" };
        }
    }
}