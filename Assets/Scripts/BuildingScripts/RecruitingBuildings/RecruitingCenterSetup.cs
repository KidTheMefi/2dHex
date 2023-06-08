using System.Collections.Generic;
using ScriptableScripts;
using UnityEngine;

namespace BuildingScripts.RecruitingBuildings
{
    [CreateAssetMenu(menuName = "Buildings/RecruitingCenter")]
    public class RecruitingCenterSetup : BaseBuildingSetup
    {
        public List<CharacterScriptable> Recruits;
    }
}