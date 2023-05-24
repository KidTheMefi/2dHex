using ScriptableScripts;
using SpellFormScripts;
using UnityEngine;

namespace MagicSkills.LifeBasedSpells
{
    public class SummonWolfPack: SingleSummonAllySpell
    {
        public SummonWolfPack()
        {
            characterScriptable = Resources.Load<CharacterScriptable>("ScriptableObjects/Characters/SummonedCreatures/Wolf");
            form = new TripleSummon(characterScriptable);
        }
        public override string[] GetCombination()
        {
            return new[] { "Life", "Life", "Summon", "Summon" };
        }
    }
}