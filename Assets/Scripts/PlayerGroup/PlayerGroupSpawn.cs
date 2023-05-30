using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TeamCreation;
using UnityEngine;
using Zenject;

namespace PlayerGroup
{
    public class PlayerGroupSpawn 
    {
        private PlayerGroupModel _playerGroupModel;
        private PlayerGroupView _playerGroupView;
        private PlayerGroupStateManager _playerGroupStateManager;
        
        public PlayerGroupSpawn(PlayerGroupModel playerGroupModel, PlayerGroupView playerGroupView, PlayerGroupStateManager playerGroupStateManager)
        {
            _playerGroupModel = playerGroupModel;
            _playerGroupView = playerGroupView;
            _playerGroupStateManager = playerGroupStateManager;
        }
        
        public void SpawnAtPosition(Vector2Int axialPos)
        {
            _playerGroupModel.SetAxialPosition(axialPos);
            _playerGroupView.transform.position = HexUtils.CalculatePosition(axialPos);
            _playerGroupStateManager.ChangeState(PlayerState.Idle);
        }

        public async UniTask SpawnLoadedModelAsync(PlayerGroupModel.SavedPlayer loadedModel)
        {
            _playerGroupModel.SetupModel(loadedModel);
            _playerGroupView.transform.position = HexUtils.CalculatePosition(_playerGroupModel.AxialPosition);
            _playerGroupStateManager.ChangeState(PlayerState.Idle);
            await UniTask.Yield();
        }
    }
}
