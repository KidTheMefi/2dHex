using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    [Serializable]
    public class EnemyModel
    {
        [SerializeField]
        private Properties _properties;
        public Properties EnemyProperties => _properties;
        
        [SerializeField]
        private Vector2Int _axialPosition;
        
        private int _energy;
        public int Energy => _energy;

        public Vector2Int AxialPosition
        {
            get { return _axialPosition; }
            set { _axialPosition = value; }
        }

        public void Setup(Properties enemySettings)
        {
            _properties = enemySettings; //new Properties(enemySettings);
            _energy = Random.Range(1,enemySettings.MaxEnergy);
        }

        public void ChangeEnergy(int i)
        {
            _energy += i;
        }
        
        
        [Serializable]
        public struct  Properties
        {
            [SerializeField] private Sprite _sprite;
            [SerializeField] private int _maxEnergy;
            [SerializeField] private int _enemyLvl;
            [SerializeField] private string _name;
            [SerializeField] private string _description;
            [SerializeField] private BiomType _biomSpawn;
            [SerializeField] private int _viewRadius;

            public Sprite Sprite => _sprite;
            public int MaxEnergy => _maxEnergy;
            public int EnemyLvl => _enemyLvl;
            public string EnemyName => _name;
            public string Description => _description; 
            public BiomType BiomType => _biomSpawn;
            public int ViewRadius => _viewRadius;
            
        }
    }
}