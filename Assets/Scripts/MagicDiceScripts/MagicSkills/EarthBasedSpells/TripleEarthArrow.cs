using BattleFieldScripts;
using DefaultNamespace;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.EarthBasedSpells
{
    public class TripleEarthArrow : IMagicSpell
    {
        private TargetingForm _form;
        
        public TripleEarthArrow() 
        {
            var damageDealer = new DamageDealer();
            damageDealer.AddMagicDamage(MagicDamage.Earth, 1);
            damageDealer.AddPhysicDamage(PhysicDamage.Piercing, 2);
            _form = new TripleForm(damageDealer);
        }
        
        public string[] GetCombination()
        {
            return new[] { "Earth", "Arrow", "Arrow" };
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
            return Field.EnemyField;
        }
    }
}