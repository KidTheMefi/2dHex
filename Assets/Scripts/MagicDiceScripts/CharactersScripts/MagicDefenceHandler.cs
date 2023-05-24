using System.Collections.Generic;
using DefaultNamespace;
using EffectDealers;
using TMPro;
using UnityEngine;

namespace CharactersScripts
{
    public class MagicDefenceHandler : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro _currentValueText;
        [SerializeField]
        private TextMeshPro _temporaryValueText;

        private Dictionary<MagicDamage, int> _baseMagicResistance = new Dictionary<MagicDamage, int>();
        private CharacterEffectHandler _characterEffectHandler;
        public int MagicDefenceCurrent { private set; get; }
        public int MagicDefenceTemporary { private set; get; }


        public void Setup(CharacterEffectHandler characterEffectHandler, int magicDefenceValue = 0, Dictionary<MagicDamage, int> magicDefence = null)
        {
            _characterEffectHandler = characterEffectHandler;
            MagicDefenceCurrent = magicDefenceValue > 0 ? magicDefenceValue : 0;
            MagicDefenceTemporary = MagicDefenceCurrent;
            _temporaryValueText.text = MagicDefenceTemporary.ToString();
            _currentValueText.text = MagicDefenceCurrent.ToString();

            _baseMagicResistance = magicDefence ?? new Dictionary<MagicDamage, int>();
        }



        public string GetMagicDefenceDescription()
        {
            string defence = null;


            if (_baseMagicResistance.Count != 0)
            {
                defence = "Magic Resistance:\n";
            }
            foreach (var magicDefence in _baseMagicResistance)
            {
                defence += $"{magicDefence.Key}: {magicDefence.Value} \n";
            }
            return defence;
        }


        public int GetDefenceValue(MagicDamage damageType)
        {
            int defence = 0;

            if (_characterEffectHandler.TryGetEffectTypeOf(typeof(Shield), out var effect))
            {
                var shield = (Shield)effect;
                defence = shield.MagicDefence.TryGetValue(damageType, out var shieldDefence) ? shieldDefence : 0;
            }


            return _baseMagicResistance.TryGetValue(damageType, out var baseDefence) ? defence + baseDefence : defence;
        }

        public int DealDamageToDefence(int damage)
        {
            int remnant = MagicDefenceTemporary - damage;
            MagicDefenceTemporary = remnant >= 0 ? remnant : 0;
            _temporaryValueText.color = MagicDefenceTemporary !=  MagicDefenceCurrent ? Color.red : Color.white;
            EnableTemporaryText(true);
            return remnant >= 0 ? 0 : -remnant;
        }

        public void AddMagicShield(int value)
        {
            value = value > 0 ? value : 0;
            MagicDefenceTemporary += value;
            _temporaryValueText.color = Color.green;
            EnableTemporaryText(true);
        }

        public void ConfirmDamage(bool value)
        {
            if (value)
            {
                MagicDefenceCurrent = MagicDefenceTemporary;
                EnableTemporaryText(false);
                _currentValueText.text = MagicDefenceCurrent.ToString();

            }
            else
            {
                MagicDefenceTemporary = MagicDefenceCurrent;
                _temporaryValueText.text = MagicDefenceTemporary.ToString();
                EnableTemporaryText(false);
            }
        }

        private void EnableTemporaryText(bool value)
        {
            if (value)
            {
                _temporaryValueText.text = MagicDefenceTemporary.ToString();
            }
            _temporaryValueText.enabled = value;
            _currentValueText.enabled = !value;
        }
    }
}