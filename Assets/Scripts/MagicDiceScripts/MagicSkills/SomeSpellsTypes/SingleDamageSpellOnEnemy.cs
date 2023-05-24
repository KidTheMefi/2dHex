using BattleFieldScripts;
using DefaultNamespace;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.SomeSpellsTypes
{
    public abstract class SingleDamageSpellOnEnemy : IMagicSpell
    {
        protected TargetingForm form;
        protected DamageDealer damageDealer;
        public virtual TargetingForm GetTargetingForm()
        {
            return form;
        }
        public abstract PossibleTarget GetPossibleTarget();
        
        public Field GetField()
        {
            return Field.EnemyField;
        }
        public abstract string[] GetCombination();
    }
}