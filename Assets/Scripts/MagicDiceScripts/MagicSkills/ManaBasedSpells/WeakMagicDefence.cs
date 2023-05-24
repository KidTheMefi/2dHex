using BattleFieldScripts;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.ManaBasedSpells
{
    public class WeakMagicDefence : SinglePositiveSpellOnAlly
    {
        public WeakMagicDefence()
        {
            positiveDealer = new PositiveEffectDealer(additionalMagicShield: 2);
            form = new SingleTargetForm(positiveDealer);
        }

        public override PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.AnyPositionWithCharacter;
        }
        public override string[] GetCombination()
        {
            return new[] { "Mana" };
        }
    }
}