using BattleFieldScripts;
using DefaultNamespace;
using EffectDealers;
using MagicSkills.SomeSpellsTypes;
using SpellFormScripts;

namespace MagicSkills.FireBasedSpells
{
    public class FireArrow : SingleDamageSpellOnEnemy
    {
        public FireArrow()
        {
            damageDealer = new DamageDealer();
            damageDealer.AddMagicDamage(MagicDamage.Fire, 2);
            damageDealer.AddPhysicDamage(PhysicDamage.Piercing, 1);
            form = new SingleTargetForm(damageDealer);
        }

        public override string[] GetCombination()
        {
            return new[] { "Fire", "Arrow" };
        }
        
        public override PossibleTarget GetPossibleTarget()
        {
            return PossibleTarget.AnyPositionWithCharacter;;
        }
    }
}