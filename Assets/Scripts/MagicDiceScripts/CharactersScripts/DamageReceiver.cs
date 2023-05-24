using System.Collections.Generic;
using DefaultNamespace;
using EffectDealers;
using UnityEngine;

namespace CharactersScripts
{
    public class DamageReceiver
    {
        private ArmorHandler _armorHandler;
        private MagicDefenceHandler _magicDefenceHandler;
        private HealthHandler _healthHandler;

        public DamageReceiver(ArmorHandler armorHandler, MagicDefenceHandler magicDefenceHandler, HealthHandler healthHandler)
        {
            _armorHandler = armorHandler;
            _magicDefenceHandler = magicDefenceHandler;
            _healthHandler = healthHandler;
        }


        public void DealPhysicDamage(PhysicDamage physicType, int damage)
        {
            damage -= _armorHandler.GetDefenceValue(physicType);
            damage = damage < 0 ? 0 : damage;
            damage = _armorHandler.DealDamageToArmor(damage); 
            if (!(damage > 0))
            {
                return;
            }
            _healthHandler.DealHealthDamage(damage);
        }

        public void DealMagicDamage(MagicDamage magicType, int damage)
        {
            damage -= _magicDefenceHandler.GetDefenceValue(magicType);
       
            damage = damage < 0 ? 0 : damage;
            damage = _magicDefenceHandler.DealDamageToDefence(damage);
            if (!(damage > 0))
            {
                return;
            }
            _healthHandler.DealHealthDamage(damage);
        }

        public string DamageDescription(DamageDealer damageDealer)
        {
            string description = null;

           
            int sumDamage = 0;
            foreach (var magic in damageDealer.MagicDamage)
            {
                var defenceValue = _magicDefenceHandler.GetDefenceValue(magic.Key);
                int resultDamage = magic.Value - defenceValue;
                resultDamage = resultDamage < 0 ? 0 : resultDamage;
                description += $"{magic.Key}: {magic.Value} - {defenceValue} = {resultDamage} \n";
                sumDamage += resultDamage;
            }
            description += damageDealer.MagicDamage.Count > 0 ? $"MagicDamage: {sumDamage}\n" : null;


           
            sumDamage = 0;
            foreach (var physic in damageDealer.PhysicDamage)
            {
                var defenceValue = _armorHandler.GetDefenceValue(physic.Key);
                int resultDamage = physic.Value - defenceValue;
                resultDamage = resultDamage < 0 ? 0 : resultDamage;
                description += $"{physic.Key}: {physic.Value} - {defenceValue} = {resultDamage} \n";
                sumDamage += resultDamage;
            }
            description += damageDealer.PhysicDamage.Count > 0 ? $"PhysicDamage: {sumDamage}\n" : null;
            
            return description;
        }
    }
}