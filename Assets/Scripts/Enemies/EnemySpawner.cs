using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Enemies
{
    public class EnemySpawner : IInitializable
    {
        private EnemyFacade.Factory _enemyFactory;
        private MapGeneration _mapGeneration;
        private EnemiesSettings _enemiesSettings;
        private HexMapContinents _hexMapContinents;

        private List<EnemyFacade> _enemies = new List<Enemies.EnemyFacade>();

        public EnemySpawner(MapGeneration mapGeneration, EnemyFacade.Factory enemyFactory, EnemiesSettings enemiesSettings, HexMapContinents hexMapContinents)
        {

            _mapGeneration = mapGeneration;
            _enemyFactory = enemyFactory;
            _enemiesSettings = enemiesSettings;
            _hexMapContinents = hexMapContinents;
        }
        public void Initialize()
        {
            _mapGeneration.MapGenerated += SpawnEnemy;
        }

        private void SpawnEnemy()
        {
            foreach (var enemy in _enemies)
            {
                enemy.Despawn();
            }
            _enemies.Clear();
            
            for (int i = 0; i < 5; i++)
            {
                var enemySetupSettings = _enemiesSettings.Enemies[Random.Range(0, _enemiesSettings.Enemies.Count)];
                //var enemyPos = _mapGeneration.GetRandomStartPosition();
                var enemyPos = _hexMapContinents.AllContinents.Find(x => x.BiomType == enemySetupSettings.Properties.BiomType).GetRandomHexAtContinent();
                var enemy = _enemyFactory.Create(enemyPos, enemySetupSettings );
                _enemies.Add( enemy);
            }
        }
    }
}
