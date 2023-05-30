using System;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using PlayerGroup;
using TeamCreation;
using UnityEngine;
using Zenject;

namespace GameEvents
{
    public class EnemyAttackEvent : IInitializable, IDisposable
    {
        private PlayerGroupStateManager _playerGroupStateManager;
        readonly SignalBus _signalBus;
        private GameEventHandler _gameEventHandler;
    
    
        public EnemyAttackEvent(SignalBus signalBus, PlayerGroupStateManager playerGroupStateManager, GameEventHandler gameEventHandler)
        {
            _signalBus = signalBus;
            _playerGroupStateManager = playerGroupStateManager;
            _gameEventHandler = gameEventHandler;
            Debug.Log(_gameEventHandler.gameObject.name);
        }
    
        private void AttackBegin(GameSignals.EnemyAttackSignal enemyAttackSignal)
        {
            Debug.LogWarning($"{enemyAttackSignal.EnemyModel.EnemyProperties.EnemyName} attacked player group");
            ComputerTeam computerTeam = new ComputerTeam(enemyAttackSignal.EnemyModel.EnemyProperties.EnemyLvl);
            //enemyAttackSignal.enemyModel.
            _playerGroupStateManager.ChangeState(PlayerState.Event);
            _gameEventHandler.FightAsync(enemyAttackSignal.EnemyModel).Forget();
        }
        public void Initialize()
        {
            _signalBus.Subscribe<GameSignals.EnemyAttackSignal>(AttackBegin);
        }
        public void Dispose()
        {
            _signalBus.Unsubscribe<GameSignals.EnemyAttackSignal>(AttackBegin);
        }
    }
}
