using System.Collections.Generic;
using ScriptableScripts;
using UnityEngine;

namespace BuildingScripts.RecruitingBuildings
{
    [CreateAssetMenu(menuName = "Buildings/RecruitingCenter")]
    public class RecruitingCenterProperty : BaseBuildingSetup
    {
        public List<CharacterScriptable> Recruits;
    }
}