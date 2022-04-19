using System.Collections;
using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;

public class AStarSearch 
{
        public Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        public Dictionary<Vector2Int, int> costSoFar = new Dictionary<Vector2Int, int>();

        public AStarSearch(IWeightedGraph<Vector2Int> graph, Vector2Int start, Vector2Int goal)
        {
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

                        foreach (var next in graph.PassibleNeighbors(current))
                        {
                                int newCost = costSoFar[current]
                                        + graph.Cost(current);
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
        }
}
