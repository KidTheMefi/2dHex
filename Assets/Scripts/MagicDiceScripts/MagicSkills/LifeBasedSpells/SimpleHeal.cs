using BattleFieldScripts;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.LifeBasedSpells
{
    public class SimpleHeal: SinglePositiveSpellOnAlly
    {
        public SimpleHeal()
        {
            positiveDealer = new PositiveEffectDealer(hpHeal: 2);
            form = new SingleTargetForm(positiveDealer);
        }

        public override PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.AnyPositionWithCharacter;
        }
        public override string[] GetCombination()
        {
            return new[] { "Life" };
        }
    }
}