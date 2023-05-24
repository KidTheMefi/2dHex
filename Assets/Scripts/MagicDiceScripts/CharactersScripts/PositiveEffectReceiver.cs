namespace CharactersScripts
{
    public class PositiveEffectReceiver
    {
        
            private ArmorHandler _armorHandler;
            private MagicDefenceHandler _magicDefenceHandler;
            private HealthHandler _healthHandler;
            
        
            public PositiveEffectReceiver(ArmorHandler armorHandler, MagicDefenceHandler magicDefenceHandler, HealthHandler healthHandler)
            {
                _armorHandler = armorHandler;
                _magicDefenceHandler = magicDefenceHandler;
                _healthHandler = healthHandler;
            }

            public void Heal(int value)
            {
                _healthHandler.Heal(value);
            }

            public void AddMagicShield(int value)
            {
                _magicDefenceHandler.AddMagicShield(value);
            }

            public void AddArmor(int value)
            {
                _armorHandler.AddArmor(value);
            }
            
        
    }
}