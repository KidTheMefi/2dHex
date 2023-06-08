using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Interfaces;

namespace BuildingScripts.RecruitingBuildings
{
    public class RecruitingCentersHandler
    {
        private RecruitingCenter.Factory _factory;
        private BaseBuilding.Factory _buildingFactory;
        private RecruitingCentersOnMap _centersOnNewMap;
        private HexMapContinents _hexMapContinents;

        private List<RecruitingCenter> _centers = new List<RecruitingCenter>();

        public RecruitingCentersHandler(RecruitingCenter.Factory factory, RecruitingCentersOnMap centersOnNewMap,
            HexMapContinents hexMapContinents, BaseBuilding.Factory buildingFactory)
        {
            _buildingFactory = buildingFactory;
            _factory = factory;
            _centersOnNewMap = centersOnNewMap;
            _hexMapContinents = hexMapContinents;
        }

        public async UniTask CreateNewCentersAsync()
        {
            await RemoveAllCentersAsync();

            foreach (var placeSetup in _centersOnNewMap.recruitingCenterProperties)
            {
                if (_hexMapContinents.TryGetAvailablePositionForBuilding(placeSetup, out var positionAvailable))
                {
                    var baseBuilding = _buildingFactory.Create(new BaseBuilding.BaseBuildingSavedData(placeSetup, positionAvailable, true));
                    var center = _factory.Create(placeSetup, baseBuilding);
                    _centers.Add(center);
                }
            }
            await UniTask.Yield();
        }

        public async UniTask CreateLoadedCentersAsync(List<RecruitingCenter.RecruitingCenterSavedData> savedRCData)
        {
            await RemoveAllCentersAsync();

            foreach (var data in savedRCData)
            {
                var baseBuilding = _buildingFactory.Create(data.baseBuildingSavedData);
                var center = _factory.Create(data.recruitingCenterSetup, baseBuilding);
                _centers.Add(center);
            }
            await UniTask.Yield();
        }
        
        
        private async UniTask RemoveAllCentersAsync()
        {
            foreach (var center in _centers)
            {
                center.Remove();
            }
            _centers.Clear();
            await UniTask.Yield();
        }
        
        public List<RecruitingCenter.RecruitingCenterSavedData> SavedData()
        {
            return _centers.Select(center => center.GetRecruitingCenterData()).ToList();
        }
    }
}