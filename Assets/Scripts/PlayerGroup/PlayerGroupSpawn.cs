using UnityEngine;
using Zenject;

namespace PlayerGroup
{
    public class PlayerGroupSpawn : IInitializable
    {
        private MapGeneration _mapGeneration;
        private PlayerGroupModel _playerGroupModel;
        private PlayerGroupView _playerGroupView;
        private PlayerGroupStateManager _playerGroupStateManager;
        
        public PlayerGroupSpawn(MapGeneration mapGeneration, PlayerGroupModel playerGroupModel, PlayerGroupView playerGroupView, PlayerGroupStateManager playerGroupStateManager)
        {
            _mapGeneration = mapGeneration;
            _playerGroupModel = playerGroupModel;
            _playerGroupView = playerGroupView;
            _playerGroupStateManager = playerGroupStateManager;
        }

        private void SpawnAtRandomPosition()
        {
            var axialPos = _mapGeneration.GetRandomStartPosition();
            _playerGroupModel.SetAxialPosition(axialPos);
            _playerGroupView.transform.position = HexUtils.CalculatePosition(axialPos);
            _playerGroupStateManager.ChangeState(PlayerState.Idle);
        }
        public void Initialize()
        {
            _mapGeneration.MapGenerated += SpawnAtRandomPosition;
        }
    }
}
