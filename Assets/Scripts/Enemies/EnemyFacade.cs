using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Enemies
{
    public class EnemyFacade : MonoBehaviour, IPoolable<Vector2Int, IMemoryPool>, IDisposable
    {
        private EnemyModel _enemyModel;
        private EnemyView _enemyView;
        private EnemyMovement _movement;
        private IMemoryPool _pool;


        [Inject]
        public void Construct(
            EnemyMovement movement, 
            EnemyModel enemyModel,
            EnemyView enemyView
        )
        {
            _enemyModel = enemyModel;
            _enemyView = enemyView;
            _movement = movement;

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
            _pool = null;
            _movement.Dispose();
        }
        public void OnSpawned(Vector2Int axialPosition, IMemoryPool pool)
        {

            AxialPosition = axialPosition;
            Position = HexUtils.CalculatePosition(axialPosition);
            _pool = pool;
        }
        public void Dispose()
        {
            
        }
        
        public class Factory : PlaceholderFactory<Vector2Int, EnemyFacade>
        {

        }
    }
}