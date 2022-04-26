using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Interfaces;
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
       // _mapContinents.ContinentsGenerated += () => DrawRandomRivers(6);
    }
    
    public async UniTask DrawRandomRivers(int count)
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
        await UniTask.Yield();
    }

    private void DrawRandomRiver(Continent continent)
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
}