using CharactersScripts;
using ScriptableScripts;
using SpellFormScripts;
using UnityEngine;

namespace MagicSkills.WaterBasedSpells
{
    public class SummonWaterSpirit : SingleSummonAllySpell
    {
        public SummonWaterSpirit()
        {
            characterScriptable = Resources.Load<CharacterScriptable>("ScriptableObjects/Characters/SummonedCreatures/Water Spirit");
            form = new SingleTargetSummonForm(characterScriptable);
        }
        public override string[] GetCombination()
        {
            return new[] { "Water", "Summon" };
        }
    }
}