using BattleFieldScripts;
using SpellFormScripts;

namespace MagicSkills
{
    public interface IMagicSpell
    {
        public string[] GetCombination();
        public TargetingForm GetTargetingForm();
        public PossibleTarget GetPossibleTarget();
        public Field GetField();
        
    }
}