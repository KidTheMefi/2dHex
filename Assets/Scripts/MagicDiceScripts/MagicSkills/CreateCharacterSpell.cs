using ScriptableScripts;
using SpellFormScripts;

namespace MagicSkills
{
    public class CreateCharacterSpell: SingleSummonAllySpell
    {
        public CreateCharacterSpell()
        {
        }
        public CreateCharacterSpell(CharacterScriptable characterScriptable)
        {
            base.characterScriptable = characterScriptable;
            form = new SingleTargetSummonForm(characterScriptable);
        }
        public override string[] GetCombination()
        {
            return new[] { "Not spell"};
        }
    }
}