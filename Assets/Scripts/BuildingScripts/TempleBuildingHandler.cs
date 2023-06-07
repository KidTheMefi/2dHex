﻿using System;
using System.Collections.Generic;
using System.Linq;
using BuildingScripts.MapTargets;
using Cysharp.Threading.Tasks;
using GameEvents.MapObjectDescriptionSignal;
using Interfaces;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace BuildingScripts
{
    //TOTAL MESS!!! TODO: REWORK ALL BUILDINGS TO ONE TEMPLATE
    public class TempleBuildingHandler : IInitializable, IDisposable
    {
        private BaseBuilding.Factory _factory;
        private TemplesOnMap _templesOnNewMap;
        private HexMapContinents _hexMapContinents;
        private IPlayerGroupEvents _playerGroupEvents;
        private IDescriptionSignalInvoker _descriptionSignalInvoker;

        private List<BaseBuilding> _temples = new List<BaseBuilding>();
        private List<BuildingModel> _templesModels = new List<BuildingModel>();

        public TempleBuildingHandler(BaseBuilding.Factory factory, TemplesOnMap templesOnNewMap,
            HexMapContinents hexMapContinents, IPlayerGroupEvents playerGroupEvents, IDescriptionSignalInvoker descriptionSignalInvoker)
        {
            _playerGroupEvents = playerGroupEvents;
            _descriptionSignalInvoker = descriptionSignalInvoker;
            _factory = factory;
            _templesOnNewMap = templesOnNewMap;
            _hexMapContinents = hexMapContinents;
        }

        public List<BaseBuilding.BaseBuildingSavedData> SavedData()
        {
            return _temples.Select(temple => temple.GetBaseBuildingSavedData()).ToList();
           
        }

        public async UniTask CreateLoadedTemplesAsync(List<BaseBuilding.BaseBuildingSavedData> savedRCData)
        {
            await RemoveAllCentersAsync();

            foreach (var data in savedRCData)
            {
                
                var baseBuilding = _factory.Create(data);
                var templeModel = new BuildingModel(baseBuilding, _descriptionSignalInvoker);
                _templesModels.Add(templeModel);
                _temples.Add(baseBuilding);
            }
            await UniTask.Yield();
        }


        public async UniTask CreateNewTemplesAsync()
        {
            await RemoveAllCentersAsync();

            foreach (var centerData in _templesOnNewMap.temples)
            {
                if (_hexMapContinents.TryGetAvailablePositionForBuilding(centerData, out var positionAvailable))
                {
                    var baseBuilding = _factory.Create(new BaseBuilding.BaseBuildingSavedData(centerData, positionAvailable, true));
                    var templeModel = new BuildingModel(baseBuilding, _descriptionSignalInvoker);
                    _templesModels.Add(templeModel);
                    _temples.Add(baseBuilding);
                }
            }
            await UniTask.Yield();
        }


        private async UniTask RemoveAllCentersAsync()
        {
            foreach (var model in _templesModels)
            {
                model.Remove();
            }
            _temples.Clear();
            _templesModels.Clear();
            await UniTask.Yield();
        }

        private void CheckVisit(Vector2Int axialPosition)
        {
            var temple = _temples.Find(rc => rc.AxialPosition == axialPosition);

            if (temple != null && temple.Open)
            {
                temple.SetOpen(false);
                CalculateVisitedTemples();
                // _signalBus.Fire(new GameSignals.RecruitingCenterVisitSignal() {RecruitingCenter = center} );
            }

        }

        private void CalculateVisitedTemples()
        {
            int visitedTemples = 0;
            foreach (var temple in _temples)
            {
                if (temple.Open)
                {
                    visitedTemples++;
                }
            }

            if (_temples.Count == visitedTemples)
            {
                Debug.Log("GAME WIN");
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
        }

        public void Initialize()
        {
            _playerGroupEvents.StoppedOnPosition += CheckVisit;
        }
        public void Dispose()
        {
            _playerGroupEvents.StoppedOnPosition -= CheckVisit;
        }
    }
}