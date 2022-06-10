using System;
using System.Collections.Generic;
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

    private bool _fogOn = false;
    
    private List<TestButtonUI> _buttons;

    public MapGeneration(
        HexMapContinents hexMapContinents,
        RiverGenerator riverGenerator,
        TestButtonUI.Factory buttonFactory,
        IHexStorage hexStorage)
    {
        _hexMapContinents = hexMapContinents;
        _riverGenerator = riverGenerator;
        _buttonFactory = buttonFactory;
        _hexStorage = hexStorage;
    }
    
    public void Initialize()
    {
        _buttons = new List<TestButtonUI>();
        _buttons.Add(_buttonFactory.Create(GenerateStart, "Generate map")); 
        _buttons.Add(_buttonFactory.Create(Fog, "Fog"));
    }

    private void Fog()
    {
        foreach (var hex in _hexStorage.HexToHexView().Values)
        {
            hex.ChangeFogStatus();
        }
    }
 

    private async void GenerateStart()
    {
        foreach (var button in _buttons)
        {
            button.enabled = false;
        }
        
        await _hexMapContinents.GenerateStart();
        
        foreach (var hex in _hexStorage.HexToHexView().Values)
        {
            hex.ChangeFogStatus(true);
        }
        
        await _riverGenerator.DrawRandomRivers(6);

        foreach (var button in _buttons)
        {
            button.enabled = true;
        }
        
        MapGenerated.Invoke();
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