using UnityEngine;
using Zenject;

namespace PlayerGroup
{
    public class PlayerGroupSpawn : IInitializable
    {
        private MapGeneration _mapGeneration;
        private PlayerGroupModel _playerGroupModel;
        private PlayerGroupView _playerGroupView;
        
        public PlayerGroupSpawn(MapGeneration mapGeneration, PlayerGroupModel playerGroupModel, PlayerGroupView playerGroupView)
        {
            _mapGeneration = mapGeneration;
            _playerGroupModel = playerGroupModel;
            _playerGroupView = playerGroupView;
        }

        private void SpawnAtRandomPosition()
        {
            var axialPos = _mapGeneration.GetRandomStartPosition();
            _playerGroupModel.SetAxialPosition(axialPos);
            _playerGroupView.transform.position = HexUtils.CalculatePosition(axialPos);
        }
        public void Initialize()
        {
            _mapGeneration.MapGenerated += SpawnAtRandomPosition;
        }
    }
}
