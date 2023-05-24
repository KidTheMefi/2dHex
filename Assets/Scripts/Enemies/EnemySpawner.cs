using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Enemies
{
    [Serializable]
    public class EnemySpawner : IInitializable
    {
        private EnemyFacade.Factory _enemyFactory;
        private MapGeneration _mapGeneration;
        private EnemiesSettings _enemiesSettings;
        private HexMapContinents _hexMapContinents;

        private List<EnemyFacade> _enemiesFacade = new List<Enemies.EnemyFacade>();
        [SerializeField]
        private SavedEnemies _savedEnemies;
        public EnemySpawner(MapGeneration mapGeneration, EnemyFacade.Factory enemyFactory, EnemiesSettings enemiesSettings, HexMapContinents hexMapContinents)
        {

            _mapGeneration = mapGeneration;
            _enemyFactory = enemyFactory;
            _enemiesSettings = enemiesSettings;
            _hexMapContinents = hexMapContinents;
        }
        public void Initialize()
        {
            _mapGeneration.MapGenerated += MapGenerationOnMapGenerated;
        }
        
        
        private void MapGenerationOnMapGenerated(bool loaded)
        {
            if (loaded)
            {
                SpawnEnemyLoadPosition();
            }
            else
            {
                SpawnEnemyNew();
            }
        }
        
        private void SpawnEnemyLoadPosition()
        {
            foreach (var enemy in _enemiesFacade)       
            {
                enemy.Despawn();
            }
            _enemiesFacade.Clear();


            var enemyModels = LoadEnemiesModelsFromJson();

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
        }
        
        
        private void SpawnEnemyNew()
        {
            foreach (var enemy in _enemiesFacade)       
            {
                enemy.Despawn();
            }
            _enemiesFacade.Clear();
            
            
            for (int i = 0; i < 5; i++)
            {
                var enemySetupSettings = _enemiesSettings.Enemies[Random.Range(0, _enemiesSettings.Enemies.Count)];
                Debug.Log(_hexMapContinents.AllContinents.Count);

                foreach (var continent in _hexMapContinents.AllContinents)
                {
                    Debug.Log(continent.BiomType);
                }
                var enemyPos = _hexMapContinents.AllContinents.Find(x => x.BiomType == enemySetupSettings.Properties.BiomType).GetRandomHexAtContinent();
                var enemy = _enemyFactory.Create(enemyPos, enemySetupSettings.Properties );
                _enemiesFacade.Add( enemy);
            }
        }

        private List<EnemyModel> LoadEnemiesModelsFromJson()
        {
            string json = File.ReadAllText(Application.dataPath + "/Save/SavedEnemies.json");
            SavedEnemies enemies = JsonUtility.FromJson<SavedEnemies>(json);
            return enemies.EnemyModels;
        }
        public void SaveEnemyModelsToJson()
        {
            SavedEnemies savedEnemies = new SavedEnemies(_enemiesFacade.Select(enemy => enemy.EnemyModel).ToList());
            string json = JsonUtility.ToJson(savedEnemies, true);
            File.WriteAllText(Application.dataPath + "/Save/SavedEnemies.json", json);
            Debug.Log("Enemy Saved");
        }
        
        [Serializable]
        private struct SavedEnemies
        {
            
            [SerializeField]
            private List<EnemyModel> _enemyModels;
            public List<EnemyModel> EnemyModels => _enemyModels;
            
            public SavedEnemies(List<EnemyModel> enemies)
            {
                _enemyModels = enemies;
            }
        }
        
    }
}
