using BattleFieldScripts;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.ManaBasedSpells
{
    public class MagicDefence : SinglePositiveSpellOnAlly
    {
        public MagicDefence()
        {
            positiveDealer = new PositiveEffectDealer(additionalMagicShield: 4, additionalArmor:1);
            form = new SingleTargetForm(positiveDealer);
        }

        public override PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.AnyPositionWithCharacter;
        }
        public override string[] GetCombination()
        {
            return new[] { "Mana", "Shield" };
        }
    }
}