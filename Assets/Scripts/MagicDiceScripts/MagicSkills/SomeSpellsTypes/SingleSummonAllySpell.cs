using BattleFieldScripts;
using CharactersScripts;
using ScriptableScripts;
using SpellFormScripts;

namespace MagicSkills
{
    public abstract class SingleSummonAllySpell : IMagicSpell
    {
        protected CharacterScriptable characterScriptable;
        protected TargetingForm form;

        public abstract string[] GetCombination();
        
        public virtual TargetingForm GetTargetingForm()
        {
            return form;
        }
        public PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.AnyPositionWithoutCharacter;
        }
        public Field GetField()
        {
            return Field.PlayerField;
        }
        
        /*public void Canceled()
        {
            summonedCharacter.CharacterDeath();
        }*/
    }
}