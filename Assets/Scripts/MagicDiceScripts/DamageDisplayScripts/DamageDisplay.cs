using TMPro;
using UnityEngine;

namespace DamageDisplayScripts
{
    public class DamageDisplay : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro _textMeshPro;
    
        public void SetDamageDescription(string description)
        {
            _textMeshPro.text = description;
        }
    }
}
