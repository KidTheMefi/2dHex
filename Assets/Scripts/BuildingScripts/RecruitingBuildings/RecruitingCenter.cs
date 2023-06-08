using System;
using GameEvents;
using GameEvents.MapObjectDescriptionSignal;
using UnityEngine;
using Zenject;

namespace BuildingScripts.RecruitingBuildings
{
    public class RecruitingCenter : BuildingModel
    {
        public RecruitingCenterSetup RecruitingCenterSetup { get;}
        private SignalBus _signalBus; 

        
        public RecruitingCenter(RecruitingCenterSetup recruitingCenterSetup, BaseBuilding baseBuilding, IDescriptionSignalInvoker descriptionSignalInvoker, SignalBus signalBus) : base(baseBuilding, descriptionSignalInvoker)
        {
            RecruitingCenterSetup = recruitingCenterSetup;
            _signalBus = signalBus;
        }

        protected override void PlayerAtBuilding()
        {
            if (Open)
            {
                SetOpen(false);
                _signalBus.Fire(new GameSignals.RecruitingCenterVisitSignal() {RecruitingCenter = this} );
            }
        }
        
        

        public RecruitingCenterSavedData GetRecruitingCenterData()
        {
            return new RecruitingCenterSavedData(RecruitingCenterSetup, _baseBuilding.GetBaseBuildingSavedData());
        }
        
        protected override string GetDescription()
        {
            string description = "Allows to recruit new crew members for money: ";

            foreach (var recruit in RecruitingCenterSetup.Recruits)
            {
                description += $"\n {recruit.CharacterName} - {recruit.HireCost}";
            }
            return description;
        }
        
        public class Factory : PlaceholderFactory<RecruitingCenterSetup, BaseBuilding, RecruitingCenter>
        {

        }
        
        [Serializable]
        public struct RecruitingCenterSavedData
        {
            [SerializeField]
            public RecruitingCenterSetup recruitingCenterSetup;
            [SerializeField]
            public BaseBuilding.BaseBuildingSavedData baseBuildingSavedData;

            public RecruitingCenterSavedData(RecruitingCenterSetup recruitingCenterSetup, BaseBuilding.BaseBuildingSavedData baseBuildingSavedData)
            {
                this.recruitingCenterSetup = recruitingCenterSetup;
                this.baseBuildingSavedData = baseBuildingSavedData;
            }
        }
    }
}