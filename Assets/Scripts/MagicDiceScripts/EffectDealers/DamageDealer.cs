using System.Collections.Generic;
using CharactersScripts;
using DamageDisplayScripts;
using UnityEngine;

namespace EffectDealers
{
    public enum PhysicDamage
    {
        Piercing,
        Slashing,
        Bludgeoning
    }

    public enum MagicDamage
    {
        Fire,
        Water,
        Wind,
        Earth
    }

    public class DamageDealer : IEffectDealer
    {
        
        private Dictionary<MagicDamage, int> _magicDamage;
        private Dictionary<PhysicDamage, int> _physicDamage;

        public Dictionary<MagicDamage, int> MagicDamage => _magicDamage;
        public Dictionary<PhysicDamage, int> PhysicDamage => _physicDamage;

        public DamageDealer()
        {
            _magicDamage = new Dictionary<MagicDamage, int>();
            _physicDamage = new Dictionary<PhysicDamage, int>();
        }

        public DamageDealer(Dictionary<MagicDamage, int> magicDamage, Dictionary<PhysicDamage, int> physicDamage)
        {
            _magicDamage = magicDamage;
            _physicDamage = physicDamage;
        }

        public DamageDealer(DamageDealer baseDamageDealer, DamageDealer additionalDamage = null)
        {
            _magicDamage = new Dictionary<MagicDamage, int>();
            _physicDamage = new Dictionary<PhysicDamage, int>();

            foreach (var keyPair in baseDamageDealer.PhysicDamage)
            {
                _physicDamage.Add(keyPair.Key, keyPair.Value);
            }

            foreach (var keyPair in baseDamageDealer.MagicDamage)
            {
                _magicDamage.Add(keyPair.Key, keyPair.Value);
            }

            if (additionalDamage == null)
            {
                return;
            }
            foreach (var keyPair in additionalDamage._magicDamage)
            {
                AddMagicDamage(keyPair.Key, keyPair.Value);
            }
            
            foreach (var keyPair in additionalDamage._physicDamage)
            {
                AddPhysicDamage(keyPair.Key, keyPair.Value);
            }
        }


        public string DamageDescription()
        {
            string description = null;
            foreach (var damage in _physicDamage)
            {
                description += $"   {damage.Key}: {damage.Value} \n";
            }
            foreach (var damage in _magicDamage)
            {
                description += $"   {damage.Key}: {damage.Value} \n";
            }
            return description;
        }
        
        public void DealEffectTo(Character character, bool displayDamage = false)
        {
            if (character == null)
            {
                return;
            }
            DamageReceiver damageReceiver = character.DamageReceiver;
            
            if (displayDamage)
            {
                DamageDisplaySignals.GetInstance().InvokeDisplaySignal(damageReceiver.DamageDescription(this), character.transform.position);
            }
            
            foreach (var physicDamage in _physicDamage)
            {
                damageReceiver.DealPhysicDamage(physicDamage.Key, physicDamage.Value);
            }

            foreach (var magicDamage in _magicDamage)
            {
                damageReceiver.DealMagicDamage(magicDamage.Key, magicDamage.Value);
            }
        }

        public void AddMagicDamage(MagicDamage magicType, int damage)
        {
            AddMagicDamageToDictionary(_magicDamage, magicType, damage);
            
        }

        private static void AddMagicDamageToDictionary(Dictionary<MagicDamage, int> magicDamage, MagicDamage magicType, int damage)
        {
            if (damage < 1 && !magicDamage.ContainsKey(magicType))
            {
                return;
            }

            if (magicDamage.ContainsKey(magicType))
            {
                var newDamage = magicDamage[magicType] + damage;
                newDamage = newDamage < 0 ? 0 : newDamage;
                magicDamage[magicType] = newDamage;
            }
            else
            {
                magicDamage.Add(magicType, damage);
            }
        }

        public void AddPhysicDamage(PhysicDamage physicDamage, int damage)
        {
            if (damage < 1 && !_physicDamage.ContainsKey(physicDamage))
            {
                return;
            }

            if (_physicDamage.ContainsKey(physicDamage))
            {
                var newDamage = _physicDamage[physicDamage] + damage;
                newDamage = newDamage < 0 ? 0 : newDamage;
                _physicDamage[physicDamage] = newDamage;
            }
            else
            {
                _physicDamage.Add(physicDamage, damage);
            }
        }

        public void RemoveAllDamage()
        {
            _magicDamage.Clear();
            _physicDamage.Clear();
        }

        public static bool CanBeCharmedWithAdditionalDamage(DamageDealer baseDamage, DamageDealer additionalDamage)
        {
            foreach (var magicDamage in additionalDamage.MagicDamage.Keys)
            {
                if (baseDamage.MagicDamage.ContainsKey(magicDamage))
                {
                    return true;
                }
            }
            
            foreach (var physicDamage in additionalDamage.PhysicDamage.Keys)
            {
                if (baseDamage.PhysicDamage.ContainsKey(physicDamage))
                {
                    return true;
                }
            }

            if (baseDamage._magicDamage.Count == 0 && additionalDamage._magicDamage.Count != 0)
            {
                return true;
            }
            return false;
        }
    }
}