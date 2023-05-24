using CharactersScripts;

namespace EffectDealers
{
    public class WeaponCharmEffectDealer : IEffectDealer
    {
        private readonly DamageDealer _damageDealer;
        
        
        public WeaponCharmEffectDealer(MagicDamage magicDamage, int value)
        {
            _damageDealer = new DamageDealer();
            _damageDealer.AddMagicDamage(magicDamage, value);
        }
        
        public WeaponCharmEffectDealer(PhysicDamage physicDamage, int value)
        {
            _damageDealer = new DamageDealer();
            _damageDealer.AddPhysicDamage(physicDamage, value);
        }
        

        public void DealEffectTo(Character character, bool show)
        {
            if (_damageDealer != null && character.CharacterAttackHandler.CanBeCharmedWithAdditionalDamage(_damageDealer))
            {
                character.CharmDamage(_damageDealer);
            }
        }
    }
}