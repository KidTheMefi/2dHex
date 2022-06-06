using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace PlayerGroup
{
    public class PlayerGroupMovement : IPlayerGroupState
    {
        private PlayerGroupModel _playerGroupModel;
        private PlayerGroupView _playerGroupView;
        private PlayerPathFind _playerPathFind;
        private PlayerGroupStateManager _playerGroupStateManager;
        private GameTime.InGameTime _inGameTime;

        private Tween _movement;
        private Queue<Vector3> _movementQueue = new Queue<Vector3>();
        private bool _hexReached;
        private bool _stopMovement;

        public PlayerGroupMovement(
            PlayerGroupModel playerGroupModel,
            PlayerGroupView playerGroupView,
            GameTime.InGameTime inGameTime, 
            PlayerGroupStateManager playerGroupStateManager, 
            PlayerPathFind playerPathFind)
        {
            _playerGroupModel = playerGroupModel;
            _playerGroupView = playerGroupView;
            _inGameTime = inGameTime;
            _playerGroupStateManager = playerGroupStateManager;
            _playerPathFind = playerPathFind;
        }

        private async UniTask StartMove()
        {
            var path = _playerPathFind.GetPath();
            if (path.Length > 0)
            {
                Debug.Log("player Start move");
                foreach (var pathPoint in path)
                {
                    _stopMovement = false;
                    _hexReached = false;
                    _playerGroupModel.SetTargetMovePosition(pathPoint.AxialCoordinate);
                    _movementQueue = HexUtils.VectorSeparation(_playerGroupView.transform.position, pathPoint.Position, pathPoint.LandTypeProperty.MovementTimeCost);
                    _inGameTime.DoTick();
                    EnergyLossAt(pathPoint).Forget();
                    await UniTask.WaitUntil(() => _hexReached);
                    _playerPathFind.RemovePoint(pathPoint);
                    _playerGroupModel.SetAxialPosition(pathPoint.AxialCoordinate);
                    
                    if (_stopMovement)
                    {
                        break;
                    }
                }
            }
            _playerGroupStateManager.ChangeState(PlayerState.Idle);
        }

        private async UniTask EnergyLossAt(Hex hex)
        {
            float delay = hex.LandTypeProperty.MovementTimeCost * _inGameTime.TickSeconds / hex.LandTypeProperty.MovementEnergyCost;
            for (int i = 0; i < hex.LandTypeProperty.MovementEnergyCost; i++)
            {
                _playerGroupModel.ChangeEnergy(-1);
                await DOVirtual.DelayedCall(delay, () => { });
                //await UniTask.Delay(TimeSpan.FromSeconds(delay));
            }
        }
        
        private async UniTask MovingToHex()
        {
            if (_movementQueue.Count != 0)
            {
                _movement = _playerGroupView.transform.DOMove(_movementQueue.Dequeue(), _inGameTime.TickSeconds).SetEase(Ease.Linear);
                await _movement;
                
                if (_movementQueue.Count != 0)
                {
                    _inGameTime.DoTick();
                }
                else
                {
                    _hexReached = true;
                }
            }
        }

        public void EnterState()
        {
            Debug.Log("Entered Movement state");
            StartMove().Forget();
            _playerGroupView.EnableTrails(true);
        }
        
        public void ExitState()
        {
            _playerGroupView.EnableTrails(false);
        }
        
        public async UniTask OnGameTick()
        {
            MovingToHex().Forget();
            await UniTask.Yield();
        }
        public void OnStopPressed()
        {
            _stopMovement = true;
        }
    }
}