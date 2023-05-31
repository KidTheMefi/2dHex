using System;
using UnityEngine;
using Zenject;

namespace BuildingScripts.RecruitingBuildings
{
    public class RecruitingCenter : MonoBehaviour, IPoolable<RecruitingCenter.RecruitingCenterSavedData, IMemoryPool>
    {
        [SerializeField]
        private SpriteRenderer _spriteRendererImage;
        [SerializeField]
        private SpriteRenderer _visitHighlight;

        private IMemoryPool _pool;

        public RecruitingCenterProperty RecruitingCenterProperty { get; private set; }

        public Vector2Int AxialPosition { get; private set; }
        public bool Visited { get; set; }


        public void Despawn()
        {
            _pool.Despawn(this);
        }
        
        public void OnDespawned()
        {
            _pool = null;
            RecruitingCenterProperty = null;
            Visited = false;
        }
        
        public void OnSpawned(RecruitingCenterSavedData property, IMemoryPool pool)
        {   
            Visited = property.visited;
            RecruitingCenterProperty = property.recruitingCenterProperty;
            _spriteRendererImage.sprite = property.recruitingCenterProperty.sprite;
            _visitHighlight.color = Visited ? Color.grey : Color.green;
            _pool = pool;
            AxialPosition = property.axialPosition;
            transform.position = HexUtils.CalculatePosition(AxialPosition);
            
        }

        public RecruitingCenterSavedData GetRecruitingCenterData()
        {
            return new RecruitingCenterSavedData(RecruitingCenterProperty, AxialPosition, Visited);
        }
        
        
        public class Factory : PlaceholderFactory<RecruitingCenterSavedData, RecruitingCenter>
        {

        }
        
        [Serializable]
        public struct RecruitingCenterSavedData
        {
            [SerializeField]
            public RecruitingCenterProperty recruitingCenterProperty;
            [SerializeField]
            public Vector2Int axialPosition;
            [SerializeField]
            public bool visited;


            public RecruitingCenterSavedData(RecruitingCenterProperty recruitingCenterProperty, Vector2Int axialPosition, bool visited)
            {
                this.recruitingCenterProperty = recruitingCenterProperty;
                this.axialPosition = axialPosition;
                this.visited = visited;
            }
        }
    }
}