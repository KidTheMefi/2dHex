using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Interfaces;
using PlayerGroup;
using UnityEngine;

namespace Enemies
{
    public class EnemyPathFind
    {
        private EnemyModel _enemyModel;
        private AStarSearch _aStarSearch;
        private IHexStorage _hexStorage;
        private PlayerGroupModel _playerGroupModel;
        private HexMapContinents _hexMapContinents;

        public EnemyPathFind(
            EnemyModel enemyModel,
            AStarSearch aStarSearch,
            IHexStorage hexStorage,
            PlayerGroupModel playerGroupModel,
            HexMapContinents hexMapContinents)
        {
            _enemyModel = enemyModel;
            _aStarSearch = aStarSearch;
            _hexStorage = hexStorage;
            _playerGroupModel = playerGroupModel;
            _hexMapContinents = hexMapContinents;
        }

        public async UniTask<Queue<Hex>> FindNewRandomPath()
        {
            Vector2Int target;
            if (_enemyModel.EnemyProperties.BiomType != BiomType.None)
            {
                target = _hexMapContinents.AllContinents.Find(c => c.BiomType == _enemyModel.EnemyProperties.BiomType).GetRandomHexAtContinent();
            }
            else
            {
                var hexes = HexUtils.GetAxialAreaAtRange(_enemyModel.AxialPosition, _enemyModel.EnemyProperties.ViewRadius);
                target = hexes[Random.Range(0, hexes.Count)];
                while (!_hexStorage.HexAtAxialCoordinateExist(target)|| target == _enemyModel.AxialPosition)
                {
                    Debug.Log("while");
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
    }
}