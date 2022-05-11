using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Interfaces;
using UnityEngine;
using Zenject;

namespace PlayerGroup
{
    public class PlayerGroupMovement : IPlayerGroupState
    {
        private PlayerGroupModel _playerGroupModel;
        private PlayerGroupView _playerGroupView;
        private PlayerPathFind _playerPathFind;
        private PlayerGroupStateManager _playerGroupStateManager;
        private GameTime _gameTime;
        
        private Queue<Vector3> _movementQueue = new Queue<Vector3>();
        private bool _hexReached;
        private bool _stopMoving;

        public PlayerGroupMovement(
            PlayerGroupModel playerGroupModel,
            PlayerGroupView playerGroupView,
            IHexMouseEvents hexMouse,
            GameTime gameTime, 
            PlayerGroupStateManager playerGroupStateManager, 
            PlayerPathFind playerPathFind)
        {
            _playerGroupModel = playerGroupModel;
            _playerGroupView = playerGroupView;
            _gameTime = gameTime;
            _playerGroupStateManager = playerGroupStateManager;
            _playerPathFind = playerPathFind;
        }

        private void OnDoubleClick()
        {
            //TODO 
            //_stopMoving = true;
        }
        
        private async UniTask StartMove()
        {
            var path = _playerPathFind.GetPath();
            if (path.Length > 0)
            {
                Debug.Log("player Start move");
                foreach (var pathPoint in path)
                {
                    _hexReached = false;
                    _stopMoving = false;
                    _playerGroupModel.SetTargetMovePosition(pathPoint.AxialCoordinate);
                    _movementQueue = HexUtils.VectorSeparation(_playerGroupView.transform.position, pathPoint.Position, pathPoint.LandTypeProperty.MovementTimeCost);
                    _gameTime.DoTick();
                    EnergyLossAt(pathPoint).Forget();
                    await UniTask.WaitUntil(() => _hexReached);
                    _playerPathFind.RemovePoint(pathPoint);
                    _playerGroupModel.SetAxialPosition(pathPoint.AxialCoordinate);
                    
                    if (_stopMoving)
                    {
                        break;
                    }
                }
            }
            _playerGroupStateManager.ChangeState(PlayerState.Idle);
        }

        private async UniTask EnergyLossAt(Hex hex)
        {
            float delay = hex.LandTypeProperty.MovementTimeCost * GameTime.MovementTimeModificator / hex.LandTypeProperty.MovementEnergyCost;
            for (int i = 0; i < hex.LandTypeProperty.MovementEnergyCost; i++)
            {
                _playerGroupModel.ChangeEnergy(-1);
                await UniTask.Delay(TimeSpan.FromSeconds(delay));
            }
        }
        
        private async UniTask MovingToHex()
        {
            if (_movementQueue.Count != 0)
            {
                await  _playerGroupView.transform.DOMove(_movementQueue.Dequeue(), GameTime.MovementTimeModificator).SetEase(Ease.Linear);
                
                if (_movementQueue.Count != 0)
                {
                    _gameTime.DoTick();
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
        }
        public void ExitState()
        {
        }
        public async UniTask OnGameTick()
        {
            await UniTask.Yield();
            MovingToHex().Forget();
        }
    }
}