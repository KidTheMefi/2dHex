using System;
using System.Collections.Generic;
using System.Linq;
using BuildingScripts.MapTargets;
using Cysharp.Threading.Tasks;
using Interfaces;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace BuildingScripts
{
    public class TempleBuildingHandler : IInitializable, IDisposable
    {
        private BaseBuilding.Factory _factory;
        private TemplesOnMap _templesOnNewMap;
        private HexMapContinents _hexMapContinents;
        private IPlayerGroupEvents _playerGroupEvents;

        private List<BaseBuilding> _temples = new List<BaseBuilding>();

        public TempleBuildingHandler(BaseBuilding.Factory factory, TemplesOnMap templesOnNewMap,
            HexMapContinents hexMapContinents, IPlayerGroupEvents playerGroupEvents)
        {
            _playerGroupEvents = playerGroupEvents;
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
                var center = _factory.Create(data);
                _temples.Add(center);
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
                    var center = _factory.Create(new BaseBuilding.BaseBuildingSavedData(centerData, positionAvailable, false));
                    _temples.Add(center);
                }
            }
            await UniTask.Yield();
        }
        
        
        private async UniTask RemoveAllCentersAsync()
        {
            foreach (var center in _temples)
            {
                center.Despawn();
            }
            _temples.Clear();
            await UniTask.Yield();
        }

        private void CheckVisit(Vector2Int axialPosition)
        {
            var temple = _temples.Find(rc => rc.AxialPosition == axialPosition);

            if (temple != null && !temple.Visited)
            {
                temple.SetVisited(true);
                CalculateVisitedTemples();

                // _signalBus.Fire(new GameSignals.RecruitingCenterVisitSignal() {RecruitingCenter = center} );
            }

        }

        private void CalculateVisitedTemples()
        {
            int visitedTemples = 0;
            foreach (var temple in _temples)
            {
                if (temple.Visited)
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