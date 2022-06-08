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
        private EnemyModel _enemyModel;
        private EnemyPathFind _enemyPathFind;
        private PlayerGroupModel _playerGroupModel;
        private InGameTime _gameTime;
        //private Transform _hexTarget;
        
        private Tween _movement;
        private Queue<Vector3> _movementQueue = new Queue<Vector3>();
        private Queue<Hex> _path = new Queue<Hex>();
        private Vector2Int _target;

        public EnemyMovement(
            EnemyModel enemyModel,
            EnemyView enemyView,
            PlayerGroupModel playerGroupModel,
            EnemyPathFind enemyPathFind, 
            InGameTime gameTime/*, [Inject(Id = "hexHighlight")] Transform hextarget*/
            )
        {
            //_hexTarget = enemyView.transform;
            _enemyView = enemyView;
            _enemyModel = enemyModel;
            _playerGroupModel = playerGroupModel;
            _enemyPathFind = enemyPathFind;
            _gameTime = gameTime;
            //_hexTarget = hextarget;
        }

        private async UniTask CheckNextHex()
        {
            if (_path.Count != 0)
            {
                var hex = _path.Dequeue();
                _target = hex.AxialCoordinate;
                //_hexTarget.position = hex.Position;
                _movementQueue = HexUtils.VectorSeparation(HexUtils.CalculatePosition(_enemyModel.AxialPosition), hex.Position, hex.LandTypeProperty.MovementTimeCost);
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
                _enemyModel.ChangeEnergy(-1);
                
                if (_movementQueue.Count == 0)
                {
                    _enemyModel.AxialPosition = _target;
                    CheckNextHex().Forget();
                    
                    
                    if (_enemyModel.Energy <= 0)
                    {
                        await _movement;
                        ChangeState.Invoke(EnemyState.Rest);
                        return;
                    }
                    
                    
                    if (HexUtils.AxialDistance(_playerGroupModel.AxialPosition, _enemyModel.AxialPosition) <= _enemyModel.EnemyProperties.ViewRadius)
                    {
                        await _movement;
                        ChangeState.Invoke(EnemyState.Chasing);
                    }
                }
            }
            else
            {
                Debug.Log("before = " + _movementQueue.Count);
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
            await CheckNextHex();
        }
        public void ExitState()
        {
            Dispose();
        }
        public async UniTask OnGameTick()
        {
            await MovingOnTick();
        }
    }
}