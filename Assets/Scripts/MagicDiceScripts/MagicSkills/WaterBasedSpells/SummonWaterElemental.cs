using ScriptableScripts;
using SpellFormScripts;
using UnityEngine;

namespace MagicSkills.WaterBasedSpells
{
    public class SummonWaterElemental : SingleSummonAllySpell
    {
        public SummonWaterElemental()
        {
            characterScriptable = Resources.Load<CharacterScriptable>("ScriptableObjects/Characters/SummonedCreatures/Water Elemental");
            form = new SingleTargetSummonForm(characterScriptable);
        }
        public override string[] GetCombination()
        {
            return new[] { "Water", "Summon", "Flow" };
        }
    }
}