using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameEvents;
using GameTime;
using UnityEngine;
using Zenject;

namespace Enemies.EnemyStates
{
    public class EnemyChasing : IEnemyState
    {

        public event Action<EnemyState> ChangeState = delegate(EnemyState state) { };
        

        private EnemyModel _enemyModel;
        private EnemyView _enemyView;
        private EnemyPathFind _enemyPathFind;
        private InGameTime _gameTime;
        readonly SignalBus _signalBus;

        private Tween _movement;
        private Queue<Vector3> _movementQueue = new Queue<Vector3>();
        private Queue<Hex> _path = new Queue<Hex>();
        private Vector2Int _target;
        
        public EnemyChasing(InGameTime gameTime, EnemyView enemyView, EnemyModel enemyModel, EnemyPathFind enemyPathFind, SignalBus signalBus)
        {
            _gameTime = gameTime;
            _enemyView = enemyView;
            _enemyModel = enemyModel;
            _enemyPathFind = enemyPathFind;
            _signalBus = signalBus;
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
                _enemyModel.ChangeEnergy(-1);
                if (_movementQueue.Count == 0)
                {
                    
                    _enemyModel.AxialPosition = _target;
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
            if (_enemyModel.Energy <= 0)
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
                _movementQueue = HexUtils.VectorSeparation(HexUtils.CalculatePosition(_enemyModel.AxialPosition), hex.Position, hex.LandTypeProperty.MovementTimeCost-1);
            }
            else
            {
                Debug.Log("PlayerReached");
                _signalBus.Fire(new GameSignals.EnemyAttackSignal() {enemyModel = _enemyModel} );
            }
        }
    }
}
