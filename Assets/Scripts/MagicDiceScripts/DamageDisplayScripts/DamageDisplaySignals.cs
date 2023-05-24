using System;
using UnityEngine;

namespace DamageDisplayScripts
{
    public class DamageDisplaySignals
    {
        public event Action<string, Vector3> ShowDamageDisplay = delegate(string s, Vector3 vector3)
        {
            
        };
        
        public event Action HideAllDamage = delegate { };
        private static DamageDisplaySignals _instance;
        public static DamageDisplaySignals GetInstance()
        {
            return _instance ??= new DamageDisplaySignals();
        }

        public void InvokeDisplaySignal(string description, Vector3 position)
        {
            ShowDamageDisplay.Invoke(description, position);
        }

        public void InvokeHideDamage()
        {
            HideAllDamage.Invoke();
        }
    }
}