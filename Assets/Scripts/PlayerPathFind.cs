
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using PlayerGroup;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class PlayerPathFind 
{
    private IHexStorage _hexStorage;
    private AStarSearch _pathFind;
    private PlayerGroupModel _playerGroupModel;
    private PathPoint.Factory _pathPointFactory;
    
    private Dictionary<Hex, PathPoint> _pathPointsAtCoordinates = new Dictionary<Hex, PathPoint>();
    
    public PlayerPathFind(IHexStorage hexStorage, AStarSearch pathFind, PlayerGroupModel playerGroupModel, PathPoint.Factory pathPointFactory)
    {
        _hexStorage = hexStorage;
        _pathFind = pathFind;
        _playerGroupModel = playerGroupModel;
        _pathPointFactory = pathPointFactory;
    }
    public void ClearPath()
    {
        foreach (var point in _pathPointsAtCoordinates)
        {
            point.Value.Dispose();
        }
        _pathPointsAtCoordinates.Clear();
    }

    public Hex[] GetPath()
    {
        return _pathPointsAtCoordinates.Keys.ToArray();
    }
    
    public void PathFindTest(Vector2Int target) // maybe TODO: smth with that
    {
        var starPathPos = _playerGroupModel.AxialPosition;
        var endPathPos = target;

        List<Hex> unusedPoint = new List<Hex>();

        if (_pathFind.TryPathFind(starPathPos, endPathPos, out var newPathCoordinates))
        {
            ClearPath();
            newPathCoordinates.Insert(0,_hexStorage.GetHexAtAxialCoordinate(endPathPos));
            newPathCoordinates.Reverse();
            /*
            
            foreach (var point in _pathPointsAtCoordinates)
            {
                if (!newPathCoordinates.Contains(point.Key))
                {
                    point.Value.Dispose();
                    unusedPoint.Add(point.Key);
                }
            }

            foreach (var point in unusedPoint)
            {
                _pathPointsAtCoordinates.Remove(point);
            }

            foreach (var coordinate in _pathPointsAtCoordinates)
            {
                newPathCoordinates.Remove(coordinate.Key);
            }*/

            foreach (var pathCoordinate in newPathCoordinates)
            {
                if (pathCoordinate.AxialCoordinate != starPathPos)
                {
                    var hex = pathCoordinate;
                    var point = _pathPointFactory.Create();
                    point.SetPathPoint(hex.Position, hex.LandTypeProperty.MovementTimeCost.ToString());
                    _pathPointsAtCoordinates.Add(pathCoordinate, point);
                }
            }
        }
        else
        {
            ClearPath();
        }
    }
}