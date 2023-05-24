using System;
using UnityEngine;

namespace DiceCubePrototype
{
    public class CubeFaceOnViewer : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        private Color _defaultColor;
        private void Start()
        {
            _defaultColor = _spriteRenderer.color;
        }
        public void SetSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }
        public void ChangeColorSprite(Color color)
        {
            _spriteRenderer.color = color;
        }

        public void ReturnDefaultColor()
        {
            _spriteRenderer.color = _defaultColor;
        }
    }
}