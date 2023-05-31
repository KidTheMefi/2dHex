using BuildingScripts;
using UnityEngine;

namespace DefaultNamespace.BuildingScripts.MapTargets
{
    public class LastTemple
    {
        private BaseBuilding _baseBuilding;
        
        public Vector2Int AxialPosition => _baseBuilding.AxialPosition;
        public bool Visited { get; set; }
    }
}