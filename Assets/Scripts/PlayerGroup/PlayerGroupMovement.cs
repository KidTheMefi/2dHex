using Cysharp.Threading.Tasks;
using DG.Tweening;
using Interfaces;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem.Utilities;
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

        public PlayerGroupMovement(MapGeneration mapGeneration, PlayerGroupModel playerGroupModel, PlayerGroupView playerGroupView, IHexMouseEvents hexMouse, PlayerPathFind playerPathFind)
        {
            _mapGeneration = mapGeneration;
            _playerGroupModel = playerGroupModel;
            _playerGroupView = playerGroupView;
            _hexMouse = hexMouse;
            _playerPathFind = playerPathFind;
        }

        public void Initialize()
        {
            _mapGeneration.MapGenerated += SpawnAtRandomPosition;
            _playerGroupModel.StateChanged += GroupStateChange;
            _hexMouse.HighlightedHexClicked += PathFind;
            _hexMouse.HighlightedHexDoubleClicked += i => StartMove().Forget() ;

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
            if (_playerGroupModel.State == PlayerState.Waiting && path.Length>0)
            {
                _playerGroupModel.ChangePlayerState(PlayerState.Moving);
                foreach (var pathPoint in path)
                {
                    _playerGroupModel.SetTargetMovePosition(pathPoint.AxialCoordinate);
                    await _playerGroupView.transform.DOMove(pathPoint.Position, GameTime.MovementTimeModificator * pathPoint.LandTypeProperty.MovementTimeCost).SetEase(Ease.Linear);
                    _playerGroupModel.SetAxialPosition(pathPoint.AxialCoordinate);
                }
                await UniTask.Yield();
                _playerGroupModel.ChangePlayerState(PlayerState.Waiting);
            }
        }

        /*private async UniTask Movement(Vector3 moveTo)
        {
            
        }*/
    }
}