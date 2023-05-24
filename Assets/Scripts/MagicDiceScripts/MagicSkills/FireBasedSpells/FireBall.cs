using BattleFieldScripts;
using DefaultNamespace;
using EffectDealers;
using MagicSkills.SomeSpellsTypes;
using SpellFormScripts;

namespace MagicSkills.FireBasedSpells
{
    public class FireBall : SingleDamageSpellOnEnemy
    {
        public FireBall()
        {
            damageDealer = new DamageDealer();
            damageDealer.AddMagicDamage(MagicDamage.Fire, 2);
            damageDealer.AddPhysicDamage(PhysicDamage.Bludgeoning, 1);
            form = new SingleTargetForm(damageDealer);
        }

        public override PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.FirstInRow;
        }
        public override string[] GetCombination()
        {
            return new[] { "Fire", "Sphere" };
        }
        
    }
}