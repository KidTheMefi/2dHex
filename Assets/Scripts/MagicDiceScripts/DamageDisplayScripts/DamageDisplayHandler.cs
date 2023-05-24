using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace DamageDisplayScripts
{
    public class DamageDisplayHandler : MonoBehaviour
    {
        [SerializeField]
        private DamageDisplay _damageDisplayPrefab;
        private ObjectPool<DamageDisplay> _characterPool;

        private List<DamageDisplay> _activeDisplay = new List<DamageDisplay>();
        private void Awake()
        {
            _characterPool = new ObjectPool<DamageDisplay>(InstantiateCharacter, TurnCharacterOn, TurnCharacterOff, 10, true);
            DamageDisplaySignals.GetInstance().ShowDamageDisplay += ShowDamageDisplay;
            DamageDisplaySignals.GetInstance().HideAllDamage += HideAllDamageDisplay;
        }
    
        private void TurnCharacterOff(DamageDisplay display)
        {
            display.gameObject.SetActive(false);
            display.transform.position = Vector3.up*20;
        }
        private void TurnCharacterOn(DamageDisplay display)
        {
            display.gameObject.SetActive(true);
        }
        private DamageDisplay InstantiateCharacter()
        {
            var display = Instantiate(_damageDisplayPrefab, transform);
            display.transform.position = Vector3.up*20;
            return display;
        }
    
        private void ShowDamageDisplay(string description, Vector3 position)
        {
            var display = _characterPool.GetObject();
            display.SetDamageDescription(description);
            display.transform.position = position;
            _activeDisplay.Add(display);
        }

        public void HideAllDamageDisplay()
        {
            foreach (var display in _activeDisplay)
            {
                _characterPool.ReturnObject(display);
            }
        }

        private void OnDestroy()
        {
            DamageDisplaySignals.GetInstance().ShowDamageDisplay -= ShowDamageDisplay;
            DamageDisplaySignals.GetInstance().HideAllDamage -= HideAllDamageDisplay;
        }
    }
}