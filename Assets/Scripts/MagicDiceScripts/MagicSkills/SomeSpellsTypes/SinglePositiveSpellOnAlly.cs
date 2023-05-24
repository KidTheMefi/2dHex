using BattleFieldScripts;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills
{
    public abstract class SinglePositiveSpellOnAlly : IMagicSpell
    {
        protected TargetingForm form;
        protected IEffectDealer positiveDealer;

        public virtual TargetingForm GetTargetingForm()
        {
            return form;
        }
        public abstract PossibleTarget GetPossibleTarget();
        public abstract string[] GetCombination();
        public Field GetField()
        {
            return Field.PlayerField;
        }
    }
}