using System;
using DefaultNamespace;
using GameEvents.MapObjectDescriptionSignal;
using UnityEngine;
using Zenject;

namespace BuildingScripts.ObservationBuildings
{
    public class ObservationPlace : BuildingModel
    {
        public ObservationPlaceSetup ObservationPlaceSetup { get; private set; }
        private FieldOfView _fieldOfView;
        private TempleBuildingHandler _templeBuildingHandler;

        public ObservationPlace(ObservationPlaceSetup observationPlaceSetup, BaseBuilding baseBuilding, IDescriptionSignalInvoker descriptionSignal,
            FieldOfView fieldOfView, TempleBuildingHandler templeBuildingHandler) : base(baseBuilding, descriptionSignal)
        {
            ObservationPlaceSetup = observationPlaceSetup;
            _fieldOfView = fieldOfView;
            _templeBuildingHandler = templeBuildingHandler;
        }

        protected override void PlayerAtBuilding()
        {
            base.PlayerAtBuilding();
            
            if (ProgressionHandler.GetInstance().PlayerMoneyValue >=  ObservationPlaceSetup.moneyCost)
            {
                ProgressionHandler.GetInstance().ChangeMoneyValueBy(-ObservationPlaceSetup.moneyCost);
                switch (ObservationPlaceSetup.observationType)
                {
                    case ObservationType.Around:
                        OpenMapAround();
                        break;
                    case ObservationType.Temple:
                        OpenUnvisitedTempleAtMap();
                        break;
                }
                SetOpen(false);
            }
            else
            {
                Debug.Log($"Not enough gold");
            }
        }


        protected override string GetDescription()
        {
            switch (ObservationPlaceSetup.observationType)
            {
                case ObservationType.Around:
                    return $"Opens an area within a radius of {ObservationPlaceSetup.observationRadius} hexes around this place.";
                case ObservationType.Temple:
                    return $"Opens an area within a radius of {ObservationPlaceSetup.observationRadius} hexes around random undiscovered temple for {ObservationPlaceSetup.moneyCost} gold.";
            }
            return null;
        }
        private void OpenMapAround()
        {
            _fieldOfView.DiscoverAtPoint(_baseBuilding.AxialPosition, ObservationPlaceSetup.observationRadius);
        }

        private void OpenUnvisitedTempleAtMap()
        {
            var data = _templeBuildingHandler.SavedData().Find(building => building.open);

            if (data.axialPosition != Vector2Int.zero)
            {
                _fieldOfView.DiscoverAtPoint(data.axialPosition, ObservationPlaceSetup.observationRadius);
            }
        }
        
        
        public ObservationPlaceSavedData GetRestingPlaceData()
        {
            return new ObservationPlaceSavedData(ObservationPlaceSetup, _baseBuilding.GetBaseBuildingSavedData());
        }
        
        public class Factory : PlaceholderFactory<ObservationPlaceSetup, BaseBuilding, ObservationPlace>
        {

        }
        
        [Serializable]
        public struct ObservationPlaceSavedData
        {
            [SerializeField]
            public ObservationPlaceSetup placeSetup;
            [SerializeField]
            public BaseBuilding.BaseBuildingSavedData baseBuildingSavedData;
            
            public ObservationPlaceSavedData(ObservationPlaceSetup placeSetup, BaseBuilding.BaseBuildingSavedData baseBuildingSavedData)
            {
                this.placeSetup = placeSetup;
                this.baseBuildingSavedData = baseBuildingSavedData;
            }
        }
    }
}