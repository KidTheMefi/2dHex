using System;
using BuildingScripts.RecruitingBuildings;
using UnityEngine;
using Zenject;

namespace BuildingScripts
{
    public class BaseBuilding : MonoBehaviour, IPoolable<BaseBuilding.BaseBuildingSavedData, IMemoryPool>
    {
        [SerializeField]
        private SpriteRenderer _spriteRendererImage;
        [SerializeField]
        private SpriteRenderer _visitHighlight;

        private IMemoryPool _pool;

        public BaseBuildingSetup BaseBuildingSetup { get; private set; }
        public Vector2Int AxialPosition { get; private set; }
        public bool Visited { get; set; }
        
        
        public void Despawn()
        {
            _pool.Despawn(this);
        }
        
        public void OnDespawned()
        {
            _pool = null;
            BaseBuildingSetup = null;
            Visited = false;
        }
        
        public BaseBuildingSavedData GetBaseBuildingSavedData()
        {
            return new BaseBuildingSavedData(BaseBuildingSetup, AxialPosition, Visited);
        }
        
        public void OnSpawned(BaseBuildingSavedData property, IMemoryPool pool)
        {
            Visited = property.visited;
            BaseBuildingSetup = property.recruitingCenterProperty;
            _spriteRendererImage.sprite = property.recruitingCenterProperty.sprite;
            _visitHighlight.color = Visited ? Color.grey : Color.green;
            _pool = pool;
            AxialPosition = property.axialPosition;
            transform.position = HexUtils.CalculatePosition(AxialPosition);
        }

        public void SetVisited(bool value)
        {
            Visited = value;
            _visitHighlight.color = Visited ? Color.grey : Color.green;
        }
        public class Factory : PlaceholderFactory<BaseBuildingSavedData, BaseBuilding>
        {

        }
        
        [Serializable]
        public struct BaseBuildingSavedData
        {
            [SerializeField]
            public BaseBuildingSetup recruitingCenterProperty;
            [SerializeField]
            public Vector2Int axialPosition;
            [SerializeField]
            public bool visited;


            public BaseBuildingSavedData(BaseBuildingSetup recruitingCenterProperty, Vector2Int axialPosition, bool visited)
            {
                this.recruitingCenterProperty = recruitingCenterProperty;
                this.axialPosition = axialPosition;
                this.visited = visited;
            }
        }
    }
}