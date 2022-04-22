using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RiverGenerator : IInitializable
{
    private List<RiverView> _testRivers = new List<RiverView>();

    private HexMapContinents _mapContinents;
    private AStarSearch _pathFind;
    private IHexStorage _hexStorage;
    private RiverView.Factory _riverFactory;

    public RiverGenerator(AStarSearch pathFind, IHexStorage hexStorage, RiverView.Factory riverFactory, HexMapContinents mapContinents)
    {
        _pathFind = pathFind;
        _hexStorage = hexStorage;
        _riverFactory = riverFactory;
        _mapContinents = mapContinents;
    }
    
    public void Initialize()
    {
        _mapContinents.ContinentsGenereted += () => DrawRandomRivers(6);
    }
    
    public void DrawRandomRivers(int count)
    {
        if (_testRivers.Count != 0)
        {
            foreach (var river in _testRivers)
            {
                river.Dispose();
            }
            _testRivers.Clear();
        }
        
        foreach (var continent in _mapContinents.AllContinents)
        {
            int rivers = Random.Range(1, count);
            for (int i = 0; i < rivers; i++)
            {
                DrawRandomRiver(continent);
            }
        }

    }

    public void DrawRandomRiver(Continent continent)
    {
        List<Vector3> positions = new List<Vector3>();

        var starPathPos = continent.AllHexes[Random.Range(0, continent.AllHexes.Count)];
        var endPathPos = continent.AllHexes[Random.Range(0, continent.AllHexes.Count)];

        while (HexUtils.AxialDistance(starPathPos, endPathPos) < 6)
        {
            endPathPos = continent.AllHexes[Random.Range(0, continent.AllHexes.Count)];
        }


        if (_pathFind.TryPathFindForRiver(starPathPos, endPathPos, out var riverPositions))
        {
            var currentRiver = _riverFactory.Create();
           

            foreach (var pathPoint in riverPositions)
            {
                positions.Add(_hexStorage.GetHexAtAxialCoordinate(pathPoint).Position + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f)));
            }

            currentRiver.SetRiver(positions);
            _testRivers.Add(currentRiver);
        }
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