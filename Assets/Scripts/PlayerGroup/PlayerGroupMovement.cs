using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Interfaces;
using UnityEngine;
using Zenject;

namespace PlayerGroup
{
    public class PlayerGroupMovement : IInitializable
    {
        private PlayerGroupModel _playerGroupModel;
        private PlayerGroupView _playerGroupView;
        private IHexMouseEvents _hexMouse;
        private PlayerPathFind _playerPathFind;
        private GameTime _gameTime;

        private Tween _movementTween;
        private Queue<Vector3> _movementQueue = new Queue<Vector3>();
        private bool _hexReached;
        private bool _stopMoving;

        public PlayerGroupMovement(
            PlayerGroupModel playerGroupModel,
            PlayerGroupView playerGroupView,
            IHexMouseEvents hexMouse,
            PlayerPathFind playerPathFind,
            GameTime gameTime)
        {
            _playerGroupModel = playerGroupModel;
            _playerGroupView = playerGroupView;
            _hexMouse = hexMouse;
            _playerPathFind = playerPathFind;
            _gameTime = gameTime;
        }

        public void Initialize()
        {
            _playerGroupModel.StateChanged += GroupStateChange;
            _hexMouse.HighlightedHexClicked += PathFind;
            _hexMouse.HighlightedHexDoubleClicked += i => OnDoubleClick();
            _gameTime.Tick += () => OnGameTick().Forget();

        }
        private void GroupStateChange(PlayerState state)
        {
            if (state != PlayerState.Waiting)
            {
                //_playerPathFind.ClearPath();
            }
        }

        private async void PathFind(Vector2Int target)
        {
            if (_playerGroupModel.State == PlayerState.Waiting)
            {
                await _playerPathFind.PathFindTest(target);
            }
        }

        private void OnDoubleClick()
        {
            if (_playerGroupModel.State == PlayerState.Waiting)
            {
                StartMove().Forget();
            }
            else
            {
                _stopMoving = true;
            }
        }
        
        private async UniTask StartMove()
        {
            var path = _playerPathFind.GetPath();
            if (path.Length > 0)
            {
                Debug.Log("player Start move");
                _playerGroupModel.ChangePlayerState(PlayerState.Moving);
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
                    _playerGroupModel.SetAxialPosition(pathPoint.AxialCoordinate, true);
                    
                    if (_stopMoving)
                    {
                        break;
                    }
                }
                _playerGroupModel.ChangePlayerState(PlayerState.Waiting);
            }
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

        private async UniTask OnGameTick()
        {
            if (_playerGroupModel.State == PlayerState.Moving)
            {
                MovingToHex().Forget();
            }
            
        }
    }
}