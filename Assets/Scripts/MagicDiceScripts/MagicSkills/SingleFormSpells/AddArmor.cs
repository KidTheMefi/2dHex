using BattleFieldScripts;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.SingleFormSpells
{
    public class AddArmor: SinglePositiveSpellOnAlly
    {
        public AddArmor()
        {
            positiveDealer = new PositiveEffectDealer(additionalArmor:2);
            form = new SingleTargetForm(positiveDealer);
        }

        public override PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.AnyPositionWithCharacter;
        }
        public override string[] GetCombination()
        {
            return new[] { "Shield" };
        }
    }
}