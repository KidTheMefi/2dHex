using System;
using TMPro;
using UnityEngine;

namespace CharactersScripts
{
    public class HealthHandler : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro _healthCurrent;
        [SerializeField]
        private TextMeshPro _healthTemporary;
        public int CurrentHealth { private set; get; }
        public int MaxHealth { private set; get; }
        public int TemporaryHealth { private set; get; }
        private Action _characterDeathAction;

        public void SetupHealth(int maxHp, int currentHealth, Action deathAction)
        {
            _characterDeathAction = deathAction;
            maxHp = maxHp > 0 ? maxHp : 1;
            currentHealth = currentHealth < 1 ? 1 : currentHealth < maxHp ? currentHealth : maxHp;
            MaxHealth = maxHp;
            CurrentHealth = MaxHealth;
            TemporaryHealth = currentHealth;
            _healthTemporary.enabled = false;
            _healthCurrent.enabled = true;
            _healthCurrent.text = CurrentHealth.ToString();
        }
        public void DealHealthDamage(int damage)
        {
            damage = damage > 0 ? damage : 0;
            
            _healthTemporary.color = damage > 0 ? Color.red : Color.white;
            TemporaryHealth = TemporaryHealth - damage;
            if (TemporaryHealth <= 0)
            {
                TemporaryHealth = 0;
            }
            _healthTemporary.text = TemporaryHealth.ToString();
            
            _healthTemporary.enabled = true;
            _healthCurrent.enabled = false;
        }
        
        public void Heal(int value)
        {
            value = value > 0 ? value : 0;
            _healthTemporary.color = TemporaryHealth != MaxHealth ? Color.green : Color.white;
            TemporaryHealth += value;
            TemporaryHealth = TemporaryHealth > MaxHealth ? MaxHealth : TemporaryHealth;
            _healthTemporary.text = TemporaryHealth.ToString();
            _healthTemporary.enabled = true;
            _healthCurrent.enabled = false;
        }

        public void ConfirmHealthValue(bool value)
        {
            if (value)
            {
                CurrentHealth = TemporaryHealth;
                _healthTemporary.enabled = false;
                _healthCurrent.enabled = true;
                _healthCurrent.text = CurrentHealth.ToString();
                if (!(CurrentHealth >0))
                {
                    _characterDeathAction.Invoke();
                }
            }
            else
            {
                TemporaryHealth = CurrentHealth;
                _healthTemporary.text = TemporaryHealth.ToString();
                _healthTemporary.enabled = false;
                _healthCurrent.enabled = true;
            }
        }
        
    }
}