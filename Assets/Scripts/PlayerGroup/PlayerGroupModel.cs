using System;
using System.IO;
using TeamCreation;
using UnityEngine;

namespace PlayerGroup
{
    [Serializable]
    public class PlayerGroupModel
    {
        public event Action PositionChanged = delegate() { };
        public event Action<int> EnergyChanged = delegate(int i) { };

        [SerializeField]
        private PlayerSettings _playerSettings;
        [SerializeField]
        private Vector2Int _axialPosition;
        private PlayerState _state = PlayerState.Idle;
        private Vector2Int _targetMovePosition;

        public SavedTeam SavedTeam => _playerSettings.SavedTeam;

        public int Energy => _playerSettings.Energy;
        public int MaxEnergy => _playerSettings.MaxEnergy;
        public int MinTimeSleepHours => _playerSettings.MinSleepTime;
        public int VisionRadius => _playerSettings.VisionRadius;
        public Vector2Int TargetMovePosition => _targetMovePosition;
        public Vector2Int AxialPosition => _axialPosition;
        public PlayerState State => _state;
        

        public PlayerGroupModel(PlayerSettings playerSettings)
        {
            _playerSettings = playerSettings;
        }

        public void LoadModelFromJson()
        {
            string json = File.ReadAllText(Application.dataPath + "/Save/SavedPlayerModel.json");
            PlayerGroupModel playerSaved = JsonUtility.FromJson<PlayerGroupModel>(json);

            _playerSettings = playerSaved._playerSettings;
            _axialPosition = playerSaved.AxialPosition;
            EnergyChanged.Invoke(Energy);
        }

        public void SaveModelToJson()
        {
            string json = JsonUtility.ToJson(this, true);
            File.WriteAllText(Application.dataPath + "/Save/SavedPlayerModel.json", json);
        }

        public void ChangeEnergy(int energy)
        {
            var newEnergy = Energy + energy;
            newEnergy = newEnergy > MaxEnergy ? MaxEnergy : newEnergy < 0 ? 0 : newEnergy;
            _playerSettings.SetEnergy(newEnergy);
            EnergyChanged.Invoke(Energy);
        }

        public void SetTargetMovePosition(Vector2Int pos)
        {
            _targetMovePosition = pos;
        }

        public void SetAxialPosition(Vector2Int pos)
        {
            _axialPosition = pos;
            PositionChanged?.Invoke();
        }

        [Serializable]
        public struct PlayerSettings
        {
            [SerializeField, Range(2,5)]
            private int _visionRadius;
            [SerializeField, Range(32,128)]
            private int _energy ;
            [SerializeField, Range(32,128)]
            private int _energyMax;
            [SerializeField, Range(4,12)]
            private int _minSleepTime;
            [SerializeField]
            private SavedTeam _savedTeam;

            public int VisionRadius => _visionRadius;
            public int Energy => _energy > _energyMax ? _energyMax : _energy;
            public int MaxEnergy => _energyMax;
            public int MinSleepTime => _minSleepTime;
            public SavedTeam SavedTeam => _savedTeam;

            public void SetEnergy(int value)
            {
                _energy = value;
            }
        }
    }
}