using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameEvents;
using Interfaces;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace BuildingScripts.RecruitingBuildings
{
    public class RecruitingCentersHandler : IInitializable, IDisposable
    {
        private RecruitingCenter.Factory _factory;
        private RecruitingCentersOnMap _centersOnNewMap;
        private HexMapContinents _hexMapContinents;
        readonly SignalBus _signalBus;
        private IPlayerGroupEvents _playerGroupEvents;

        private List<RecruitingCenter> _centers = new List<RecruitingCenter>();

        public RecruitingCentersHandler(RecruitingCenter.Factory factory, RecruitingCentersOnMap centersOnNewMap,
            HexMapContinents hexMapContinents, SignalBus signalBus, IPlayerGroupEvents playerGroupEvents)
        {
            _playerGroupEvents = playerGroupEvents;
            _factory = factory;
            _centersOnNewMap = centersOnNewMap;
            _hexMapContinents = hexMapContinents;
            _signalBus = signalBus;
        }

        public List<RecruitingCenter.RecruitingCenterSavedData> SavedData()
        {
            return _centers.Select(center => center.GetRecruitingCenterData()).ToList();
        }

        public async UniTask CreateLoadedCentersAsync(List<RecruitingCenter.RecruitingCenterSavedData> savedRCData)
        {
            await RemoveAllCentersAsync();

            foreach (var data in savedRCData)
            {
                var center = _factory.Create(data);
                _centers.Add(center);
            }
            await UniTask.Yield();
        }
        
        
        public async UniTask CreateNewCentersAsync()
        {
           await RemoveAllCentersAsync();

            foreach (var centerData in _centersOnNewMap.recruitingCenterProperties)
            {
                if (_hexMapContinents.TryGetAvailablePositionForBuilding(centerData, out var positionAvailable))
                {
                    var center = _factory.Create(new RecruitingCenter.RecruitingCenterSavedData(centerData, positionAvailable, false));
                    _centers.Add(center);
                }
            }
            await UniTask.Yield();
        }
        
        private async UniTask RemoveAllCentersAsync()
        {
            foreach (var center in _centers)
            {
                center.Despawn();
            }
            _centers.Clear();
            await UniTask.Yield();
        }

        private void CheckVisit(Vector2Int axialPosition)
        {
            var center = _centers.Find(rc => rc.AxialPosition == axialPosition);

            if (center != null && !center.Visited)
            {
                center.Visited = true;
                _signalBus.Fire(new GameSignals.RecruitingCenterVisitSignal() {RecruitingCenter = center} );
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