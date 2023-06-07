using UnityEngine;

namespace BuildingScripts.RestPlaces
{
    [CreateAssetMenu(menuName = "Buildings/RestingPlace")]
    public class RestingPlaceSetup : BaseBuildingSetup
    {
        [SerializeField, Range(0,10)]
        public int moneyCost;
        [SerializeField, Range(0,100)]
        public int restValue;
    }
}