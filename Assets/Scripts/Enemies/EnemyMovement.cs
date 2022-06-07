using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameTime;
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
        private InGameTime _inGameTime;
        private HexMapContinents _hexMapContinents;
        
        private Continent _continentBiom;
        private Tween _movement;
        private Queue<Vector3> _movementQueue = new Queue<Vector3>();
        private Queue<Hex> _path = new Queue<Hex>();
        private Vector2Int _target;

        public EnemyMovement(
            EnemyModel enemyModel,
            EnemyView enemyView,
            PlayerGroupModel playerGroupModel,
            AStarSearch aStarSearch,
            IHexStorage hexStorage,
            InGameTime inGameTime, HexMapContinents hexMapContinents)
        {
            _enemyView = enemyView;
            _enemyModel = enemyModel;
            _aStarSearch = aStarSearch;
            _hexStorage = hexStorage;
            _inGameTime = inGameTime;
            _hexMapContinents = hexMapContinents;
            _playerGroupModel = playerGroupModel;
        }


        public void Initialize()
        {
            //_inGameTime.Tick += MovingOnTick;
            //CheckNextHex().Forget();
        }

        public void StartMovement()
        {
            if (_enemyModel.EnemyProperties.BiomType != BiomType.None)
            {
                _continentBiom = _hexMapContinents.AllContinents.Find(c => c.BiomType == _enemyModel.EnemyProperties.BiomType);
            }
            _inGameTime.Tick += MovingOnTick;
        }

        private async UniTask FindNewPath()
        {
            Vector2Int target; 
            if (_enemyModel.EnemyProperties.BiomType != BiomType.None)
            {
                target = _continentBiom.GetRandomHexAtContinent();
                Debug.Log("smth");
            }
            else
            {
                var hexes = HexUtils.GetAxialAreaAtRange(_enemyModel.AxialPosition, _enemyModel.EnemyProperties.ViewRadius);
                target = hexes[Random.Range(0, hexes.Count)];
                while (!_hexStorage.HexAtAxialCoordinateExist(target))
                {
                    target = hexes[Random.Range(0, hexes.Count)];
                }
            }
            
            _target = target;
            _path = await PathFind(_target);
        }

        private async UniTask CheckForEnemyNear()
        {
            var distanceToPlayer = HexUtils.AxialDistance(_playerGroupModel.AxialPosition, _enemyModel.AxialPosition);

            if (distanceToPlayer < _enemyModel.EnemyProperties.ViewRadius && _target != _playerGroupModel.TargetMovePosition)
            {
                _target = _playerGroupModel.TargetMovePosition;
                _path = await PathFind(_target);
            }
        }

        private async UniTask CheckNextHex()
        {
            await CheckForEnemyNear();
            
            if (_path.Count != 0)
            {
                var hex = _path.Dequeue();
                _target = hex.AxialCoordinate;
                _movementQueue = HexUtils.VectorSeparation(HexUtils.CalculatePosition(_enemyModel.AxialPosition), hex.Position, hex.LandTypeProperty.MovementTimeCost);
            }
            else
            {
                if (_enemyModel.AxialPosition == _playerGroupModel.AxialPosition)
                {
                    Debug.LogWarning("Player reached");
                    return;
                }
                //Debug.Log("_path.Count  = 0");
                await FindNewPath();
                CheckNextHex().Forget();
            }
        }

        private void MovingOnTick()
        {
            if (_movementQueue.Count != 0)
            {
                var moveTo = _movementQueue.Dequeue();
                _movement = _enemyView.transform.DOMove(moveTo, _inGameTime.TickSeconds).SetEase(Ease.Linear);
                if (_movementQueue.Count == 0)
                {
                    _enemyModel.AxialPosition = _target;
                    CheckNextHex().Forget();
                }
            }
            else
            {
                CheckNextHex().Forget();
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
            _inGameTime.Tick -= MovingOnTick;
            _movement.Kill();
            _movementQueue.Clear();
            _path.Clear();
        }
    }
}