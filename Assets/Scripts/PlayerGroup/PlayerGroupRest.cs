using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameTime;
using UI;
using UnityEngine;
using Zenject;

public enum RestType
{
    Wait,
    WaitFor,
    SleepFor
}

namespace PlayerGroup
{
    public class PlayerGroupRest : IPlayerGroupState
    {
        private RestType _restType;

        private PlayerGroupModel _playerGroupModel;
        private PlayerUIEnergy _playerUIEnergy;
        private PlayerGroupStateManager _playerGroupStateManager;
        private PlayerGroupView _playerGroupView;
        private GameTime.InGameTime _inGameTime;

        private bool _sleep = false;
        private bool _stopRest;

        public PlayerGroupRest(
            PlayerGroupModel playerGroupModel,
            PlayerGroupView playerGroupView,
            GameTime.InGameTime inGameTime,
            PlayerUIEnergy playerUIEnergy,
            PlayerGroupStateManager playerGroupStateManager)
        {
            _playerGroupModel = playerGroupModel;
            _playerGroupView = playerGroupView;
            _inGameTime = inGameTime;
            _playerUIEnergy = playerUIEnergy;
            _playerGroupStateManager = playerGroupStateManager;

            _playerUIEnergy.SetActionAtButton(StartRest);
        }

        private void StartRest(int hours, bool sleep)
        {
            if (_playerGroupStateManager.CurrentState == PlayerState.Idle && hours > 0)
            {
                _sleep = sleep;
                
                _restType = _sleep ? RestType.SleepFor : RestType.WaitFor;

                _playerGroupStateManager.ChangeState(PlayerState.Rest);
                StartRestAsync(hours, sleep).Forget();
            }
        }

        private async UniTask StartRest()
        {
            _stopRest = false;
            _inGameTime.SetTimeState(TimeStates.Fast);
            while (!_stopRest)
            {
                _inGameTime.DoTick();
                await DOVirtual.DelayedCall(_inGameTime.TickSeconds, () => { });
            }
            _playerGroupStateManager.ChangeState(PlayerState.Idle);
        }

        private async UniTask StartRestAsync(int hours, bool sleep = false)
        {
            _inGameTime.SetTimeState(_sleep ? TimeStates.VeryFast : TimeStates.Fast);
            _stopRest = false;
            for (int i = 0; i < hours; i++)
            {

                _inGameTime.DoTick();
                _playerUIEnergy.AddRestSliderValue(-1);
                await DOVirtual.DelayedCall(_inGameTime.TickSeconds, () => { });
                if (!_sleep && _stopRest)
                {
                    Debug.Log("end rest break");
                    break;
                }
            }
            Debug.Log("end rest");
            _playerGroupStateManager.ChangeState(PlayerState.Idle);
        }

        private void Rest()
        {
            switch (_restType)
            {
                case RestType.SleepFor:
                    _playerGroupModel.ChangeEnergy(+2);
                    break;
                default:
                    _playerGroupModel.ChangeEnergy(+1);
                    break;
            }
        }

        public void EnterState()
        {
            if (_restType == RestType.Wait)
            {
               // StartRest().Forget();
            }
            //Debug.Log("Entered Rest state");
        }

        public void ExitState()
        {
            _restType = RestType.Wait;
            _inGameTime.SetTimeState(TimeStates.Default);
        }
        public async UniTask OnGameTick()
        {
            Rest();
            await UniTask.Yield();
        }
        public void OnStopPressed()
        {
            _stopRest = true;
        }
    }
}