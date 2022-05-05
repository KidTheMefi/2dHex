using System.Collections.Generic;
using Zenject;

namespace Enemies
{
    public class EnemySpawner : IInitializable
    {
        private EnemyFacade.Factory _enemyFactory;
        private MapGeneration _mapGeneration;

        private List<EnemyFacade> _enemies = new List<Enemies.EnemyFacade>();

        public EnemySpawner(MapGeneration mapGeneration, EnemyFacade.Factory enemyFactory)
        {

            _mapGeneration = mapGeneration;
            _enemyFactory = enemyFactory;
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
            
            for (int i = 0; i < 3; i++)
            {
                var enemyPos = _mapGeneration.GetRandomStartPosition();
          
                var enemy = _enemyFactory.Create(enemyPos);
                _enemies.Add( enemy);
            }
        }
    }
}
