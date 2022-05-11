using System;
using Cysharp.Threading.Tasks;
using UI;
using UnityEngine;
using Zenject;

namespace PlayerGroup
{
    public class PlayerGroupRest : IPlayerGroupState
    {
        private PlayerGroupModel _playerGroupModel;
        private PlayerUIEnergy _playerUIEnergy;
        private PlayerGroupStateManager _playerGroupStateManager;
        private PlayerGroupView _playerGroupView;
        private GameTime _gameTime;

        private bool _sleep = false;

        public PlayerGroupRest(
            PlayerGroupModel playerGroupModel,
            PlayerGroupView playerGroupView,
            GameTime gameTime,
            PlayerUIEnergy playerUIEnergy,
            PlayerGroupStateManager playerGroupStateManager)
        {
            _playerGroupModel = playerGroupModel;
            _playerGroupView = playerGroupView;
            _gameTime = gameTime;
            _playerUIEnergy = playerUIEnergy;
            _playerGroupStateManager = playerGroupStateManager;

            _playerUIEnergy.SetActionAtButton(StartRest);
        }

        private void StartRest(int hours, bool sleep = false)
        {
            if (_playerGroupStateManager.CurrentState == PlayerState.Idle)
            {
                _playerGroupStateManager.ChangeState(PlayerState.Rest);
                StartRestAsync(hours, sleep).Forget();
            }
        }

        private async UniTask StartRestAsync(int hours, bool sleep = false)
        {
            _playerUIEnergy.SetRestSliderInteractable(false);
            if (sleep)
            {
                _sleep = true;
            }

            for (int i = 0; i < hours; i++)
            {
                _gameTime.DoTick();
                await UniTask.Delay(TimeSpan.FromSeconds(GameTime.MovementTimeModificator));
            }
            _playerUIEnergy.SetRestSliderInteractable(true);
            _playerGroupStateManager.ChangeState(PlayerState.Idle);
        }

        private void Rest()
        {
            if (_sleep)
            {
                _playerGroupModel.ChangeEnergy(+2);
            }
            else
            {
                _playerGroupModel.ChangeEnergy(+1);
            }
            _playerUIEnergy.AddRestSliderValue(-1);

        }

        public void EnterState()
        {
            Debug.Log("Entered Rest state");

            //_playerUIEnergy.SetAction(StartRest);
        }
        public void ExitState()
        {
            //throw new NotImplementedException();
        }
        public async UniTask OnGameTick()
        {
            Rest();
        }
    }
}