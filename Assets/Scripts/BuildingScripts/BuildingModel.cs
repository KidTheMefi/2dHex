using System;
using GameEvents.MapObjectDescriptionSignal;
using UnityEngine;

namespace BuildingScripts
{
    public class BuildingModel
    {
        public BaseBuilding _baseBuilding;
        public Vector2Int AxialPosition => _baseBuilding.AxialPosition;
        public bool Open => _baseBuilding.Open;
        
        private IDescriptionSignalInvoker _descriptionSignalInvoker;
        
        public BuildingModel(BaseBuilding baseBuilding, IDescriptionSignalInvoker descriptionSignalInvoker)
        {
            _baseBuilding = baseBuilding;
            _baseBuilding.PlayerAtBuilding += PlayerAtBuilding;
            _baseBuilding.MouseOnBuilding += BaseBuildingOnMouseOnBuilding;
            _descriptionSignalInvoker = descriptionSignalInvoker;
        }
        private void BaseBuildingOnMouseOnBuilding(bool value)
        {
            if (value)
            {
                var baseSetup = _baseBuilding.BaseBuildingSetup;
                var description = new MapObjectDescriptionSignal.DescriptionStruct(baseSetup.name, GetDescription(), baseSetup.sprite);
                _descriptionSignalInvoker.ShowDescriptionInvoke(description);
            }
            else
            {
                _descriptionSignalInvoker.HideDescriptionInvoke();
            }
        }

        protected virtual string GetDescription()
        {
            return _baseBuilding.BaseBuildingSetup.description;
        }

        protected virtual void PlayerAtBuilding()
        {
            Debug.Log($"Player stopped at {_baseBuilding.BaseBuildingSetup.name} [{AxialPosition}]");
        }
        
        public void Remove()
        {
            _baseBuilding.Despawn();
            _baseBuilding.PlayerAtBuilding -= PlayerAtBuilding;
            _baseBuilding.MouseOnBuilding -= BaseBuildingOnMouseOnBuilding;
        }

        public void SetOpen(bool value)
        {
            _baseBuilding.SetOpen(value);
        }
    }
}