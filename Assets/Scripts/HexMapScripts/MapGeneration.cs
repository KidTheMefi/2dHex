using System;
using Cysharp.Threading.Tasks;
using HexMapScripts;
using Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGeneration : IRandomPassablePosition
{
    private HexMapContinents _hexMapContinents;
    private RiverGenerator _riverGenerator;
    private IHexStorage _hexStorage;
    
    public MapGeneration(
        HexMapContinents hexMapContinents,
        RiverGenerator riverGenerator,
        IHexStorage hexStorage)
    {
        _hexMapContinents = hexMapContinents;
        _riverGenerator = riverGenerator;
        _hexStorage = hexStorage;
    }

    public async UniTask LoadMapAsync()
    {
        HexMapSaved savedMap = HexMapSaved.GetSaveFromJson();
        if (savedMap == null || savedMap.Hexes == null)
        {
            string message = savedMap == null ? "savedMap == null" : "savedMap.HexStorageOddOffset == null";
            Debug.Log(message);
            await GenerateNewMapAsync();
            return;
        }

        await _hexStorage.LoadMap(savedMap);
        await _hexMapContinents.LoadContinentsAsync(savedMap.AllContinents);
        
        Debug.Log("Fog loaded;");
        
        //await _riverGenerator.DrawRandomRivers(6);
        
        await UniTask.Yield();
    }

    public async UniTask GenerateNewMapAsync()
    {
        await _hexMapContinents.GenerateStart();

        foreach (var hex in _hexStorage.HexToHexView().Values)
        {
            hex.SetTileVisible(TileVisibility.Undiscovered);
        }
        Debug.Log("Fog generated;");
        await _riverGenerator.DrawRandomRivers(6);
        await SaveMapAsync();
        await UniTask.Yield();
    }

    public async UniTask SaveMapAsync()
    {
        HexMapSaved savedMap = new HexMapSaved();
        savedMap.SaveContinents(_hexMapContinents.AllContinents);
        _hexStorage.SaveMap(savedMap);
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        HexMapSaved.SaveToJson(savedMap);
    }
    
    public Vector2Int GetRandomStartPosition()
    {
        var random = _hexMapContinents.AllContinentsHexes[Random.Range(0, _hexMapContinents.AllContinentsHexes.Count)];
        while (!_hexStorage.GetHexAtAxialCoordinate(random).LandTypeProperty.IsPassable)
        {
            random = _hexMapContinents.AllContinentsHexes[Random.Range(0, _hexMapContinents.AllContinentsHexes.Count)];
        }
        return random;
    }
}