using Zenject;

namespace PlayerGroup
{
    public class PlayerGroupMovement : IInitializable
    {
        private MapGeneration _mapGeneration;
        private PlayerGroupModel _playerGroupModel;
        private PlayerGroupView _playerGroupView;
        private HexMouseController _hexMouseController;
        private PlayerPathFind _playerPathFind;

        public PlayerGroupMovement(MapGeneration mapGeneration, PlayerGroupModel playerGroupModel, PlayerGroupView playerGroupView, HexMouseController hexMouseController, PlayerPathFind playerPathFind)
        {
            _mapGeneration = mapGeneration;
            _playerGroupModel = playerGroupModel;
            _playerGroupView = playerGroupView;
            _hexMouseController = hexMouseController;
            _playerPathFind = playerPathFind;
        }

        public void Initialize()
        {
            _mapGeneration.MapGenerated += SpawnAtRandomPosition;
            _playerGroupModel.Selected += EnablePathFind;
            _playerGroupModel.StateChanged += GroupStateChange;

        }

        private void GroupStateChange(PlayerState state)
        {
            switch (state)
            {
                case PlayerState.Waiting:
                    {
                        EnablePathFind(_playerGroupModel.IsSelected);
                        break;
                    }
                default:
                    return;
            }
        }

        private void EnablePathFind(bool enable)
        {
            if (_playerGroupModel.State == PlayerState.Waiting)
            {
                _playerPathFind.PathFindEnable(enable);
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