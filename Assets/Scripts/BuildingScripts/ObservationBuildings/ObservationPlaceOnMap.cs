using System.Collections.Generic;
using UnityEngine;

namespace BuildingScripts.ObservationBuildings
{
    [CreateAssetMenu (menuName = "Buildings/ObservationPlacesOnMap")]
    public class ObservationPlaceOnMap : ScriptableObject
    {
        public List<ObservationPlaceSetup> ObservationPlaceSetupList;
    }
}