using System.Collections.Generic;
using UnityEngine;

namespace BuildingScripts.MapTargets
{
    [CreateAssetMenu (menuName = "Buildings/TemplesOnMap")]
    public class TemplesOnMap : ScriptableObject
    {
        public List<BaseBuildingSetup> temples;
    }
}