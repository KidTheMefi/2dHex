using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Zenject;

namespace BuildingScripts.RestPlaces
{
    public class RestingPlaceHandler
    {
        private BaseBuilding.Factory _factory;
        private RestingPlace.Factory _villageFactory;
        private RestingPlaceOnMap _restingPlaceOnMap;
        private HexMapContinents _hexMapContinents;

        private List<RestingPlace> _restingPlaces = new List<RestingPlace>();
        
        public RestingPlaceHandler(BaseBuilding.Factory factory, RestingPlace.Factory villageFactory, RestingPlaceOnMap restingPlaceOnMap, HexMapContinents hexMapContinents)
        {
            _factory = factory;
            _villageFactory = villageFactory;
            _restingPlaceOnMap = restingPlaceOnMap;
            _hexMapContinents = hexMapContinents;
            
        }

        public async UniTask CreateNewRestingPlacesAsync()
        {
            await RemoveAllCentersAsync();

            foreach (var restingPlaceSetup in _restingPlaceOnMap.RestingPlaces)
            {
                if (_hexMapContinents.TryGetAvailablePositionForBuilding(restingPlaceSetup, out var positionAvailable))
                {
                    var baseBuilding = _factory.Create(new BaseBuilding.BaseBuildingSavedData(restingPlaceSetup, positionAvailable, true));
                    var restVillage = _villageFactory.Create(restingPlaceSetup, baseBuilding);
                    
                    _restingPlaces.Add(restVillage);
                }
            }
            await UniTask.Yield();
        }
        
        
        public async UniTask CreateLoadedRestingPlacesAsync(List<RestingPlace.RestingPlaceSavedData> savedRPData)
        {
            await RemoveAllCentersAsync();

            foreach (var data in savedRPData)
            {
                var baseBuilding = _factory.Create(data.baseBuildingSavedData);
                var restVillage = _villageFactory.Create(data.restingPlaceSetup, baseBuilding);
                _restingPlaces.Add(restVillage);
            }
            await UniTask.Yield();
        }
        
        private async UniTask RemoveAllCentersAsync()
        {
            foreach (var restingPlace in _restingPlaces)
            {
                restingPlace.Remove();
            }
            _restingPlaces.Clear();
            await UniTask.Yield();
        }
        
        
        public List<RestingPlace.RestingPlaceSavedData> SavedData()
        {
            return _restingPlaces.Select(rp => rp.GetRestingPlaceData()).ToList();
        }
    }
}