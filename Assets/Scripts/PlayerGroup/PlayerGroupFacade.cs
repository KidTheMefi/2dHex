using UnityEngine;
using Zenject;

namespace PlayerGroup
{
    public class PlayerGroupFacade : MonoBehaviour
    {
        private PlayerGroupModel _playerGroupModel;
        private PlayerGroupView _playerGroupView;
        private PlayerGroupStateManager _playerGroupStateManager;


        [Inject]
        public void Construct(
            PlayerGroupModel playerGroupModel,
            PlayerGroupView playerGroupView,
            PlayerGroupStateManager playerGroupStateManager)
        {
            _playerGroupModel = playerGroupModel;
            _playerGroupView = playerGroupView;
            _playerGroupStateManager = playerGroupStateManager;
        }

        public PlayerState State => _playerGroupStateManager.CurrentState;

        public Vector2Int AxialPosition
        {
            get => _playerGroupModel.AxialPosition;
            set => _playerGroupModel.SetAxialPosition(value);
        }

        public Vector2Int FutureAxialPosition
        {
            get => _playerGroupModel.TargetMovePosition;
            set => _playerGroupModel.SetTargetMovePosition(value);
        }
    }
}