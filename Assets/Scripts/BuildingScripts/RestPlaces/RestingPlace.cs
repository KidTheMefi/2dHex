using System;
using BuildingScripts.RestPlaces;
using DefaultNamespace;
using GameEvents.MapObjectDescriptionSignal;
using PlayerGroup;
using UnityEngine;
using Zenject;

namespace BuildingScripts
{
    public class RestingPlace : BuildingModel
    {
        public RestingPlaceSetup RestingPlaceSetup { get; private set;}
        private PlayerGroupModel _playerGroupModel;
        
        public RestingPlace(RestingPlaceSetup restingPlaceSetup, BaseBuilding baseBuilding, 
            PlayerGroupModel playerGroupModel, IDescriptionSignalInvoker descriptionSignal) : base(baseBuilding, descriptionSignal)
        {
            RestingPlaceSetup = restingPlaceSetup;
            _playerGroupModel = playerGroupModel;
        }

        protected override void PlayerAtBuilding()
        {
            base.PlayerAtBuilding();
            if (ProgressionHandler.GetInstance().PlayerMoneyValue >=  RestingPlaceSetup.moneyCost)
            {
                Debug.Log($"Rest {RestingPlaceSetup.restValue} for {RestingPlaceSetup.moneyCost} gold");
                ProgressionHandler.GetInstance().ChangeMoneyValueBy(-RestingPlaceSetup.moneyCost);
                _playerGroupModel.ChangeEnergy(RestingPlaceSetup.restValue);
                SetOpen(false);
            }
            else
            {
                Debug.Log($"Not enough gold for good rest");
            }
        }
        
        
        public RestingPlaceSavedData GetRestingPlaceData()
        {
            return new RestingPlaceSavedData(RestingPlaceSetup, _baseBuilding.GetBaseBuildingSavedData());
        }
        
        protected override string GetDescription()
        {
            string cost = RestingPlaceSetup.moneyCost == 0 ? "free" : $"{RestingPlaceSetup.moneyCost} gold";
            return $"Instantly restores {RestingPlaceSetup.restValue} rest point for {cost}.";
        }
        
        public class Factory : PlaceholderFactory<RestingPlaceSetup, BaseBuilding, RestingPlace>
        {

        }
        
        [Serializable]
        public struct RestingPlaceSavedData
        {
            [SerializeField]
            public RestingPlaceSetup restingPlaceSetup;
            [SerializeField]
            public BaseBuilding.BaseBuildingSavedData baseBuildingSavedData;
            
            public RestingPlaceSavedData(RestingPlaceSetup restingPlaceSetup, BaseBuilding.BaseBuildingSavedData baseBuildingSavedData)
            {
                this.restingPlaceSetup = restingPlaceSetup;
                this.baseBuildingSavedData = baseBuildingSavedData;
            }
        }
    }
}