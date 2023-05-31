using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    [Serializable]
    public class EnemySpawner 
    {
        private EnemyFacade.Factory _enemyFactory;
        private EnemiesSettings _enemiesSettings;
        private HexMapContinents _hexMapContinents;

        private List<EnemyFacade> _enemiesFacade = new List<EnemyFacade>();

        public EnemySpawner(EnemyFacade.Factory enemyFactory, EnemiesSettings enemiesSettings, HexMapContinents hexMapContinents)
        {
            _enemyFactory = enemyFactory;
            _enemiesSettings = enemiesSettings;
            _hexMapContinents = hexMapContinents;
        }
        
        
        public async UniTask SpawnLoadedEnemyAsync(List<EnemyModel> enemies)
        {
            foreach (var enemy in _enemiesFacade)       
            {
                enemy.Despawn();
            }
            _enemiesFacade.Clear();

            var enemyModels = enemies;

            if (enemyModels == null || enemyModels.Count == 0)
            {
                Debug.LogError("NO ENEMY MODELS");
                return;
            }
            
            foreach (var model in enemyModels)
            {
                var enemy = _enemyFactory.Create(model.AxialPosition, model.EnemyProperties );
                _enemiesFacade.Add( enemy);
            }
            await UniTask.Yield();
        }

        public void SpawnEnemyNew()
        {
            foreach (var enemy in _enemiesFacade)       
            {
                enemy.Despawn();
            }
            _enemiesFacade.Clear();

            foreach (var enemySetupSettings in _enemiesSettings.Enemies)
            {
                var randCount = Random.Range(1, 4);
                for (int i = 0; i < randCount; i++)
                {
                    var enemyPos = _hexMapContinents.AllContinents.Find(x => x.BiomType == enemySetupSettings.Properties.BiomType).GetRandomHexAtContinent();
                    var enemy = _enemyFactory.Create(enemyPos, enemySetupSettings.Properties );
                    _enemiesFacade.Add( enemy);
                }
            }
        }
        
        public  List<EnemyModel> GetCurrentEnemiesModel()
        {
            return _enemiesFacade.Select(enemy => enemy.EnemyModel).ToList();
        }
    }
}
