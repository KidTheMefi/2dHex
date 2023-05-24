using BattleFieldScripts;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.ManaBasedSpells
{
    public class SphereBarrier: IMagicSpell
    {
        private TargetingForm _form;
        
        public SphereBarrier() 
        {
            var additionalDefence = new PositiveEffectDealer(additionalMagicShield: 3, additionalArmor: 2);
            
           
            _form = new SphereForm(additionalDefence);
        }
        
        public string[] GetCombination()
        {
            return new[] { "Mana", "Shield", "Sphere" };
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