using System.Collections.Generic;
using BattleFieldScripts;
using DefaultNamespace;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.WaterBasedSpells
{
    public class StrongWaterFlow : IMagicSpell
    {
        private TargetingForm _targetingForm;
        
        public StrongWaterFlow()
        {
            var baseMagicDamage = new Dictionary<MagicDamage, int>() { [MagicDamage.Water] = 2 };
            var basePhysicDamage = new Dictionary<PhysicDamage, int>() { [PhysicDamage.Bludgeoning] = 2 };
            var additionalPhysicDamage = new Dictionary<PhysicDamage, int>() { [PhysicDamage.Bludgeoning] = -1 };
            DamageDealer baseDamageDealer = new DamageDealer(baseMagicDamage, basePhysicDamage);
            DamageDealer additionalDamage = new DamageDealer(new Dictionary<MagicDamage, int>(), additionalPhysicDamage);
            _targetingForm = new TargetBasedOnDistance(baseDamageDealer, additionalDamage, true);
        }
        
        public string[] GetCombination()
        {
            return new[] { "Water", "Flow", "Flow" };
        }
        public TargetingForm GetTargetingForm()
        {
            return _targetingForm;
        }
        public PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.AnyPositionWithCharacter;
        }
        public Field GetField()
        {
            return Field.EnemyField;
        }
    }
}