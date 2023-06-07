using UnityEngine;

namespace BuildingScripts.ObservationBuildings
{
    public enum ObservationType
    {
        Around, Temple
    }
    
    [CreateAssetMenu(menuName = "Buildings/ObservationPlace")]
    public class ObservationPlaceSetup  : BaseBuildingSetup
    {
        [SerializeField, Range(0, 10)]
        public int moneyCost;
        [SerializeField, Range(1, 10)]
        public int observationRadius;
        [SerializeField]
        public ObservationType observationType;

    }
}