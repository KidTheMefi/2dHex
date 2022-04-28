using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Interfaces;
using PlayerGroup;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class PlayerPathFind : IInitializable
{
    private IHexStorage _hexStorage;
    private AStarSearch _pathFind;
    private PlayerGroupModel _playerGroupModel;
    private IHexMouseEvents _hexMouse;
    private PathPoint.Factory _pathPointFactory;
    
    private Dictionary<Vector2Int, PathPoint> _pathPointsAtCoordinates = new Dictionary<Vector2Int, PathPoint>();

    private List<Vector2Int> _path;
    public List<Vector2Int> Path => _path; 
    
    public PlayerPathFind(IHexStorage hexStorage, AStarSearch pathFind, PlayerGroupModel playerGroupModel, IHexMouseEvents hexMouse, PathPoint.Factory pathPointFactory)
    {
        _hexStorage = hexStorage;
        _pathFind = pathFind;
        _playerGroupModel = playerGroupModel;
        _hexMouse = hexMouse;
        _pathPointFactory = pathPointFactory;
    }
    
    public void Initialize()
    {
        _hexMouse.HighlightedHexClicked += PathFindTest;
    }

    public void ClearPath()
    {
        foreach (var point in _pathPointsAtCoordinates)
        {
            point.Value.Dispose();
        }
        _pathPointsAtCoordinates.Clear();
    }

    public void PathFindTest(Vector2Int target) // maybe TODO: smth with that
    {
        var starPathPos = _playerGroupModel.AxialPosition;
        var endPathPos = target;

        List<Vector2Int> unusedPoint = new List<Vector2Int>();

        if (_pathFind.TryPathFind(starPathPos, endPathPos, out var newPathCoordinates))
        {
            newPathCoordinates.Add(endPathPos);
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
            }

            foreach (var pathCoordinate in newPathCoordinates)
            {
                if (pathCoordinate != starPathPos)
                {
                    var hex = _hexStorage.GetHexAtAxialCoordinate(pathCoordinate);
                    var point = _pathPointFactory.Create();
                    point.SetPathPoint(hex.Position, hex.LandTypeProperty.MovementCost.ToString());
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