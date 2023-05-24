using BattleFieldScripts;
using DefaultNamespace;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.EarthBasedSpells
{
    public class MurderousEarthArrows: IMagicSpell
    {
        private TargetingForm _form;
        
        public MurderousEarthArrows() 
        {
            var damageDealer = new DamageDealer();
            damageDealer.AddMagicDamage(MagicDamage.Earth, 3);
            damageDealer.AddPhysicDamage(PhysicDamage.Piercing, 3);
            _form = new TripleForm(damageDealer);
        }
        
        public string[] GetCombination()
        {
            return new[] { "Earth", "Earth", "Arrow", "Arrow" };
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