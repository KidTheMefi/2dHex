using System.Collections.Generic;
using DefaultNamespace;
using EffectDealers;
using TMPro;
using UnityEngine;

namespace CharactersScripts
{
    public class ArmorHandler : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro _currentValueText;
        [SerializeField]
        private TextMeshPro _temporaryValueText;
        
        private Dictionary<PhysicDamage, int> _basePhysicResistance = new Dictionary<PhysicDamage, int>();
        
        private CharacterEffectHandler _characterEffectHandler;
        public int ArmorCurrent { private set; get; }
        public int ArmorTemporary { private set; get; }

        
        public void Setup(CharacterEffectHandler characterEffectHandler, int armorValue = 0, Dictionary<PhysicDamage, int> physicDefence = null)
        {
            _characterEffectHandler = characterEffectHandler;
            ArmorCurrent = armorValue > 0 ? armorValue : 0;
            ArmorTemporary = ArmorCurrent;
            _temporaryValueText.text = ArmorTemporary.ToString();
            _currentValueText.text = ArmorCurrent.ToString();
            
            _basePhysicResistance = physicDefence ?? new Dictionary<PhysicDamage, int>();
        }

        public string GetArmorDefenceDescription()
        {
            string defence = null;

            if (_basePhysicResistance.Count != 0)
            {
                defence = "Physic Resistance:\n";
            }
            foreach (var physicDefence in _basePhysicResistance)
            {
                defence += $"{physicDefence.Key}: {physicDefence.Value} \n";
            }

            return defence;
        }
        
        public int GetDefenceValue(PhysicDamage damageType)
        {
            int defence = 0;

            if (_characterEffectHandler.TryGetEffectTypeOf(typeof(Shield), out var effect))
            {
                var shield = (Shield)effect;
                defence = shield.PhysicDefence.TryGetValue(damageType, out var shieldDefence) ? shieldDefence : 0;
            }
            
            return _basePhysicResistance.TryGetValue(damageType, out var baseDefence) ? defence + baseDefence : defence;
        }
        
        public int DealDamageToArmor(int damage)
        {
            int remnant = ArmorTemporary - damage;
            ArmorTemporary = remnant >= 0 ? remnant : 0;
            _temporaryValueText.color = ArmorTemporary != ArmorCurrent ? Color.red : Color.white;
            EnableTemporaryText(true);
            return remnant >= 0 ? 0 : -remnant;
        }

        public void AddArmor(int value)
        {
            _temporaryValueText.color = Color.green;
            ArmorTemporary += value;
            EnableTemporaryText(true);
        }
        
        public void ConfirmValue(bool value)
        {
            if (value)
            {
                ArmorCurrent = ArmorTemporary;
                EnableTemporaryText(false);
                _currentValueText.text = ArmorCurrent.ToString();
                
            }
            else
            {
                ArmorTemporary = ArmorCurrent;
                _temporaryValueText.text = ArmorTemporary.ToString();
                EnableTemporaryText(false);
            }
        }
        
        private void EnableTemporaryText(bool value)
        {
            if (value)
            {
                _temporaryValueText.text = ArmorTemporary.ToString();
            }
            _temporaryValueText.enabled = value;
            _currentValueText.enabled = !value;
        }
    }
}