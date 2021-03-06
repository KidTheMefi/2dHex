using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Interfaces;
using Priority_Queue;
using UnityEngine;

public class AStarSearch 
{
        private IHexStorage _hexStorage;
        public AStarSearch(IHexStorage hexStorage)
        {
                _hexStorage = hexStorage;
        }
        
        private async UniTask<Dictionary<Vector2Int, Vector2Int>> PlayerPathFindAsync(Vector2Int start, Vector2Int goal)
        {
                var cameFrom = AStarSearchCameFrom(start, goal);
                await UniTask.Yield();
                return cameFrom;
        }

        private  Dictionary<Vector2Int, Vector2Int> AStarSearchCameFrom(Vector2Int start, Vector2Int goal)
        {
                Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
                Dictionary<Vector2Int, int> costSoFar = new Dictionary<Vector2Int, int>();
                var frontier = new SimplePriorityQueue<Vector2Int>(); 
               
                frontier.Enqueue(start, 0);     
                
                cameFrom[start] = start;
                costSoFar[start] = 0;
                
                while (frontier.Count > 0)
                {
                        var current = frontier.Dequeue();

                        if (current.Equals(goal))
                        {
                                break;
                        }

                        /*if (HexUtils.AxialDistance(current, goal) == 1)
                        {
                                cameFrom[goal] = current;
                                return cameFrom;
                        }*/
                        
                        foreach (var next in PassableNeighbors(current))
                        {
                                int newCost = costSoFar[current]
                                        +_hexStorage.GetHexAtAxialCoordinate(current).LandTypeProperty.MovementTimeCost;
                                if (!costSoFar.ContainsKey(next)
                                    || newCost < costSoFar[next])
                                {
                                        costSoFar[next] = newCost;
                                        float priority = newCost + HexUtils.AxialDistance(next, goal);;
                                        frontier.Enqueue(next, priority);
                                        cameFrom[next] = current;
                                }
                        }
                }
                return cameFrom;
        }
                
        public async UniTask<List<Hex>> TryPathFind(Vector2Int starPathPos, Vector2Int endPathPos)
        {
                var pathDictionary = await PlayerPathFindAsync(starPathPos, endPathPos);
                List<Hex> newPath = new List<Hex>(); 
                Vector2Int drawPathPoint = endPathPos;
                
                while (!(drawPathPoint == starPathPos))
                {
                        if (!pathDictionary.TryGetValue(drawPathPoint, out drawPathPoint))
                        {
                                //Debug.Log(starPathPos + " Unreachable from " + endPathPos);
                                break;
                        }
                        newPath.Add(_hexStorage.GetHexAtAxialCoordinate(drawPathPoint));
                }
                return  newPath;
        }
        
        public bool TryPathFindForRiver(Vector2Int starPathPos, Vector2Int endPathPos, out List<Vector2Int> path)
        {
                
                List<Vector2Int> newPath = new List<Vector2Int>(); 
                Vector2Int drawPathPoint = endPathPos;
                while (!(drawPathPoint == starPathPos))
                {
                        if (!AStarSearchCameFrom(starPathPos,endPathPos ).TryGetValue(drawPathPoint, out drawPathPoint))
                        {
                                //Debug.Log(starPathPos + " Unreachable from " + endPathPos);
                                break;
                        }
                        newPath.Add(drawPathPoint);
                        foreach (var dir in HexUtils.AxialDirectionVectors)
                        {
                                Vector2Int next = new Vector2Int(drawPathPoint.x + dir.x, drawPathPoint.y + dir.y);
                                if (_hexStorage.HexAtAxialCoordinateExist(next) && _hexStorage.GetHexAtAxialCoordinate(next).LandTypeProperty.LandType == LandType.Water)
                                {
                                        newPath.Add(next);
                                        path = newPath;
                                        return true;
                                }
                        }
                }
                path = newPath;
                if (path.Count == 0)
                {
                        return false;
                }
                return true;
        }
        
        private IEnumerable<Vector2Int> PassableNeighbors(Vector2Int axial)
        {
                foreach (var dir in HexUtils.AxialDirectionVectors)
                {
                        Vector2Int next = new Vector2Int(axial.x + dir.x, axial.y + dir.y);
                        if (_hexStorage.HexAtAxialCoordinateExist(next) && _hexStorage.GetHexAtAxialCoordinate(next).LandTypeProperty.IsPassable) 
                        {
                                yield return next;
                        }
                }
        }
}
