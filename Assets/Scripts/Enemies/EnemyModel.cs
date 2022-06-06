using System;
using UnityEngine;
using Zenject;

namespace Enemies
{
    public class EnemyModel
    {
        private int _maxEnergy;
        private int _maxHp;
        private int _attack;
        private string _name;
        private string _description;

        public int MaxEnergy => _maxEnergy;
        public int HP => _maxHp;
        public int Attack => _attack;
        public string EnemyName => _name;
        public string Description => _description;

        private Vector2Int _axialPosition;
        private int _viewRadius = 6;

        public int ViewRadius => _viewRadius;


        public Vector2Int AxialPosition
        {
            get { return _axialPosition; }
            set { _axialPosition = value; }
        }
        
        public void SetupModel(EnemySettings enemySettings)
        {
        _maxEnergy = enemySettings.MaxEnergy;
        _maxHp = enemySettings.HP;
        _attack =enemySettings.Attack;
        _name = enemySettings.EnemyName;
        _description = enemySettings.Description;
        }
    }
}