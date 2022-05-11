using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerGroup
{


    public class PlayerGroupModel
    {
        public event Action<Vector2Int, int> PositionChanged = delegate(Vector2Int i, int i1) { };
        
        public event Action<int> EnergyChanged = delegate(int i) { };
        public event Action<PlayerState> StateChanged = delagate => { };
        
        private Vector2Int _axialPosition;
        private PlayerState _state = PlayerState.Idle;
        private int _visionRadius = 3;
        private int _energy = 64;
        private int _energyMax = 64;
        private int _minSleepTime = 8;
        private Vector2Int _targetMovePosition;


        public int Energy => _energy;
        public int MaxEnergy => _energyMax;
        public int MinTimeSleepHours => _minSleepTime;
        public Vector2Int TargetMovePosition => _targetMovePosition;
        public Vector2Int AxialPosition => _axialPosition;
        public PlayerState State => _state;

        public void ChangeEnergy(int energy)
        {
            _energy += energy;
            _energy = _energy > _energyMax ? _energyMax : _energy < 0 ? 0 : _energy;
            EnergyChanged.Invoke(_energy);
        }
        
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
            _axialPosition = pos;
            PositionChanged?.Invoke(pos, _visionRadius);
        }
    }
}