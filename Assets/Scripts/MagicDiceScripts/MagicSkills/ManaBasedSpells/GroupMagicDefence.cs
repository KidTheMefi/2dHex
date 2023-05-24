using BattleFieldScripts;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.ManaBasedSpells
{
    public class GroupMagicDefence : IMagicSpell
    {
        private TargetingForm _form;
        
        public GroupMagicDefence() 
        {
            var positiveDealer = new PositiveEffectDealer(additionalMagicShield: 4);
            _form = new TripleForm(positiveDealer);
        }
        
        public string[] GetCombination()
        {
            return new[] { "Mana", "Shield", "Shield" };
        }
        public TargetingForm GetTargetingForm()
        {
            return _form;
        }
        public PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.AnyPosition;
        }
        public Field GetField()
        {
            return Field.PlayerField;
        }
    }
}