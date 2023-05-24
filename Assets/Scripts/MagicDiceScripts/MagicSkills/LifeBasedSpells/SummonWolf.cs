using ScriptableScripts;
using SpellFormScripts;
using UnityEngine;

namespace MagicSkills.LifeBasedSpells
{
    public class SummonWolf: SingleSummonAllySpell
    {
        public SummonWolf()
        {
            characterScriptable = Resources.Load<CharacterScriptable>("ScriptableObjects/Characters/SummonedCreatures/Wolf");
            form = new SingleTargetSummonForm(characterScriptable);
        }
        public override string[] GetCombination()
        {
            return new[] { "Life", "Life", "Summon" };
        }
    }
}