using System;
using System.Collections;
using System.Collections.Generic;
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
    private HexMouseController _hexMouseController;
    private PathPoint.Factory _pathPointFactory;

    //private Vector2Int _starPoint;
    //private List<Vector2Int> _pathCoordinates = new List<Vector2Int>();
    private Dictionary<Vector2Int, PathPoint> _pathPointsAtCoordinates = new Dictionary<Vector2Int, PathPoint>();

    public PlayerPathFind(IHexStorage hexStorage, AStarSearch pathFind, PlayerGroupModel playerGroupModel, HexMouseController hexMouseController, PathPoint.Factory pathPointFactory)
    {
        _hexStorage = hexStorage;
        _pathFind = pathFind;
        _playerGroupModel = playerGroupModel;
        _hexMouseController = hexMouseController;
        _pathPointFactory = pathPointFactory;
    }

    public void PathFindEnable(bool isEnable)
    {
        if (isEnable)
        {
            _hexMouseController.HighlightedHexChanged += PathFindTest;
        }
        else
        {
            EndPathFind();
        }
    }

    private void EndPathFind()
    {
        _hexMouseController.HighlightedHexChanged -= PathFindTest;
        ClearPath();
    }

    private void ClearPath()
    {
        foreach (var point in _pathPointsAtCoordinates)
        {
            point.Value.Dispose();
        }
       _pathPointsAtCoordinates.Clear();
        //_pathCoordinates.Clear();
        //await UniTask.Yield();
    }
    
    private async void PathFindTest(Vector2Int target)
    {
        var starPathPos = _playerGroupModel.AxialPosition;
        var endPathPos = target;
        
       //var asa =  _pathPointsAtCoordinates[endPathPos];

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
    
    public void Initialize()
    {
        //throw new NotImplementedException();
    }
}