using System;
using Cysharp.Threading.Tasks;
using UI;
using Zenject;

namespace PlayerGroup
{
    public class PlayerGroupRest : IInitializable, IPlayerGroupState
    {
        private PlayerGroupModel _playerGroupModel;
        private PlayerUIEnergy _playerUIEnergy;
        private PlayerGroupView _playerGroupView;
        private GameTime _gameTime;

        private bool _sleep = false;
        
        public PlayerGroupRest(PlayerGroupModel playerGroupModel, PlayerGroupView playerGroupView, GameTime gameTime, PlayerUIEnergy playerUIEnergy)
        {
            _playerGroupModel = playerGroupModel;
            _playerGroupView = playerGroupView;
            _gameTime = gameTime;
            _playerUIEnergy = playerUIEnergy;
        }

        public void Initialize()
        {
            _gameTime.Tick += ()=>OnGameTick().Forget();
            _playerUIEnergy.SetAction(StartRestAsync);
        }

        private void StartRestAsync(int hours, bool sleep = false)
        {
            StartRest(hours,sleep).Forget();
        }
        private async UniTask StartRest(int hours, bool sleep = false)
        {
            _playerGroupModel.ChangePlayerState(PlayerState.Rest);
            if (sleep)
            {
                _sleep = true;
            }
            
            for (int i = 0; i < hours; i++)
            {
                _gameTime.DoTick();
                await UniTask.Delay(TimeSpan.FromSeconds(GameTime.MovementTimeModificator));
            }
            _playerGroupModel.ChangePlayerState(PlayerState.Idle);
        }
        
        private void Rest()
        {
            if (_playerGroupModel.State== PlayerState.Rest)
            {
                if (_sleep)
                {
                    _playerGroupModel.ChangeEnergy(+2);
                }
                else
                {
                    _playerGroupModel.ChangeEnergy(+1);
                }
            }
        }

        public void EnterState()
        {
            throw new NotImplementedException();
        }
        public void ExitState()
        {
            throw new NotImplementedException();
        }
        public async UniTask OnGameTick()
        {
            Rest();
        }
    }
}
