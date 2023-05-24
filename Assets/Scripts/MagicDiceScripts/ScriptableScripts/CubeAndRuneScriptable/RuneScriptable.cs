using UnityEngine;

namespace ScriptableScripts
{
    [CreateAssetMenu(menuName = "RuneSetting")]
    public class RuneScriptable : ScriptableObject
    {
        [SerializeField]
        private string _runeName;
        [SerializeField]
        private Sprite _runeOnDiceSprite;
        [SerializeField]
        private Sprite _simpleRuneSprite;

        public string RuneName => _runeName;
        public Sprite RuneOnDiceSprite => _runeOnDiceSprite;
        public Sprite SimpleRuneSprite => _simpleRuneSprite;
        
        private void OnEnable()
        {
            _runeName = name;
        }
        private void OnValidate()
        {
            _runeName = name;
        }
    }
}