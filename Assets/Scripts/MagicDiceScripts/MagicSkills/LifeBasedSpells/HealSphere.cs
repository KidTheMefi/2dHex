using BattleFieldScripts;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.LifeBasedSpells
{
    public class HealSphere : SinglePositiveSpellOnAlly
    {
        public HealSphere()
        {
            positiveDealer = new PositiveEffectDealer(hpHeal: 5);
            form = new SingleTargetForm(positiveDealer);
        }

        public override PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.AnyPositionWithCharacter;
        }
        public override string[] GetCombination()
        {
            return new[] { "Life", "Sphere" };
        }
    }
}