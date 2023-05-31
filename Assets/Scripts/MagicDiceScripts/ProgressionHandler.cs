using System;
using TeamCreation;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class ProgressionHandler : MonoBehaviour
    {
        private static ProgressionHandler _instance;
        
        public int PlayerMoneyValue { get; private set;}
        [SerializeField]
        private TextMeshProUGUI _playerMoneyText;

        public static ProgressionHandler GetInstance()
        {
            if (_instance == null)
            {
                var instance = Instantiate(Resources.Load<ProgressionHandler>("ProgressionHandler"));
                if (instance == null)
                {
                    throw new NullReferenceException();
                }
                _instance = instance;
            }
            return _instance;
        }

        private void Awake()
        {
            _instance = this;
            DontDestroyOnLoad(this);
            PlayerMoneyValue = 10;
            MoneyValueChanged();
        }

        public void ChangeMoneyValueBy(int value)
        {
            PlayerMoneyValue += value;
            PlayerMoneyValue = PlayerMoneyValue > 0 ? PlayerMoneyValue : 0;
            MoneyValueChanged();
        }

        private void MoneyValueChanged()
        {
            _playerMoneyText.text = PlayerMoneyValue.ToString();
        }
    }
}