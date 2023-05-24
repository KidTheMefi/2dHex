using System.Collections.Generic;
using BattleFieldScripts;
using DefaultNamespace;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.WindBasedSpells
{
    public class WindFlow: IMagicSpell
    {
        private TargetingForm _targetingForm;
        public WindFlow()
        {
            var baseMagicDamage = new Dictionary<MagicDamage, int>() { [MagicDamage.Wind] = 4 };
            var additionalMagicDamage = new Dictionary<MagicDamage, int>() { [MagicDamage.Wind] = -1 };
            DamageDealer baseDamageDealer = new DamageDealer(baseMagicDamage, new Dictionary<PhysicDamage, int>());
            DamageDealer additionalDamage = new DamageDealer(additionalMagicDamage, new Dictionary<PhysicDamage, int>());
            _targetingForm = new TargetBasedOnDistance(baseDamageDealer, additionalDamage, false);
        }
        
        public string[] GetCombination()
        {
            return new[] { "Wind", "Flow" };
        }
        public TargetingForm GetTargetingForm()
        {
            return _targetingForm;
        }
        public PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.FirstInRow;
        }
        public Field GetField()
        {
            return Field.EnemyField;
        }
    }
   
}