using System;
using UnityEngine;
using Zenject;

namespace Enemies
{
    public class EnemyFacade : MonoBehaviour, IPoolable<Vector2Int, EnemyModel.Properties, IMemoryPool>
    {
        private EnemyModel _enemyModel;
        private EnemyView _enemyView;
        private EnemyStateManager _enemyStateManager;
        private IMemoryPool _pool;
        
        public EnemyModel EnemyModel => _enemyModel;

        [Inject]
        public void Construct(
            EnemyMovement movement, 
            EnemyModel enemyModel,
            EnemyView enemyView,
            EnemyStateManager enemyStateManager
        )
        {
            _enemyModel = enemyModel;
            _enemyView = enemyView;
            _enemyStateManager = enemyStateManager;
        }

        public Vector2Int AxialPosition
        {
            get { return _enemyModel.AxialPosition; }
            set { _enemyModel.AxialPosition = value; }
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
            _enemyStateManager.Dispose();
            _pool = null;
            
        }
        public void OnSpawned(Vector2Int axialPosition, EnemyModel.Properties enemySettings, IMemoryPool pool)
        {

            AxialPosition = axialPosition;
            Position = HexUtils.CalculatePosition(axialPosition);
            _pool = pool;
            
            _enemyView.SetSprite(enemySettings.Sprite);
            _enemyView.MouseOnObject += OnMouseAtObject;
            _enemyModel.Setup(enemySettings);
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

        public class Factory : PlaceholderFactory<Vector2Int, EnemyModel.Properties, EnemyFacade>
        {

        }
    }
}