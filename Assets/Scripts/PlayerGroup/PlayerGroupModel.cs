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
        public event Action<Vector2Int, int> PositionChanged = delegate(Vector2Int i, int i1) { };
        public event Action<PlayerState> StateChanged = delagate => { };
        public event Action<bool> Selected = delagate => { };
        private Vector2Int _axialPosition;
        private PlayerState _state = PlayerState.Waiting;
        private int _visionRadius = 3;
        private Vector2Int _targetMovePosition;

        public Vector2Int TargetMovePosition => _targetMovePosition;
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

        public void SetTargetMovePosition(Vector2Int pos)
        {
            _targetMovePosition = pos;
        }

        public void SetAxialPosition(Vector2Int pos)
        {
            _targetMovePosition = pos;
            _axialPosition = pos;
            PositionChanged?.Invoke(pos, _visionRadius);
        }
    }
}