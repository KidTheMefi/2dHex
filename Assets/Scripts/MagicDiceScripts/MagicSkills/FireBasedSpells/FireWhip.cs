using BattleFieldScripts;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.FireBasedSpells
{
    public class FireWhip : IMagicSpell
    {

        private TargetingForm _targetingForm;
        public FireWhip()
        {
            DamageDealer dealer = new DamageDealer();
            dealer.AddMagicDamage(MagicDamage.Fire, 3);
            _targetingForm = new WhipForm(dealer);
        }
        public string[] GetCombination()
        {
            return new[] { "Fire", "Blade", "Flow" };
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