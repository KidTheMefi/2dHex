using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Interfaces;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class RiverGenerator : IInitializable
{
    private List<RiverView> _testRivers = new List<RiverView>();

    private HexMapContinents _mapContinents;
    private AStarSearch _pathFind;
    private IHexStorage _hexStorage;
    private RiverView.Factory _riverFactory;

    public List<RiverStruct> riversPositions {get; private set; }
    

    public RiverGenerator(AStarSearch pathFind, IHexStorage hexStorage, RiverView.Factory riverFactory, HexMapContinents mapContinents)
    {
        _pathFind = pathFind;
        _hexStorage = hexStorage;
        _riverFactory = riverFactory;
        _mapContinents = mapContinents;
        riversPositions = new List<RiverStruct>();
    }
    
    public void Initialize()
    {
       // _mapContinents.ContinentsGenerated += () => DrawRandomRivers(6);
    }

    public async UniTask DrawLoadedRivers(List<RiverStruct> rivers)
    {

        foreach (var river in rivers)
        {
            var currentRiver = _riverFactory.Create();
            currentRiver.SetRiver(river.RiverPositions);
        }
        await UniTask.Yield();
        riversPositions = rivers;
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
            riversPositions.Add(new RiverStruct(positions));
            _testRivers.Add(currentRiver);
        }
    }
    
    [Serializable]
    public struct RiverStruct
    {
        [SerializeField]
        public List<Vector3> RiverPositions;
        
        public RiverStruct(List<Vector3> riverPositions)
        {
            RiverPositions = riverPositions;
        }
    }
}