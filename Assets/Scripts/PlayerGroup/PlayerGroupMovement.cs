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

        }
        private void GroupStateChange(PlayerState state)
        {
            if (state != PlayerState.Waiting)
            {
                _playerPathFind.ClearPath();
            }
        }

        private void PathFind(Vector2Int target)
        {
            if (_playerGroupModel.State == PlayerState.Waiting)
            {
                _playerPathFind.PathFindTest(target);
            }
        }

        private void SpawnAtRandomPosition()
        {
            var axialPos = _mapGeneration.GetRandomStartPosition();
            _playerGroupModel.SetAxialPosition(axialPos);
            _playerGroupView.transform.position = HexUtils.CalculatePosition(axialPos);
        }
    }
}