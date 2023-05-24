using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;
using Zenject;

public class MapGeneration : IInitializable, IRandomPassablePosition
{
    public event Action<bool> MapGenerated =  delegate(bool load) { };
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
        _buttons.Add(_buttonFactory.Create(() => GenerateStart().Forget(), "Generate map"));
        _buttons.Add(_buttonFactory.Create(() => LoadMap().Forget(), "Load map"));
        _buttons.Add(_buttonFactory.Create(Fog, "Fog"));

        // GenerateStart().Forget();
    }


    private void Fog()
    {
        foreach (var hex in _hexStorage.HexToHexView().Values)
        {
            hex.ChangeFogStatus();
        }
    }

    private async UniTask LoadMap()
    {
        foreach (var button in _buttons)
        {
            button.enabled = false;
        }

        HexMapSaved savedMap = HexMapSaved.GetSaveFromJson();

        if (savedMap == null || savedMap.Hexes == null)
        {
            string message = savedMap == null ? "savedMap == null" : "savedMap.HexStorageOddOffset == null";
            Debug.Log(message);
            GenerateStart().Forget();
            return;
        }

        await _hexStorage.LoadMap(savedMap);
        await _hexMapContinents.LoadContinentsAsync(savedMap.AllContinents);
        
        foreach (var hex in _hexStorage.HexToHexView().Values)
        {
            hex.ChangeFogStatus(true);
        }

        //await _riverGenerator.DrawRandomRivers(6);

        foreach (var button in _buttons)
        {
            button.enabled = true;
        }
        
        MapGenerated.Invoke(true);
    }

    private async UniTask GenerateStart()
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

        HexMapSaved savedMap = new HexMapSaved();
        savedMap.SaveContinents(_hexMapContinents.AllContinents);
        _hexStorage.SaveMap(savedMap);

        await UniTask.Delay(TimeSpan.FromSeconds(1));
        HexMapSaved.SaveToJson(savedMap);
        MapGenerated.Invoke(false);
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