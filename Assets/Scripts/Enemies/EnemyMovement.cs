using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameTime;
using PlayerGroup;
using UnityEngine;
using Zenject;

namespace Enemies
{
    public class EnemyMovement :  IDisposable, IEnemyState
    {
        public event Action<EnemyState> ChangeState = delegate(EnemyState state) { };

        private EnemyView _enemyView;
        private EnemyMapModel _enemyMapModel;
        private EnemyPathFind _enemyPathFind;
        private PlayerGroupModel _playerGroupModel;
        private InGameTime _gameTime;

        private Tween _movement;
        private Queue<Vector3> _movementQueue = new Queue<Vector3>();
        private Queue<Hex> _path = new Queue<Hex>();
        private Vector2Int _target;

        public EnemyMovement(
            EnemyMapModel enemyMapModel,
            EnemyView enemyView,
            PlayerGroupModel playerGroupModel,
            EnemyPathFind enemyPathFind, 
            InGameTime gameTime
            )
        {
            _enemyView = enemyView;
            _enemyMapModel = enemyMapModel;
            _playerGroupModel = playerGroupModel;
            _enemyPathFind = enemyPathFind;
            _gameTime = gameTime;
        }

        private void CalculateTargetPointerPosition(Vector3 targetPosition)
        {
            var pos =  targetPosition - HexUtils.CalculatePosition(_enemyMapModel.AxialPosition);
            _enemyView.SetTargetPointer(pos);
        }
        
        private async UniTask CheckNextHex()
        {
            if (_path.Count != 0)
            {
                var hex = _path.Dequeue();
                _target = hex.AxialCoordinate;
                CalculateTargetPointerPosition(hex.Position);
                _movementQueue = HexUtils.VectorSeparation(HexUtils.CalculatePosition(_enemyMapModel.AxialPosition), hex.Position, hex.LandTypeProperty.MovementTimeCost);
            }
            else
            {
                _path = await _enemyPathFind.FindNewRandomPath();
                CheckNextHex().Forget();
            }
        }
        

        private async UniTask MovingOnTick()
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
                    
                    
                    if (_enemyMapModel.Energy <= 0)
                    {
                        await _movement;
                        ChangeState.Invoke(EnemyState.Rest);
                        return;
                    }
                    
                    
                    if (HexUtils.AxialDistance(_playerGroupModel.AxialPosition, _enemyMapModel.AxialPosition) <= _enemyMapModel.EnemyProperties.ViewRadius)
                    {
                        await _movement;
                        ChangeState.Invoke(EnemyState.Chasing);
                    }
                }
            }
            else
            {
                CheckNextHex().Forget();
            }
        }
        
        public void Dispose()
        {
            _movement.Kill();
            _movementQueue.Clear();
            _path.Clear();
        }

        public async void EnterState()
        {
            _enemyView.TargetPointerArrowEnable(true);
            await CheckNextHex();
        }
        public void ExitState()
        {
            _enemyView.TargetPointerArrowEnable(false);
            Dispose();
        }
        public async UniTask OnGameTick()
        {
            await MovingOnTick();
        }
    }
}