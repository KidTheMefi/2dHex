using System;
using UnityEngine;
using Zenject;

namespace Enemies
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        public void SetSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }

        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }
        
    }
}
