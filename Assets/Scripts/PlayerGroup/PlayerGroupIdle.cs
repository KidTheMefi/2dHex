using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Interfaces;
using PlayerGroup;
using UnityEngine;
using Zenject;

public class PlayerGroupIdle : IPlayerGroupState//, IInitializable
{
    private Vector2Int _pathTarget;
    private Vector2Int _unclickable = new Vector2Int(999,999);

    private PlayerPathFind _playerPathFind;
    private IHexMouseEvents _hexMouse;
    private PlayerGroupStateManager _playerGroupStateManager;
    public PlayerGroupIdle(
        PlayerPathFind playerPathFind,
        IHexMouseEvents hexMouse,
        PlayerGroupStateManager playerGroupStateManager)
    {
        _playerPathFind = playerPathFind;
        _hexMouse = hexMouse;
        _playerGroupStateManager = playerGroupStateManager;
    }

    private async UniTask PathFind(Vector2Int target)
    {
        /*if (_playerGroupStateManager.CurrentState == PlayerState.Idle)
        {*/
            await _playerPathFind.PathFindTest(target);
        /*}*/
    }
    
    private void OnDoubleClick()
    {
        if (_playerPathFind.GetPath().Length!=0)
        {
            _playerGroupStateManager.ChangeState(PlayerState.Moving);
        }
    }
    
    public UniTask OnGameTick()
    {
        Debug.LogWarning("GameTick on Idle state!");
        throw new System.NotImplementedException();
    }

    public void MouseHexClicked(Vector2Int hex)
    {
        if (hex != _pathTarget)
        {
            PathFind(hex).Forget();
            _pathTarget = hex;
        }
        else 
        {
            OnDoubleClick();
            _pathTarget = _unclickable;
        }
    }
    
    public void EnterState()
    {
        Debug.Log("Entered Idle state");
        _hexMouse.HighlightedHexClicked += MouseHexClicked;
        //_hexMouse.HighlightedHexClicked += PathFind;
        //_hexMouse.HighlightedHexDoubleClicked += OnDoubleClick;
    }
    public void ExitState()
    {
        _hexMouse.HighlightedHexClicked -= MouseHexClicked;
        //_hexMouse.HighlightedHexClicked -= PathFind;
        //_hexMouse.HighlightedHexDoubleClicked -= OnDoubleClick;
    }
    
}
