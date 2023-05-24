using System.Collections.Generic;
using BattleFieldScripts;
using DefaultNamespace;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.WaterBasedSpells
{
    public class Tsunami : IMagicSpell
    {
        private TargetingForm _targetingForm;

        public Tsunami()
        {
            var baseMagicDamage = new Dictionary<MagicDamage, int>() { [MagicDamage.Water] = 4 };
            var basePhysicDamage = new Dictionary<PhysicDamage, int>() { [PhysicDamage.Bludgeoning] = 3 };
            var additionalPhysicDamage = new Dictionary<PhysicDamage, int>() { [PhysicDamage.Bludgeoning] = -1 };
            DamageDealer baseDamageDealer = new DamageDealer(baseMagicDamage, basePhysicDamage);
            DamageDealer additionalDamage = new DamageDealer(new Dictionary<MagicDamage, int>(), additionalPhysicDamage);
            _targetingForm = new TargetBasedOnDistance(baseDamageDealer, additionalDamage, true);
        }

        public string[] GetCombination()
        {
            return new[] { "Water", "Water", "Flow", "Flow" };
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