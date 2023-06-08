using System;
using Cysharp.Threading.Tasks;
using PlayerGroup;
using UnityEngine;
using Zenject;

namespace GameEvents
{
    public class RecruitingCenterVisitEvent : IInitializable, IDisposable
    {
        private PlayerGroupStateManager _playerGroupStateManager; 
        readonly SignalBus _signalBus;
        private GameEventHandler _gameEventHandler;
        
        
        public RecruitingCenterVisitEvent(SignalBus signalBus, PlayerGroupStateManager playerGroupStateManager, GameEventHandler gameEventHandler)
        {
            _signalBus = signalBus;
            _playerGroupStateManager = playerGroupStateManager;
            _gameEventHandler = gameEventHandler;
        }
    
        private void Visit(GameSignals.RecruitingCenterVisitSignal signal)
        {
            _playerGroupStateManager.ChangeState(PlayerState.Event);
            _gameEventHandler.RecruitCenterVisitAsync(signal.RecruitingCenter.RecruitingCenterSetup).Forget();
        }
        
        public void Initialize()
        {
            _signalBus.Subscribe<GameSignals.RecruitingCenterVisitSignal>(Visit);
        }
        public void Dispose()
        {
            _signalBus.Unsubscribe<GameSignals.RecruitingCenterVisitSignal>(Visit);
        }
    }
}