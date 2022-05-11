using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Interfaces;
using PlayerGroup;
using UnityEngine;
using Zenject;

public class PlayerGroupIdle : IPlayerGroupState//, IInitializable
{
    private PlayerPathFind _playerPathFind;
    private PlayerGroupModel _playerGroupModel;
    private IHexMouseEvents _hexMouse;
    private PlayerGroupStateManager _playerGroupStateManager;
    public PlayerGroupIdle(
        PlayerPathFind playerPathFind,
        PlayerGroupModel playerGroupModel,
        IHexMouseEvents hexMouse,
        PlayerGroupStateManager playerGroupStateManager)
    {
        _playerPathFind = playerPathFind;
        _playerGroupModel = playerGroupModel;
        _hexMouse = hexMouse;
        _playerGroupStateManager = playerGroupStateManager;
    }

    private async void PathFind(Vector2Int target)
    {
        if (_playerGroupStateManager.CurrentState == PlayerState.Idle)
        {
            await _playerPathFind.PathFindTest(target);
        }
    }
    
    private void OnDoubleClick()
    {
        _playerGroupStateManager.ChangeState(PlayerState.Moving);
    }
    
    public async UniTask OnGameTick()
    {
        await UniTask.Yield();
        Debug.LogWarning("GameTick on Idle state!");
        throw new System.NotImplementedException();
    }

    public void EnterState()
    {
        Debug.Log("Entered Idle state");
        _hexMouse.HighlightedHexClicked += PathFind;
        _hexMouse.HighlightedHexDoubleClicked += OnDoubleClick;
    }
    public void ExitState()
    {
        _hexMouse.HighlightedHexClicked -= PathFind;
        _hexMouse.HighlightedHexDoubleClicked -= OnDoubleClick;
    }
    
}
