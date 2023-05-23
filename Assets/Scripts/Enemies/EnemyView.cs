using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Enemies
{
    public class EnemyView : MonoBehaviour, IEnemyVisualisation
    {
        public event Action<bool> MouseOnObject = delegate(bool b)
        {
            
        };
        
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private ParticleSystem _sleepParticle;
        [SerializeField] private SpriteRenderer _iconSpriteRenderer;
        [SerializeField] private SpriteRenderer _targetPointerArrow;
        [SerializeField] private Collider2D _collider;

        private bool isChasing;

        public void SetSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }

        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        private void ReturnDefaultVisualisation()
        {
            _sleepParticle.Stop();
            //_iconSpriteRenderer.DOFade(0.5f, 0);
            isChasing = false;
        }

        private async UniTask ChasingMode()
        {
            isChasing = true;
        }

        public void TargetPointerArrowEnable(bool value)
        {
            _targetPointerArrow.gameObject.SetActive(value);
        }

        public void SetTargetPointer(Vector3 position)
        {
            float angle = Vector3.Angle(Vector3.up, position) -45f;
            angle = position.x < 0 ? angle : -angle;
            _targetPointerArrow.gameObject.transform.rotation = Quaternion.Euler(0,0,angle);
            _targetPointerArrow.gameObject.transform.localPosition = position.normalized * 0.53f;
        }

        public void ChangeEnemyVisualisation(EnemyState state)
        {
            ReturnDefaultVisualisation();

            switch (state)
            {
                case EnemyState.Rest:
                    _sleepParticle.Play();
                    break;
                case EnemyState.Chasing:
                    ChasingMode().Forget();
                    break;
            }
        }

        public void OnMouseEnter()
        {
            MouseOnObject.Invoke(true);
        }

        public void OnMouseExit()
        {
            MouseOnObject.Invoke(false);
        }

    }
}