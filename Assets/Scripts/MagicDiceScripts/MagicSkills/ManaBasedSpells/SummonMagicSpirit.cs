using CharactersScripts;
using ScriptableScripts;
using SpellFormScripts;
using UnityEngine;

namespace MagicSkills.ManaBasedSpells
{
    public class SummonMagicSpirit : SingleSummonAllySpell
    {
        public SummonMagicSpirit()
        {
            characterScriptable = Resources.Load<CharacterScriptable>("ScriptableObjects/Characters/SummonedCreatures/Magic Spirit");
            form = new SingleTargetSummonForm(characterScriptable);
        }
        public override string[] GetCombination()
        {
            return new[] { "Mana", "Summon" };
        }
    }
}