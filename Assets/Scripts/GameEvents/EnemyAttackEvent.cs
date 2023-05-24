using System;
using GameEvents;
using PlayerGroup;
using TeamCreation;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class EnemyAttackEvent : IInitializable, IDisposable
{
    private PlayerGroupModel _playerGroupModel;
    private PlayerGroupStateManager _playerGroupStateManager;
    readonly SignalBus _signalBus;
    
    
    public EnemyAttackEvent(PlayerGroupModel playerGroupModel, SignalBus signalBus, PlayerGroupStateManager playerGroupStateManager)
    {
        _playerGroupModel = playerGroupModel;
        _signalBus = signalBus;
        _playerGroupStateManager = playerGroupStateManager;
    }
    
    private void AttackBegin(GameSignals.EnemyAttackSignal enemyAttackSignal)
    {
        Debug.LogWarning($"{enemyAttackSignal.enemyModel.EnemyProperties.EnemyName} attacked player group");
        ComputerTeam computerTeam = new ComputerTeam(enemyAttackSignal.enemyModel.EnemyProperties.EnemyLvl);
        _playerGroupStateManager.ChangeState(PlayerState.Event);

        SceneManager.LoadScene("FightScene", LoadSceneMode.Single); 
        //SceneManager.LoadScene(2, LoadSceneMode.Single);
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
