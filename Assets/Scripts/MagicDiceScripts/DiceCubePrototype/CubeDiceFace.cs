using DefaultNamespace;
using ScriptableScripts;
using UnityEngine;

namespace DiceCubePrototype
{
    public class CubeDiceFace : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _diceFaceSpriteRenderer ;
        [SerializeField]
        private SpriteRenderer _spriteSelectedRenderer ;
        [SerializeField]
        private Collider _checkCollider;

        private Color _defaultColor;
        private Color _selectedColor = new Color(1,1,1,0.2f);
        private RuneScriptable _runeScriptable;
        public RuneScriptable RuneScriptable => _runeScriptable;
        
        private void Start()
        {
            _defaultColor = _diceFaceSpriteRenderer.color;
        }

        private void HighLightFace()
        {
            _diceFaceSpriteRenderer.color = Color.white;
        }

        public void FaceOnTop()
        {
            HighLightFace();
        }

        public void Selected(bool value)
        {
            if (value)
            {
                _spriteSelectedRenderer.sprite = _diceFaceSpriteRenderer.sprite;
                _diceFaceSpriteRenderer.color =  _defaultColor;
                _spriteSelectedRenderer.enabled = true;
            }
            else
            {
                _spriteSelectedRenderer.sprite = _diceFaceSpriteRenderer.sprite;
                HighLightFace();
                _spriteSelectedRenderer.enabled = false;
            }
        }

        public void ResetFace()
        {
            Selected(false);
            _diceFaceSpriteRenderer.color =  _defaultColor;
        }

        public void SetRune(RuneScriptable runeScriptable)
        {
            _runeScriptable = runeScriptable;
            _diceFaceSpriteRenderer.sprite = _runeScriptable !=  null ? _runeScriptable.RuneOnDiceSprite : null;
        }
        public Sprite GetFaceSprite()
        {
            return _diceFaceSpriteRenderer.sprite;
        }
    }
}