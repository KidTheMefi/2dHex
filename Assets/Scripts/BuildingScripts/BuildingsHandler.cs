using System;
using System.Collections.Generic;
using BuildingScripts.ObservationBuildings;
using BuildingScripts.RecruitingBuildings;
using BuildingScripts.RestPlaces;
using Cysharp.Threading.Tasks;

namespace BuildingScripts
{
    public class BuildingsHandler
    {
        private TempleBuildingHandler _templeBuildingHandler;
        private RecruitingCentersHandler _recruitingCentersHandler;
        private RestingPlaceHandler _restingPlaceHandler;
        private ObservationPlaceHandler _observationPlaceHandler;


        public BuildingsHandler(TempleBuildingHandler templeBuildingHandler,
            RecruitingCentersHandler recruitingCentersHandler,
            RestingPlaceHandler restingPlaceHandler,
            ObservationPlaceHandler observationPlaceHandler)
        {
            _templeBuildingHandler = templeBuildingHandler;
            _recruitingCentersHandler = recruitingCentersHandler;
            _restingPlaceHandler = restingPlaceHandler;
            _observationPlaceHandler = observationPlaceHandler;
        }

        public async UniTask CreateNewBuildings()
        {
            await _templeBuildingHandler.CreateNewTemplesAsync();
            await _recruitingCentersHandler.CreateNewCentersAsync();
            await _restingPlaceHandler.CreateNewRestingPlacesAsync();
            await _observationPlaceHandler.CreateNewPlacesAsync();
        }

        public async UniTask LoadSavedDataAsync(BuildingsSaveData savedData)
        {
            await _templeBuildingHandler.CreateLoadedTemplesAsync(savedData.templesSavedData);
            await _recruitingCentersHandler.CreateLoadedCentersAsync(savedData.recruitingCenterSavedData);
            await _restingPlaceHandler.CreateLoadedRestingPlacesAsync(savedData.restingPlacesSavedData);
            await _observationPlaceHandler.CreateLoadedPlacesAsync(savedData.observationPlaceSavedData);
        }

        public BuildingsSaveData GetSaveData()
        {
            BuildingsSaveData savedData = new BuildingsSaveData();
            savedData.templesSavedData = _templeBuildingHandler.SavedData();
            savedData.recruitingCenterSavedData = _recruitingCentersHandler.SavedData();
            savedData.restingPlacesSavedData = _restingPlaceHandler.SavedData();
            savedData.observationPlaceSavedData = _observationPlaceHandler.SavedData();
            return savedData;
        }

        [Serializable]
        public struct BuildingsSaveData
        {
            public List<BaseBuilding.BaseBuildingSavedData> templesSavedData;
            public List<RecruitingCenter.RecruitingCenterSavedData> recruitingCenterSavedData;
            public List<RestingPlace.RestingPlaceSavedData> restingPlacesSavedData;
            public List<ObservationPlace.ObservationPlaceSavedData> observationPlaceSavedData;
        }
    }
}