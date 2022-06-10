using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Interfaces;
using PlayerGroup;
using UnityEngine;

namespace Enemies
{
    public class EnemyPathFind
    {
        private EnemyMapModel _enemyMapModel;
        private AStarSearch _aStarSearch;
        private IHexStorage _hexStorage;
        private PlayerGroupModel _playerGroupModel;
        private HexMapContinents _hexMapContinents;

        public EnemyPathFind(
            EnemyMapModel enemyMapModel,
            AStarSearch aStarSearch,
            IHexStorage hexStorage,
            PlayerGroupModel playerGroupModel,
            HexMapContinents hexMapContinents)
        {
            _enemyMapModel = enemyMapModel;
            _aStarSearch = aStarSearch;
            _hexStorage = hexStorage;
            _playerGroupModel = playerGroupModel;
            _hexMapContinents = hexMapContinents;
        }

        public async UniTask<Queue<Hex>> FindNewRandomPath()
        {
            Vector2Int target;
            if (_enemyMapModel.EnemyProperties.BiomType != BiomType.None)
            {
                target = _hexMapContinents.AllContinents.Find(c => c.BiomType == _enemyMapModel.EnemyProperties.BiomType).GetRandomHexAtContinent();
            }
            else
            {
                var hexes = HexUtils.GetAxialAreaAtRange(_enemyMapModel.AxialPosition, _enemyMapModel.EnemyProperties.ViewRadius);
                target = hexes[Random.Range(0, hexes.Count)];
                while (!_hexStorage.HexAtAxialCoordinateExist(target)|| target == _enemyMapModel.AxialPosition)
                {
                    target = hexes[Random.Range(0, hexes.Count)];
                }
            }

            var path = await PathFind(target);
            return path;
        }

        public async UniTask<Queue<Hex>> FindPathToPlayer()
        {
            var path = await PathFind(_playerGroupModel.TargetMovePosition);
            return path;
        }

        private async UniTask<Queue<Hex>> PathFind(Vector2Int target)
        {
            var starPathPos = _enemyMapModel.AxialPosition;
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
    }
}