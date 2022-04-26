using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum playerState
{
    Moving, FinishedMove, Waiting
}

public class PlayerGroupModel
{
    public event Action<playerState> StateChanged = delagate => { };
    public event Action<bool> Selected = delagate => { };
    private Vector2Int _axialPosition;
    private bool _selected;
    private playerState _state = playerState.Waiting;

    public bool IsSelected => _selected; 
    public Vector2Int AxialPosition => _axialPosition;
    public playerState State => _state;

    public void ChangePlayerState(playerState state)
    {
        // ReSharper disable once RedundantCheckBeforeAssignment
        if (_state != state)
        {
            _state = state;
            StateChanged?.Invoke(_state);
        }
    }

    public void Select(bool select)
    {
        if (_selected != select)
        {
            _selected = select;
            Selected?.Invoke(_selected);
        }
    }

    public void SetAxialPosition(Vector2Int pos)
    {
        _axialPosition = pos;
    }
}
