using System;
using System.Collections.Generic;
using System.IO;
using BuildingScripts;
using BuildingScripts.RecruitingBuildings;
using Cysharp.Threading.Tasks;
using Enemies;
using GameTime;
using PlayerGroup;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class SaveHandler : MonoBehaviour
    {
        private TestButtonUI.Factory _buttonFactory;
        private List<TestButtonUI> _buttons;
        private PlayerGroupModel _playerGroupModel;
        private EnemySpawner _enemySpawner;
        private MapGeneration _mapGeneration;
        private PlayerGroupSpawn _playerGroupSpawn;
        private FieldOfView _fieldOfView;
        private InGameTime _inGameTime;
        private BuildingsHandler _buildingsHandler;

        [SerializeField]
        private SceneScriptableData _sceneScriptableData;

        [Inject]
        private void Construct(TestButtonUI.Factory buttonFactory, MapGeneration mapGeneration,
            PlayerGroupModel playerGroupModel, EnemySpawner enemySpawner,
            PlayerGroupSpawn playerGroupSpawn, FieldOfView fieldOfView,
            InGameTime inGameTime, BuildingsHandler buildingsHandler)
        {
            _buildingsHandler = buildingsHandler;
            _fieldOfView = fieldOfView;
            _buttonFactory = buttonFactory;
            _playerGroupModel = playerGroupModel;
            _enemySpawner = enemySpawner;
            _mapGeneration = mapGeneration;
            _playerGroupSpawn = playerGroupSpawn;
            _inGameTime = inGameTime;
        }

        private void Start()
        {
            InitializeAsync().Forget();
        }

        private async UniTask InitializeAsync()
        {
            _buttons = new List<TestButtonUI>();
            if (_sceneScriptableData.LoadMap)
            {
                await LoadAsync();
            }
            else
            {
                await NewGameAsync();
            }
            _buttons.Add(_buttonFactory.Create(() => SaveAsync().Forget(), "Save"));
            _buttons.Add(_buttonFactory.Create(Fog, "Fog"));
            _buttons.Add(_buttonFactory.Create(Exit, "Exit"));
            _buttons.Add(_buttonFactory.Create(SceneChanger.GetInstance().BackToStartMenu, "Start menu"));
            SceneChanger.GetInstance().LoadScreenEnabled(false);
        }

        private void Exit()
        {

             #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }
        private void Fog()
        {
            _fieldOfView.RemoveFog();
        }

        private async UniTask NewGameAsync()
        {
            SetButtonsInteractable(false);
            await _mapGeneration.GenerateNewMapAsync();
            _playerGroupSpawn.SpawnAtPosition(_mapGeneration.GetRandomStartPosition());
            _enemySpawner.SpawnEnemyNew();
            await _buildingsHandler.CreateNewBuildings();
            SetButtonsInteractable(true);
        }

        private async UniTask LoadAsync()
        {
            SetButtonsInteractable(false);
            _fieldOfView.CleanOpenedHexes();
            await _mapGeneration.LoadMapAsync();
            await LoadDataFromJsonAsync();
            SetButtonsInteractable(true);
        }
        
        private void SetButtonsInteractable(bool value)
        {
            foreach (var button in _buttons)
            {
                button.SetInteractable(value);
            }
        }

        public async UniTask SaveAsync()
        {
            SaveData saveData = new SaveData();
            saveData.CurrentTimeInTick = _inGameTime.CurrentTimeInTicks;

            SetButtonsInteractable(false);
            await _mapGeneration.SaveMapAsync();
            saveData.EnemyList = _enemySpawner.GetCurrentEnemiesModel();
            saveData.PlayerGroupModel = _playerGroupModel.GetSavedModel();
            saveData.BuildingsSaveData = _buildingsHandler.GetSaveData();

            SaveDataToJson(saveData);
            await UniTask.Yield();
            SetButtonsInteractable(true);
        }

        private void SaveDataToJson(SaveData saveData)
        {
            string json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(Application.dataPath + "/Save/SavedData.json", json);
        }

        private async UniTask LoadDataFromJsonAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            string json = File.ReadAllText(Application.dataPath + "/Save/SavedData.json");
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            _inGameTime.SetTime(saveData.CurrentTimeInTick);
            await _playerGroupSpawn.SpawnLoadedModelAsync(saveData.PlayerGroupModel);

            var enemyEncounter = _sceneScriptableData.GetEnemyModelFromAttackEvent();
            if (enemyEncounter != null)
            {

                var enemyModel = saveData.EnemyList.Find(enemy => enemy.AxialPosition == enemyEncounter.AxialPosition);
                Debug.Log($"ENEMY ENCOUNTER: {enemyModel == null} || {saveData.EnemyList.Contains(enemyModel)}");
                saveData.EnemyList.Remove(enemyModel);
            }
            await _enemySpawner.SpawnLoadedEnemyAsync(saveData.EnemyList);
            await _buildingsHandler.LoadSavedDataAsync(saveData.BuildingsSaveData);
            await UniTask.Yield();
        }

        [Serializable]
        public struct SaveData
        {
            public int CurrentTimeInTick;
            public List<EnemyModel> EnemyList;
            public PlayerGroupModel.SavedPlayer PlayerGroupModel;
            public BuildingsHandler.BuildingsSaveData BuildingsSaveData;
            
        }
    }
}