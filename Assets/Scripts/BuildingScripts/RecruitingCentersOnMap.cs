using System.Collections.Generic;
using UnityEngine;

namespace BuildingScripts
{
    [CreateAssetMenu(menuName = "Buildings/RecruitingCentersOnMap")]
    public class RecruitingCentersOnMap : ScriptableObject
    {
        public List<RecruitingCenterProperty> recruitingCenterProperties;
    }
}