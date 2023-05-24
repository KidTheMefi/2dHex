using BattleFieldScripts;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.EarthBasedSpells
{
    public class EarthSpear : IMagicSpell
    {
        private TargetingForm _targetingForm;
        public EarthSpear()
        {
            DamageDealer damageDealer = new DamageDealer();
            damageDealer.AddPhysicDamage(PhysicDamage.Piercing, 3);
            damageDealer.AddMagicDamage(MagicDamage.Earth, 2);
            _targetingForm = new SpearForm(damageDealer);
        }
        
        
        public string[] GetCombination()
        {
            return new[] { "Earth", "Arrow", "Blade" };
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