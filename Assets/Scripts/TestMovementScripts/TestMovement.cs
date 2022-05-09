using Cysharp.Threading.Tasks;
using DG.Tweening;
using Interfaces;
using UnityEngine;
using Zenject;

namespace TestMovementScripts
{
    public class TestMovement : MonoBehaviour
    {
        private GameTime _gameTime;
        private IHexStorage _hexStorage;

        private Tween _movingTween;
        
        [Inject]
        public void Construct(GameTime gameTime,IHexStorage hexStorage)
        {
            _gameTime = gameTime;
            _hexStorage = hexStorage;
        }

       
        private async UniTask<bool> Move()
        {
           // await UniTask.WaitUntil(() => _gameTime.IsPlay);
            Debug.Log("startMove");
            _movingTween = transform.DOMoveX(50, 20).SetEase(Ease.Linear);
            await _movingTween.AwaitForComplete();
            Debug.Log("endMove");
            return true;
        }

        private void PauseMove(bool isPlay)
        {
            if (_movingTween != null)
            {
                if (isPlay)
                {
                    if (!_movingTween.IsPlaying())
                    {
                        _movingTween.Play();
                    }
                }
                else
                {
                    _movingTween.Pause();
                }
            }
        }
    }
}
