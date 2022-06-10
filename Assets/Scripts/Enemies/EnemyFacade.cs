using System;
using UnityEngine;
using Zenject;

namespace Enemies
{
    public class EnemyFacade : MonoBehaviour, IPoolable<Vector2Int, EnemySettings, IMemoryPool>
    {
        private EnemyMapModel _enemyMapModel;
        private EnemyView _enemyView;
        private EnemyMovement _movement;
        private EnemyStateManager _enemyStateManager;
        private IMemoryPool _pool;

        [Inject]
        public void Construct(
            EnemyMovement movement, 
            EnemyMapModel enemyMapModel,
            EnemyView enemyView,
            EnemyStateManager enemyStateManager
        )
        {
            _enemyMapModel = enemyMapModel;
            _enemyView = enemyView;
            _movement = movement;
            _enemyStateManager = enemyStateManager;
        }

        public Vector2Int AxialPosition
        {
            get { return _enemyMapModel.AxialPosition; }
            set { _enemyMapModel.AxialPosition = value; }
        }
        
        public Vector3 Position
        {
            get { return _enemyView.Position; }
            set { _enemyView.Position = value; }
        }
        
        public void Despawn()
        {
            _pool.Despawn(this);
        }
        

        public void OnDespawned()
        {
            _enemyView.MouseOnObject -= OnMouseAtObject;
            _movement.Dispose();
            _enemyStateManager.Dispose();
            _pool = null;
            
        }
        public void OnSpawned(Vector2Int axialPosition, EnemySettings enemySettings, IMemoryPool pool)
        {

            AxialPosition = axialPosition;
            Position = HexUtils.CalculatePosition(axialPosition);
            _pool = pool;
            
            _enemyView.SetSprite(enemySettings.Properties.Sprite);
            _enemyView.MouseOnObject += OnMouseAtObject;
            _enemyMapModel.Setup(enemySettings);
            _enemyStateManager.Initialize();
        }

        private void OnMouseAtObject(bool value)
        {
            if (value)
            {
                Debug.Log("mouse entered");
            }
            else
            {
                Debug.Log("mouse exit"); 
            }
        }

        public class Factory : PlaceholderFactory<Vector2Int, EnemySettings, EnemyFacade>
        {

        }
    }
}