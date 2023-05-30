using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Enemies;
using PlayerGroup;
using UnityEngine;

public class EnemyEncounterEvent
{
    private PlayerGroupStateManager _playerGroupStateManager;
    private PlayerGroupModel _playerGroupModel;
    
    public EnemyEncounterEvent(PlayerGroupStateManager playerGroupStateManager, PlayerGroupModel playerGroupModel)
    {
        _playerGroupStateManager = playerGroupStateManager;
        _playerGroupModel = playerGroupModel;
    }

    public void EventStart(EnemyModel enemyModel)
    {
        _playerGroupStateManager.ChangeState(PlayerState.Event);
        Debug.Log("EnemyEncounterEvent");

        DOVirtual.DelayedCall(0.5f, () => { });
        EventEnd();
    }

    private void EventEnd()
    {
        Debug.Log("EnemyEncounterEvent ended");
        _playerGroupStateManager.ChangeState(PlayerState.Idle);
    }
}
