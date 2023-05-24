using BattleFieldScripts;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.FireBasedSpells
{
    public class FireCharm : SinglePositiveSpellOnAlly
    {
        public FireCharm()
        {
            positiveDealer = new WeaponCharmEffectDealer(MagicDamage.Fire, 1);
            form = new SingleTargetForm(positiveDealer);
        }

        public override PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.AnyPositionWithCharacter;
        }
        public override string[] GetCombination()
        {
            return new[] { "Fire" };
        }
    }
}