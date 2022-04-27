using System;
using Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;
using Zenject;

public class MapGeneration : IInitializable, IRandomPassablePosition
{
    public event Action MapGenerated = delegate { };
    private HexMapContinents _hexMapContinents;
    private RiverGenerator _riverGenerator;
    private TestButtonUI.Factory _buttonFactory;
    private IHexStorage _hexStorage;
    
    
    public MapGeneration( HexMapContinents hexMapContinents, RiverGenerator riverGenerator, TestButtonUI.Factory buttonFactory, IHexStorage hexStorage)
    {
        _hexMapContinents = hexMapContinents;
        _riverGenerator = riverGenerator;
        _buttonFactory = buttonFactory;
        _hexStorage = hexStorage;
    }



    public void Initialize()
    {
        var buttonUI = _buttonFactory.Create();
        buttonUI.Init(GenerateStart);
    }
    
    private async void GenerateStart()
    {
        await _hexMapContinents.GenerateStart();
        await _riverGenerator.DrawRandomRivers(6);
        
        MapGenerated.Invoke();
    }
    
    public Vector2Int GetRandomStartPosition()
    {
        var random = _hexMapContinents.AllContinentsHexes[ Random.Range(0,_hexMapContinents.AllContinentsHexes.Count)];
        while (!_hexStorage.GetHexAtAxialCoordinate(random).LandTypeProperty.IsPassable)
        {
            random = _hexMapContinents.AllContinentsHexes[ Random.Range(0,_hexMapContinents.AllContinentsHexes.Count)];
        }
        return random;
    }
}
