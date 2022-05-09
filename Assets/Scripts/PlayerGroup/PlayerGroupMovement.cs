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
        private MapGeneration _mapGeneration;
        private PlayerGroupModel _playerGroupModel;
        private PlayerGroupView _playerGroupView;
        private IHexMouseEvents _hexMouse;
        private PlayerPathFind _playerPathFind;
        private GameTime _gameTime;

        private Tween _movementTween;
        private Queue<Vector3> _movementQueue = new Queue<Vector3>();
        private bool hexReached;

        public PlayerGroupMovement(
            MapGeneration mapGeneration,
            PlayerGroupModel playerGroupModel,
            PlayerGroupView playerGroupView,
            IHexMouseEvents hexMouse,
            PlayerPathFind playerPathFind,
            GameTime gameTime)
        {
            _mapGeneration = mapGeneration;
            _playerGroupModel = playerGroupModel;
            _playerGroupView = playerGroupView;
            _hexMouse = hexMouse;
            _playerPathFind = playerPathFind;
            _gameTime = gameTime;
        }

        public void Initialize()
        {
            _mapGeneration.MapGenerated += SpawnAtRandomPosition;
            _playerGroupModel.StateChanged += GroupStateChange;
            _hexMouse.HighlightedHexClicked += PathFind;
            _hexMouse.HighlightedHexDoubleClicked += i => StartMove().Forget();
            _gameTime.Tick += () => OnGameTick().Forget();

        }
        private void GroupStateChange(PlayerState state)
        {
            if (state != PlayerState.Waiting)
            {
                _playerPathFind.ClearPath();
            }
        }

        private async void PathFind(Vector2Int target)
        {
            if (_playerGroupModel.State == PlayerState.Waiting)
            {
                await _playerPathFind.PathFindTest(target);
            }
        }

        private void SpawnAtRandomPosition()
        {
            var axialPos = _mapGeneration.GetRandomStartPosition();
            _playerGroupModel.SetAxialPosition(axialPos);
            _playerGroupView.transform.position = HexUtils.CalculatePosition(axialPos);
        }

        private async UniTask StartMove()
        {
            
            var path = _playerPathFind.GetPath();
            if (_playerGroupModel.State == PlayerState.Waiting && path.Length > 0)
            {
                Debug.Log("player Start move");
                _playerGroupModel.ChangePlayerState(PlayerState.Moving);
                foreach (var pathPoint in path)
                {
                    
                    hexReached = false;
                    _playerGroupModel.SetTargetMovePosition(pathPoint.AxialCoordinate);
                    _movementQueue = HexUtils.VectorSeparation(_playerGroupView.transform.position, pathPoint.Position, pathPoint.LandTypeProperty.MovementTimeCost);
                    _gameTime.DoTick();
                    await UniTask.WaitUntil(() => hexReached);
                    _playerGroupModel.SetAxialPosition(pathPoint.AxialCoordinate);
                }
                _playerGroupModel.ChangePlayerState(PlayerState.Waiting);
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
                    hexReached = true;
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