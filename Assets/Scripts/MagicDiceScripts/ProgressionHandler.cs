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
        private TextMeshPro _playerMoneyText;
        [SerializeField]
        private TextMeshPro _levelText;
        private int _lvl;
        
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
            _lvl = 0;
            DontDestroyOnLoad(this);
            PlayerMoneyValue = 12;
            MoneyValueChanged();
            NextLevel();
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

        public void NextLevel()
        {
            _lvl++;
            ComputerTeam computerTeam = new ComputerTeam(_lvl);
            Debug.Log($"{_lvl} level team constructed");
            _levelText.text = $"Enemy team level: {_lvl}";
        }
        
    }
}