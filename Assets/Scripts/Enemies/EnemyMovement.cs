using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Interfaces;
using PlayerGroup;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class EnemyMovement : IInitializable, IDisposable
    {
        private EnemyView _enemyView;
        private EnemyModel _enemyModel;
        private AStarSearch _aStarSearch;
        private IHexStorage _hexStorage;
        private PlayerGroupModel _playerGroupModel;

        private Tween _movement;

        public EnemyMovement(
            EnemyModel enemyModel,
            EnemyView enemyView,
            PlayerGroupModel playerGroupModel,
            AStarSearch aStarSearch,
            IHexStorage hexStorage)
        {
            _enemyView = enemyView;
            _enemyModel = enemyModel;
            _aStarSearch = aStarSearch;
            _hexStorage = hexStorage;
            _playerGroupModel = playerGroupModel;
        }


        public void Initialize()
        {
            Movement();
        }
        
        private async UniTask Movement()
        {
            //await UniTask.Delay(5000);
            for (int i = 0; i < 100; i++)
            {
                var target = await FindNewTarget();
                await MoveToTarget(target);
            }
        }

        private async UniTask<Vector2Int> FindNewTarget()
        {
            if (PlayerNear(out var player))
            {
                return player;
            }

            var hexes = HexUtils.GetAxialAreaAtRange(_enemyModel.AxialPosition, 6);
            var target = hexes[Random.Range(0, hexes.Count)];
            while (!_hexStorage.HexAtAxialCoordinateExist(target))
            {
                target = hexes[Random.Range(0, hexes.Count)];
            }
            await UniTask.Yield();
            return target;
        }

        private bool PlayerNear(out Vector2Int target)
        {
            var distanceToPlayer = HexUtils.AxialDistance(_playerGroupModel.AxialPosition, _enemyModel.AxialPosition);
            target = _playerGroupModel.TargetMovePosition;
            if (distanceToPlayer < _enemyModel.ViewRadius)
            {
                return true;
            }
            return false;
        }

        private async UniTask MoveToTarget(Vector2Int target)
        {
            var path = await PathFind(target);

            foreach (var hex in path)
            {
                _movement = _enemyView.transform.DOMove(hex.Position, GameTime.MovementTimeModificator * hex.LandTypeProperty.MovementTimeCost).SetEase(Ease.Linear);
                await _movement;
                _enemyModel.AxialPosition = hex.AxialCoordinate;
                if (PlayerNear(out var player) && player != target)
                {
                    return;
                }
            }
        }

        private async UniTask<List<Hex>> PathFind(Vector2Int target)
        {
            var starPathPos = _enemyModel.AxialPosition;
            var endPathPos = target;
            var pathList = await _aStarSearch.TryPathFind(starPathPos, endPathPos);

            if (pathList.Count != 0)
            {
                pathList.Reverse();
                pathList.Add(_hexStorage.GetHexAtAxialCoordinate(endPathPos));
            }


            //await UniTask.Yield();
            return pathList;
        }


        public void Dispose()
        {
            _movement.Kill();
        }
    }
}