using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameTime;
using PlayerGroup;
using UnityEngine;

namespace Enemies.EnemyStates
{
    public class EnemyChasing : IEnemyState
    {

        public event Action<EnemyState> ChangeState = delegate(EnemyState state) { };

        private EnemyMapModel _enemyMapModel;
        private EnemyView _enemyView;
        private EnemyPathFind _enemyPathFind;
        private PlayerGroupModel _playerGroupModel;
        private InGameTime _gameTime;

        private Tween _movement;
        private Queue<Vector3> _movementQueue = new Queue<Vector3>();
        private Queue<Hex> _path = new Queue<Hex>();
        private Vector2Int _target;
        
        public EnemyChasing(InGameTime gameTime, EnemyView enemyView, EnemyMapModel enemyMapModel, PlayerGroupModel playerGroupModel, EnemyPathFind enemyPathFind)
        {
            _gameTime = gameTime;
            _enemyView = enemyView;
            _enemyMapModel = enemyMapModel;
            _playerGroupModel = playerGroupModel;
            _enemyPathFind = enemyPathFind;
        }

        public async void EnterState()
        {
            Debug.Log("Enter chase state");
            _path = await _enemyPathFind.FindPathToPlayer();
        }
        public void ExitState()
        {
            Debug.Log("Exit chase state");
            _movementQueue.Clear();
            _path.Clear();
        }
        public async UniTask OnGameTick()
        {
            MovingOnTick();
            await UniTask.Yield();
        }
        
        private void MovingOnTick()
        {
            if (_movementQueue.Count != 0)
            {
                var moveTo = _movementQueue.Dequeue();
                _movement = _enemyView.transform.DOMove(moveTo, _gameTime.TickSeconds).SetEase(Ease.Linear);
                _enemyMapModel.ChangeEnergy(-1);
                if (_movementQueue.Count == 0)
                {
                    
                    _enemyMapModel.AxialPosition = _target;
                    CheckNextHex().Forget();
                }
            }
            else
            {
                CheckNextHex().Forget();
            }
        }
        
        private async UniTask CheckNextHex()
        {
            if (_enemyMapModel.Energy <= 0)
            {
                await _movement;
                ChangeState.Invoke(EnemyState.Rest);
                return;
            }
            
            _path = await _enemyPathFind.FindPathToPlayer();

            if (_path.Count != 0)
            {
                var hex = _path.Dequeue();
                _target = hex.AxialCoordinate;
                _movementQueue = HexUtils.VectorSeparation(HexUtils.CalculatePosition(_enemyMapModel.AxialPosition), hex.Position, hex.LandTypeProperty.MovementTimeCost-1);
            }
            else
            {
                Debug.LogWarning("Player reached");
                    return;
            }
        }
    }
}
