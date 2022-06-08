using System;
using UnityEngine;
using Zenject;

namespace Enemies
{
    public class EnemyFacade : MonoBehaviour, IPoolable<Vector2Int, EnemySettings, IMemoryPool>
    {
        private EnemyModel _enemyModel;
        private EnemyView _enemyView;
        private EnemyMovement _movement;
        private EnemyStateManager _enemyStateManager;
        private IMemoryPool _pool;

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
            _movement = movement;
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
            _enemyModel.Setup(enemySettings);
            _enemyStateManager.Initialize();
        }

        public class Factory : PlaceholderFactory<Vector2Int, EnemySettings, EnemyFacade>
        {

        }
    }
}