using BattleFieldScripts;
using DefaultNamespace;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.WindBasedSpells
{
    public class StrongWindVortex : IMagicSpell
    {
        private TargetingForm _form;
        
        public StrongWindVortex()
        {
            var damageDealer = new DamageDealer();
            damageDealer.AddMagicDamage(MagicDamage.Wind, 2);
            damageDealer.AddPhysicDamage(PhysicDamage.Bludgeoning, 1);
            _form = new SphereForm(damageDealer);
        }
        
        public string[] GetCombination()
        {
            return new[] { "Wind", "Sphere", "Sphere" };
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