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
        private GameTime _gameTime;

        private Tween _movement;
        private Queue<Vector3> _movementQueue = new Queue<Vector3>();
        private Queue<Hex> _path = new Queue<Hex>();
        private Vector2Int _target;
        private bool _hexReached;

        public EnemyMovement(
            EnemyModel enemyModel,
            EnemyView enemyView,
            PlayerGroupModel playerGroupModel,
            AStarSearch aStarSearch,
            IHexStorage hexStorage,
            GameTime gameTime)
        {
            _enemyView = enemyView;
            _enemyModel = enemyModel;
            _aStarSearch = aStarSearch;
            _hexStorage = hexStorage;
            _gameTime = gameTime;
            _playerGroupModel = playerGroupModel;
        }


        public void Initialize()
        {
            _gameTime.Tick += () => MovingOnTick().Forget();
            CheckNextHex().Forget();
        }

        private async UniTask FindNewPath()
        {
            var hexes = HexUtils.GetAxialAreaAtRange(_enemyModel.AxialPosition, _enemyModel.ViewRadius);
            var target = hexes[Random.Range(0, hexes.Count)];
            while (!_hexStorage.HexAtAxialCoordinateExist(target))
            {
                target = hexes[Random.Range(0, hexes.Count)];
            }
            _target = target;
            _path = await PathFind(_target);
        }

        private async UniTask CheckForEnemyNear()
        {
            var distanceToPlayer = HexUtils.AxialDistance(_playerGroupModel.AxialPosition, _enemyModel.AxialPosition);

            if (distanceToPlayer < _enemyModel.ViewRadius && _target != _playerGroupModel.TargetMovePosition)
            {
                _target = _playerGroupModel.TargetMovePosition;
                _path = await PathFind(_target);
            }
        }

        private async  UniTask CheckNextHex()
        {
            if (_path.Count != 0)
            {
                await CheckForEnemyNear();

                var hex = _path.Dequeue();
                _target = hex.AxialCoordinate;
                _movementQueue = HexUtils.VectorSeparation(HexUtils.CalculatePosition(_enemyModel.AxialPosition), hex.Position, hex.LandTypeProperty.MovementTimeCost);
            }
            else
            {
                await FindNewPath();
                CheckNextHex().Forget();
            }
        }

        private async UniTask MovingOnTick()
        {
            if (_movementQueue.Count != 0)
            {
                bool isLast = false;
                var moveTo = _movementQueue.Dequeue();
                _movement = _enemyView.transform.DOMove(moveTo, GameTime.MovementTimeModificator).SetEase(Ease.Linear);
                if (_movementQueue.Count == 0)
                {
                    _enemyModel.AxialPosition = _target;
                    await CheckNextHex();
                    isLast = true;
                }
                await _movement;
                if (isLast)
                {
                    _hexReached = true;
                }
            }
            else
            {
                Debug.Log("player reached");
            }
        }

        private async UniTask<Queue<Hex>> PathFind(Vector2Int target)
        {
            var starPathPos = _enemyModel.AxialPosition;
            var endPathPos = target;
            var pathList = await _aStarSearch.TryPathFind(starPathPos, endPathPos);
            Queue<Hex> path = new Queue<Hex>();

            if (pathList.Count != 0)
            {
                pathList.Reverse();
                pathList.Add(_hexStorage.GetHexAtAxialCoordinate(endPathPos));
                pathList.RemoveAt(0);
            }
            foreach (var hex in pathList)
            {
                path.Enqueue(hex);
            }
            return path;
        }

        public void Dispose()
        {
            _movement.Kill();
        }
    }
}