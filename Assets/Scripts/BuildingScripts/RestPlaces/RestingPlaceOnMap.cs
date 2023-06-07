using System.Collections.Generic;
using UnityEngine;

namespace BuildingScripts.RestPlaces
{
    [CreateAssetMenu (menuName = "Buildings/RestingPlacesOnMap")]
    public class RestingPlaceOnMap : ScriptableObject
    {
        public List<RestingPlaceSetup> RestingPlaces;
    }
}