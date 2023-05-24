using BattleFieldScripts;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.LifeBasedSpells
{
    public class HealFlow : SinglePositiveSpellOnAlly
    {
        public HealFlow()
        {   positiveDealer = new PositiveEffectDealer(hpHeal: 3);
            form = new ColumnOneEffect(positiveDealer);
        }

        public override PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.AnyPositionWithCharacter;
        }
        public override string[] GetCombination()
        {
            return new[] { "Life", "Flow" };
        }
    }
}