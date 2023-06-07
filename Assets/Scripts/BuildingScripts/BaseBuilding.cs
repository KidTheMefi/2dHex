using System;
using GameEvents.MapObjectDescriptionSignal;
using Interfaces;
using UnityEngine;
using Zenject;

namespace BuildingScripts
{
    public class BaseBuilding : MonoBehaviour, IPoolable<BaseBuilding.BaseBuildingSavedData, IMemoryPool>
    {
        public event Action PlayerAtBuilding = delegate { };
        public event Action<bool> MouseOnBuilding = delegate(bool value) { };
        [SerializeField]
        private SpriteRenderer _spriteRendererImage;
        [SerializeField]
        private SpriteRenderer _openHighlight;
        [SerializeField]
        private SpriteRenderer _buildingIcon;
 

        private IMemoryPool _pool;
        private IPlayerGroupEvents _playerGroupEvents;
        
        public BaseBuildingSetup BaseBuildingSetup { get; private set; }
        public Vector2Int AxialPosition { get; private set; }
        public bool Open { get; set; }

        
        [Inject]
        public void Construct(IPlayerGroupEvents playerGroupEvents)
        {
            _playerGroupEvents = playerGroupEvents;
        }

        private void PlayerGroupEventsOnStoppedOnPosition(Vector2Int playerPosition)
        {
            if (playerPosition == AxialPosition)
            {
                PlayerAtBuilding.Invoke();
            }
        }


        private void OnMouseEnter()
        {
            MouseOnBuilding.Invoke(true);
        }
        private void OnMouseExit()
        {
            MouseOnBuilding.Invoke(false);
        }

        public void Despawn()
        {
            _pool.Despawn(this);
        }
        
        public void OnDespawned()
        {
            gameObject.name = "Building";
            _pool = null;
            BaseBuildingSetup = null;
            Open = true;
            _playerGroupEvents.StoppedOnPosition -= PlayerGroupEventsOnStoppedOnPosition;
        }
        
        public BaseBuildingSavedData GetBaseBuildingSavedData()
        {
            return new BaseBuildingSavedData(BaseBuildingSetup, AxialPosition, Open);
        }
        
        public void OnSpawned(BaseBuildingSavedData property, IMemoryPool pool)
        {
            gameObject.name = property.baseBuildingSetup.name;
            Open = property.open;
            BaseBuildingSetup = property.baseBuildingSetup;
            _buildingIcon.sprite = property.baseBuildingSetup.iconSprite;
            _spriteRendererImage.sprite = property.baseBuildingSetup.sprite;
            _openHighlight.color = Open ? Color.green : Color.grey;
            _pool = pool;
            AxialPosition = property.axialPosition;
            transform.position = HexUtils.CalculatePosition(AxialPosition) + Vector3.back*0.1f;
            _playerGroupEvents.StoppedOnPosition += PlayerGroupEventsOnStoppedOnPosition;
        }

        public void SetOpen(bool value)
        {
            Color greenTransparent = new Color(0, 1, 0, 0.5f);
          
            Open = value;
            Debug.LogFormat($"Set open {value}");
            _openHighlight.color = Open ? greenTransparent : Color.clear;
           
        }
        public class Factory : PlaceholderFactory<BaseBuildingSavedData, BaseBuilding>
        {

        }
        
        [Serializable]
        public struct BaseBuildingSavedData
        {
            [SerializeField]
            public BaseBuildingSetup baseBuildingSetup;
            [SerializeField]
            public Vector2Int axialPosition;
            [SerializeField]
            public bool open;


            public BaseBuildingSavedData(BaseBuildingSetup baseBuildingSetup, Vector2Int axialPosition, bool open)
            {
                this.baseBuildingSetup = baseBuildingSetup;
                this.axialPosition = axialPosition;
                this.open = open;
            }
        }
    }
}