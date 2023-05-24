using ScriptableScripts;
using SpellFormScripts;
using UnityEngine;

namespace MagicSkills.LifeBasedSpells
{
    public class SummonWildBoars : SingleSummonAllySpell
    {
        public SummonWildBoars()
        {
            characterScriptable = Resources.Load<CharacterScriptable>("ScriptableObjects/Characters/SummonedCreatures/Boar");
            form = new TripleSummon(characterScriptable);
        }
        public override string[] GetCombination()
        {
            return new[] { "Life", "Summon", "Summon" };
        }
    }
}