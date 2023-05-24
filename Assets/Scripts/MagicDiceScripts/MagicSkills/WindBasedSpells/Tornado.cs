using BattleFieldScripts;
using DefaultNamespace;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.WindBasedSpells
{
    public class Tornado: IMagicSpell
    {
        private TargetingForm _form;
        
        public Tornado()
        {
            var damageDealer = new DamageDealer();
            damageDealer.AddMagicDamage(MagicDamage.Wind, 4);
            damageDealer.AddPhysicDamage(PhysicDamage.Bludgeoning, 2);
            _form = new SphereForm(damageDealer);
        }
        
        public string[] GetCombination()
        {
            return new[] { "Wind", "Wind", "Sphere", "Sphere" };
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