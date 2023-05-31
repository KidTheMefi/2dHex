using System;
using System.Collections.Generic;
using BuildingScripts.RecruitingBuildings;
using Cysharp.Threading.Tasks;

namespace BuildingScripts
{
    public class BuildingsHandler
    {
        private TempleBuildingHandler _templeBuildingHandler;
        private RecruitingCentersHandler _recruitingCentersHandler;


        public BuildingsHandler(TempleBuildingHandler templeBuildingHandler, RecruitingCentersHandler recruitingCentersHandler)
        {
            _templeBuildingHandler = templeBuildingHandler;
            _recruitingCentersHandler = recruitingCentersHandler;
        }




        public async UniTask CreateNewBuildings()
        {
           await _templeBuildingHandler.CreateNewTemplesAsync();
           await _recruitingCentersHandler.CreateNewCentersAsync();
        }

        public async UniTask LoadSavedDataAsync(BuildingsSaveData savedData)
        {
           await _templeBuildingHandler.CreateLoadedTemplesAsync(savedData.templesSavedData);
           await _recruitingCentersHandler.CreateLoadedCentersAsync(savedData.recruitingCenterSavedData);
        }
        
        public BuildingsSaveData GetSaveData()
        {
            BuildingsSaveData savedData = new BuildingsSaveData();
            savedData.templesSavedData = _templeBuildingHandler.SavedData();
            savedData.recruitingCenterSavedData = _recruitingCentersHandler.SavedData();
            return savedData;
        }
        
        [Serializable]
        public struct BuildingsSaveData
        {
            public List<BaseBuilding.BaseBuildingSavedData> templesSavedData;
            public List<RecruitingCenter.RecruitingCenterSavedData> recruitingCenterSavedData;
        }
    }
}