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
    public class EnemyMovement : IInitializable, IDisposable, IEnemyState
    {
        public event Action<EnemyState> ChangeState = delegate(EnemyState state) { };

        private EnemyView _enemyView;
        private EnemyModel _enemyModel;
        private EnemyPathFind _enemyPathFind;
        private PlayerGroupModel _playerGroupModel;
        private InGameTime _gameTime;
        private Transform _hexTarget;


        private bool _changeState;
        private Tween _movement;
        private Queue<Vector3> _movementQueue = new Queue<Vector3>();
        private Queue<Hex> _path = new Queue<Hex>();
        private Vector2Int _target;

        public EnemyMovement(
            EnemyModel enemyModel,
            EnemyView enemyView,
            PlayerGroupModel playerGroupModel,
            EnemyPathFind enemyPathFind, 
            InGameTime gameTime, [Inject(Id = "hexHighlight")] Transform hextarget
            )
        {
            _enemyView = enemyView;
            _enemyModel = enemyModel;
            _playerGroupModel = playerGroupModel;
            _enemyPathFind = enemyPathFind;
            _gameTime = gameTime;
            _hexTarget = hextarget;
        }

        public void Initialize()
        {
            //_inGameTime.Tick += MovingOnTick;
            //CheckNextHex().Forget();
        }

        private  void CheckForEnemyNear()
        {
           
        }

        private async UniTask CheckNextHex()
        {
            if (_path.Count != 0)
            {
                var hex = _path.Dequeue();
                _target = hex.AxialCoordinate;
                _hexTarget.position = hex.Position;
                 Debug.Log(_target);
                _movementQueue = HexUtils.VectorSeparation(HexUtils.CalculatePosition(_enemyModel.AxialPosition), hex.Position, hex.LandTypeProperty.MovementTimeCost);
            }
            else
            {
                if (_enemyModel.AxialPosition == _playerGroupModel.AxialPosition)
                {
                    Debug.LogWarning("Player reached");
                    return;
                }
                
                _path = await _enemyPathFind.FindNewRandomPath();
                CheckNextHex().Forget();
            }
        }

        private async UniTask MovingOnTick()
        {
            if (_movementQueue.Count != 0)
            {
                Debug.Log(_movementQueue.Count);
                var moveTo = _movementQueue.Dequeue();
                Debug.Log( String.Format("from {0} to {1} ", _enemyView.transform.position, moveTo));
                _movement = _enemyView.transform.DOMove(moveTo, _gameTime.TickSeconds).SetEase(Ease.Linear);
                _enemyModel.ChangeEnergy(-1);
                
                if (_movementQueue.Count == 0)
                {
                    _enemyModel.AxialPosition = _target;
                    CheckNextHex().Forget();
                    
                    
                    if (_enemyModel.Energy <= 0)
                    {
                        Debug.Log("At check next hex " + _enemyModel.Energy);
                        _changeState = true;
                        await _movement;
                        ChangeState.Invoke(EnemyState.Rest);
                        return;
                    }
                    
                    
                    if (HexUtils.AxialDistance(_playerGroupModel.AxialPosition, _enemyModel.AxialPosition) < _enemyModel.EnemyProperties.ViewRadius)
                    {
                        _changeState = true;
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
            _changeState = false;
             await CheckNextHex();
        }
        public void ExitState()
        {
            Dispose();
        }
        public async UniTask OnGameTick()
        {
            if (!_changeState)
            {Debug.Log("Tick");
                await MovingOnTick();
            }
            
            //await UniTask.Yield();
        }

        public void OnGameTickTest()
        {
            Debug.Log("test");
        }
    }
}