using ScriptableScripts;
using SpellFormScripts;
using UnityEngine;

namespace MagicSkills.SingleFormSpells
{
    public class SummonDummy : SingleSummonAllySpell
    {
        public SummonDummy()
        {
            characterScriptable = Resources.Load<CharacterScriptable>("ScriptableObjects/Characters/SummonedCreatures/Dummy");
            form = new SingleTargetSummonForm(characterScriptable);
        }
        public override string[] GetCombination()
        {
            return new[] {"Summon" };
        }
    }
}