using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ScriptableScripts;
using UnityEngine;

namespace DiceCubePrototype
{
    public class DiceFaceInCreation : MonoBehaviour
    {
        public event Action<DiceFaceInCreation> FaceSelected = delegate(DiceFaceInCreation creation) { };
        public event Action FaceUnclicked = delegate { };
        [SerializeField]
        private SpriteRenderer _diceFaceSpriteRenderer;
        [SerializeField]
        private SpriteRenderer _spriteSelectedRenderer;
        [SerializeField]
        private Sprite _emptySprite;
        [SerializeField]
        private Collider _collider;
        private Color _defaultColor;
        private RuneScriptable _runeScriptable;
        public RuneScriptable RuneScriptable => _runeScriptable;
        private RuneScriptable _tempRune;
        private CancellationTokenSource _glowingCTS;
        

        private bool _isSelected;
        private void Start()
        {
            _defaultColor = _diceFaceSpriteRenderer.color;
            Glowing().Forget();
        }

        public void SetActiveCollision(bool value)
        {
            _collider.enabled = value;
        }
        
        private async UniTask Glowing()
        {
            _glowingCTS = new CancellationTokenSource();
            float colorAlpha = 0.02f;
            while (!_glowingCTS.IsCancellationRequested)
            {
                Color newColor = new Color(_defaultColor.r, _defaultColor.g, _defaultColor.b, _diceFaceSpriteRenderer.color.a + colorAlpha);
                _diceFaceSpriteRenderer.color = newColor;
                colorAlpha = newColor.a < _defaultColor.a || newColor.a > 0.97f ? -colorAlpha : colorAlpha;
                await UniTask.DelayFrame(3);
            }
        }

        private void OnDestroy()
        {
            _glowingCTS?.Cancel();
        }
        
        public void Selected(bool value)
        {
            if (_isSelected == value)
            {
                if (_isSelected)
                {
                    FaceUnclicked.Invoke();
                }
                return;
            }
            if (value)
            {
                _spriteSelectedRenderer.sprite = _diceFaceSpriteRenderer.sprite;
                _spriteSelectedRenderer.enabled = true;
                FaceSelected.Invoke(this);
            }
            else
            {
                _spriteSelectedRenderer.sprite = _diceFaceSpriteRenderer.sprite;
                //HighLightFace();
                _spriteSelectedRenderer.enabled = false;
            }
            _isSelected = value;
        }

        public void SetRune(RuneScriptable runeScriptable)
        {
            _runeScriptable = runeScriptable;
            _spriteSelectedRenderer.sprite = runeScriptable != null ? runeScriptable.RuneOnDiceSprite : _emptySprite;
            _diceFaceSpriteRenderer.sprite = _spriteSelectedRenderer.sprite;
        }
        
        public void SetTempRune(RuneScriptable runeScriptable)
        {
            _tempRune = runeScriptable;
            _spriteSelectedRenderer.sprite = runeScriptable != null ? runeScriptable.RuneOnDiceSprite : _emptySprite;
        }

        public void ConfirmRune(bool value)
        {
            _runeScriptable = value ? _tempRune : _runeScriptable;

            if (value)
            {
                _diceFaceSpriteRenderer.sprite = _spriteSelectedRenderer.sprite;
            }
            else
            {
                _spriteSelectedRenderer.sprite = _diceFaceSpriteRenderer.sprite;
                
            }
        }
    }
}