using System.Collections.Generic;
using UnityEngine;

namespace BuildingScripts.RecruitingBuildings
{
    [CreateAssetMenu(menuName = "Buildings/RecruitingCentersOnMap")]
    public class RecruitingCentersOnMap : ScriptableObject
    {
        public List<RecruitingCenterSetup> recruitingCenterProperties;
    }
}