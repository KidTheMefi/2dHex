using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Enemies
{
    public class EnemyView : MonoBehaviour, IEnemyVisualisation
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private ParticleSystem _sleepParticle;
        [SerializeField] private SpriteRenderer _iconSpriteRenderer;

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
            _iconSpriteRenderer.DOFade(0.5f,0);
            isChasing = false;
        }

        private async UniTask ChasingMode()
        {
            isChasing = true;
            while (isChasing)
            {
                await _iconSpriteRenderer.DOFade(1f, 0.5f);
                await _iconSpriteRenderer.DOFade(0.5f, 0.5f);
            }
        }
        
        public void ChangeEnemyVisualisation(EnemyState state)
        {
            ReturnDefaultVisualisation();
                
            switch (state)
            {
                case EnemyState.Rest:
                    Debug.Log("Rest visualisation start");
                    _sleepParticle.Play();
                    break;
                case EnemyState.Chasing:
                    ChasingMode().Forget();
                    break;
            }
        }
    }
}
