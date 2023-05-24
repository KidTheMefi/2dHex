using DefaultNamespace;
using ScriptableScripts;
using UnityEngine;

namespace CharactersScripts
{
    public class CharacterFactoryWithPool : MonoBehaviour
    {
        private static CharacterFactoryWithPool _instance;
        public static CharacterFactoryWithPool Instance => _instance;
        
        [SerializeField]
        private Character _characterPrefab;
        private ObjectPool<Character> _characterPool;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                _characterPool = new ObjectPool<Character>(InstantiateCharacter, TurnCharacterOn, TurnCharacterOff, 20, true);
            }
            else
            {
                Destroy(this);
            }
        }
        
        private Character InstantiateCharacter()
        {
            var character = Instantiate(_characterPrefab, transform);
            character.transform.position = Vector3.up*20;
            character.CharacterRemoveEvent += BackToPool;
            return character;
        }
        
        private void TurnCharacterOff(Character character)
        {
            character.gameObject.SetActive(false);
            character.gameObject.name = _characterPrefab.name;
        }
        private void TurnCharacterOn(Character character)
        {
            character.gameObject.SetActive(true);
        }

        public Character CreateCharacter(CharacterScriptable characterScriptable)
        {
            var character = _characterPool.GetObject();
            character.Setup(characterScriptable);
            return character;
        }
        
        private void BackToPool(Character character)
        {
            character.transform.SetParent(transform);
            character.transform.position = Vector3.up*20;
            _characterPool.ReturnObject(character);
        }
    }
}