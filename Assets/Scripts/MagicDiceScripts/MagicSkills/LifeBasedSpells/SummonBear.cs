using ScriptableScripts;
using SpellFormScripts;
using UnityEngine;

namespace MagicSkills.LifeBasedSpells
{
    public class SummonBear : SingleSummonAllySpell
    {
        public SummonBear()
        {
            characterScriptable = Resources.Load<CharacterScriptable>("ScriptableObjects/Characters/SummonedCreatures/Bear");
            form = new SingleTargetSummonForm(characterScriptable);
        }
        public override string[] GetCombination()
        {
            return new[] { "Life", "Summon", "Shield" };
        }
    }
}