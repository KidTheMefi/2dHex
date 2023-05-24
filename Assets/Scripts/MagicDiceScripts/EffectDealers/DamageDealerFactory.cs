
namespace EffectDealers
{
    public static class DamageDealerFactory
    {
        public static DamageDealer CreateRandomPhysicAttackDealer()
        {
            var damageDealer = new DamageDealer();
            damageDealer.AddPhysicDamage(SomeStatic.RandomEnumValue<PhysicDamage>(), UnityEngine.Random.Range(1, 4));
            return damageDealer;
        }

        public static DamageDealer CreateRandomMagicAttackDealer()
        {
            var damageDealer = new DamageDealer();
            damageDealer.AddMagicDamage(SomeStatic.RandomEnumValue<MagicDamage>(), UnityEngine.Random.Range(1, 4));
            return damageDealer;
        }
    }
}