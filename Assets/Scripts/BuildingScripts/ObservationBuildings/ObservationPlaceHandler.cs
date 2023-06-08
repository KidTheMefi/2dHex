using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace BuildingScripts.ObservationBuildings
{
    public class ObservationPlaceHandler
    {
        private BaseBuilding.Factory _buildingFactory;
        private ObservationPlace.Factory _observationPlaceFactory;
        private ObservationPlaceOnMap _observationPlaceOnMap;
        private HexMapContinents _hexMapContinents;

        private List<ObservationPlace> _buildings = new List<ObservationPlace>();
        
        public ObservationPlaceHandler(BaseBuilding.Factory buildingFactory, ObservationPlace.Factory observationPlaceFactory, ObservationPlaceOnMap observationPlaceOnMap, HexMapContinents hexMapContinents)
        {
            _buildingFactory = buildingFactory;
            _observationPlaceFactory = observationPlaceFactory;
            _observationPlaceOnMap = observationPlaceOnMap;
            _hexMapContinents = hexMapContinents;
        }
        
        public async UniTask CreateNewPlacesAsync()
        {
            await RemoveAllBuildingsAsync();

            foreach (var placeSetup in _observationPlaceOnMap.ObservationPlaceSetupList)
            {
                if (_hexMapContinents.TryGetAvailablePositionForBuilding(placeSetup, out var positionAvailable))
                {
                    var baseBuilding = _buildingFactory.Create(new BaseBuilding.BaseBuildingSavedData(placeSetup, positionAvailable, true));
                    var observationPlace = _observationPlaceFactory.Create(placeSetup, baseBuilding);
                    
                    _buildings.Add(observationPlace);
                }
            }
            await UniTask.Yield();
        }
        
        
        public async UniTask CreateLoadedPlacesAsync(List<ObservationPlace.ObservationPlaceSavedData> savedRPData)
        {
            await RemoveAllBuildingsAsync();

            foreach (var data in savedRPData)
            {
                var baseBuilding = _buildingFactory.Create(data.baseBuildingSavedData);
                var restVillage = _observationPlaceFactory.Create(data.placeSetup, baseBuilding);
                _buildings.Add(restVillage);
            }
            await UniTask.Yield();
        }
        
        private async UniTask RemoveAllBuildingsAsync()
        {
            foreach (var building in _buildings)
            {
                building.Remove();
            }
            _buildings.Clear();
            await UniTask.Yield();
        }
        
        
        public List<ObservationPlace.ObservationPlaceSavedData> SavedData()
        {
            return _buildings.Select(op => op.GetRestingPlaceData()).ToList();
        }
    }
}