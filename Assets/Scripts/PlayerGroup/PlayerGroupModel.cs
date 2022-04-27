using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerGroup
{
    public enum PlayerState
    {
        Moving, Event, FinishedMove, Waiting
    }

    public class PlayerGroupModel
    {
        public event Action<PlayerState> StateChanged = delagate => { };
        public event Action<bool> Selected = delagate => { };
        private Vector2Int _axialPosition;
        private bool _selected;
        private PlayerState _state = PlayerState.Waiting;

        public bool IsSelected => _selected; 
        public Vector2Int AxialPosition => _axialPosition;
        public PlayerState State => _state;

        public void ChangePlayerState(PlayerState state)
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
}