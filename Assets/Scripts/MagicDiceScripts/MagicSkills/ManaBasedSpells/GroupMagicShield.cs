using System.Collections.Generic;
using BattleFieldScripts;
using CharactersScripts;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.ManaBasedSpells
{
    public class GroupMagicShield : IMagicSpell
    {
        private TargetingForm _form;
        
        public GroupMagicShield() 
        {
            var additionalDefence = new PositiveEffectDealer(additionalMagicShield: 3);

            Shield shield = new Shield("Magic Shield",new Dictionary<MagicDamage, int>());
            
            shield.MagicDefence.Add(MagicDamage.Wind, 1);
            shield.MagicDefence.Add(MagicDamage.Water, 1);
            shield.MagicDefence.Add(MagicDamage.Fire, 1);
            shield.MagicDefence.Add(MagicDamage.Earth, 1);

            var additionalResist = new ShieldEffectDealer(shield);

           
            _form = new TripleForm(new DoubleEffectDealer(additionalResist, additionalDefence));
        }
        
        public string[] GetCombination()
        {
            return new[] { "Mana", "Mana", "Shield", "Shield" };
        }
        public TargetingForm GetTargetingForm()
        {
            return _form;
        }
        public PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.AnyPosition;
        }
        public Field GetField()
        {
            return Field.PlayerField;
        }
    
    }
}