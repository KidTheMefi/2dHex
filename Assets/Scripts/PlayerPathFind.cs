using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class PlayerPathFind : IInitializable
{
    private IHexStorage _hexStorage;
    private AStarSearch _pathFind;

    private Vector2Int _starPoint;
    private List<Vector2Int> _tutorPath;
    
    public PlayerPathFind(IHexStorage hexStorage, AStarSearch pathFind)
    {
        _hexStorage = hexStorage;
        _pathFind = pathFind;
    }
    
    public void Initialize()
    {
        foreach (var hex in _hexStorage.HexToHexView())
        {
            
        }
    }

    public void ClickOnStartPoint(HexView hexView)
    {
        if (_hexStorage.HexViewToHex().TryGetValue(hexView, out var hex))
        {
           // _starPoint = _hexStorage.
        }
    }
    
    public void SelectEndPoint(HexView hexView)
    {
        
    }
    
/*
    public void PathFindTest()
    {
        if (_tutorPath != null)
        {
            foreach (var pathPoint in _tutorPath)
            {
                _hexToHexViews[GetHexAtAxialCoordinate(pathPoint)].SetMeshRendererActive(false);
            }
        }

        foreach (var point in _pathPoints)
        {
            Destroy(point);
        }

        var starPathPos = _allContinentsHexes[Random.Range(0, _allContinentsHexes.Count)];
        var endPathPos = _allContinentsHexes[Random.Range(0, _allContinentsHexes.Count)];

        _pathPoints.Add(Instantiate(_startPathPointCircle, _hexStorage.GetHexAtAxialCoordinate(starPathPos).Position, Quaternion.identity));
        _pathPoints.Add(Instantiate(_endPathPointCircle, _hexStorage.GetHexAtAxialCoordinate(endPathPos).Position, Quaternion.identity));

        if (_pathFind.TryPathFind(starPathPos, endPathPos, out _tutorPath))
        {
            foreach (var pathPoint in _tutorPath)
            {
                var hex = _hexStorage.GetHexAtAxialCoordinate(pathPoint);

                //_pathPoints.Add(Instantiate(_pathPointCircle, hex.Position(), Quaternion.identity));

                _hexToHexViews[hex].TextAtHex(hex.LandTypeProperty.MovementCost.ToString());
                _hexToHexViews[hex].SetMeshRendererActive(true);
            }
        }
    }
*/
    
}
