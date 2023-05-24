using CharactersScripts;
using ScriptableScripts;
using SpellFormScripts;
using UnityEngine;

namespace MagicSkills.LifeBasedSpells
{
    public class SummonBoar : SingleSummonAllySpell
    {
        public SummonBoar()
        {
            characterScriptable = Resources.Load<CharacterScriptable>("ScriptableObjects/Characters/SummonedCreatures/Boar");
            form = new SingleTargetSummonForm(characterScriptable);
        }
        public override string[] GetCombination()
        {
            return new[] { "Life", "Summon" };
        }
    }
}