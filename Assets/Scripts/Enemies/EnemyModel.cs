using System;
using UnityEngine;

namespace Enemies
{
    public class EnemyModel
    {
        private Properties _properties;
        public Properties EnemyProperties => _properties;
        
        private Vector2Int _axialPosition;
        


        public Vector2Int AxialPosition
        {
            get { return _axialPosition; }
            set { _axialPosition = value; }
        }

        public void Setup(EnemySettings enemySettings)
        {
            _properties = new Properties(enemySettings);
        }
        
        
        
        [Serializable]
        public class Properties
        {
            [SerializeField] private Sprite _sprite;
            [SerializeField] private int _maxEnergy;
            [SerializeField] private int _maxHp;
            [SerializeField] private int _attack;
            [SerializeField] private string _name;
            [SerializeField] private string _description;
            [SerializeField] private BiomType _biomSpawn;
            [SerializeField] private int _viewRadius = 6;
            

            public Sprite Sprite => _sprite;
            public int MaxEnergy => _maxEnergy;
            public int HP => _maxHp;
            public int Attack => _attack;
            public string EnemyName => _name;
            public string Description => _description; 
            public BiomType BiomType => _biomSpawn;
            public int ViewRadius => _viewRadius;
            
            public Properties(EnemySettings enemySettings)
            {
                _sprite = enemySettings.Properties.Sprite;
                _maxEnergy = enemySettings.Properties.MaxEnergy;
                _maxHp = enemySettings.Properties.HP;
                _attack =enemySettings.Properties.Attack;
                _name = enemySettings.Properties.EnemyName;
                _description = enemySettings.Properties.Description;
                _biomSpawn = enemySettings.Properties.BiomType;
                _viewRadius = enemySettings.Properties.ViewRadius;
            }
        }
    }
}