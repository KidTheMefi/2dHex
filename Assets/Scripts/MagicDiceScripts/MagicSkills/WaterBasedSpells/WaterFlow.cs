using System.Collections.Generic;
using BattleFieldScripts;
using DefaultNamespace;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.WaterBasedSpells
{
    public class WaterFlow : IMagicSpell
    {
        private TargetingForm _targetingForm;
        public WaterFlow()
        {
            var baseMagicDamage = new Dictionary<MagicDamage, int>() { [MagicDamage.Water] = 2 };
            var basePhysicDamage = new Dictionary<PhysicDamage, int>() { [PhysicDamage.Bludgeoning] = 2 };
            var additionalPhysicDamage = new Dictionary<PhysicDamage, int>() { [PhysicDamage.Bludgeoning] = -1 };
            DamageDealer baseDamageDealer = new DamageDealer(baseMagicDamage, basePhysicDamage);
            DamageDealer additionalDamage = new DamageDealer(new Dictionary<MagicDamage, int>(), additionalPhysicDamage);
            _targetingForm = new TargetBasedOnDistance(baseDamageDealer, additionalDamage, false);
        }
        
        public string[] GetCombination()
        {
            return new[] { "Water", "Flow" };
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