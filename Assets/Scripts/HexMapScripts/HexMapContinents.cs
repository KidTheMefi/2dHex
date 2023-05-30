using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Interfaces;
using UnityEngine;


public class HexMapContinents
{
    public event Action ContinentsGenerated = delegate { };
    private MapSetting _mapSetting;
    private Continent.Factory _continentFactory;
    
    private List<Vector2Int> _allContinentsHexes = new List<Vector2Int>();
    public List<Vector2Int> AllContinentsHexes => _allContinentsHexes;

    private List<Continent> _allContinents;
    private IHexStorage _hexStorage;

    public List<Continent> AllContinents => _allContinents;

    public HexMapContinents(Continent.Factory continentFactory, IHexStorage hexStorage, MapSetting mapSetting)
    {
        _continentFactory = continentFactory;
        _hexStorage = hexStorage;
        _mapSetting = mapSetting;
    }

    
    public async UniTask GenerateStart()  //was Void
    {
        foreach (var hex in _hexStorage.HexToHexView())
        {
            hex.Key.SetLandTypeProperty(_mapSetting.DefaultLandTypeProperty);
        }

        _allContinentsHexes?.Clear();
        _allContinents?.Clear();
        await CreateContinentsAsync();
        ContinentsGenerated.Invoke();
    }

    private async UniTask  CreateContinentsAsync()
    {
        _allContinents = new List<Continent>();

        foreach (var continent in _mapSetting.ContinentsAtMap)
        {
            var newContinent = _continentFactory.Create();
            int tilesCount = _mapSetting.MapResolution().x * _mapSetting.MapResolution().y * continent.Percent / 100;
            Vector2Int continentStartPos = HexUtils.OffsetOddToAxial(_mapSetting.MapResolution().x * continent.StartPointXInPercent / 100,
                _mapSetting.MapResolution().y * continent.StartPointYInPercent / 100);

            await newContinent.CreateNewContinent(continentStartPos, continent.Settings, tilesCount, _allContinentsHexes);

            _allContinentsHexes.AddRange(newContinent.AllHexes);
            _allContinents.Add(newContinent);
        }
        await UniTask.Yield();
    }

    public async UniTask LoadContinentsAsync(List<Continent> savedContinents)
    {
        _allContinents = savedContinents;
        _allContinentsHexes = new List<Vector2Int>();
        foreach (var continent in _allContinents)
        {
            _allContinentsHexes.AddRange(continent.AllHexes);
        }
        await UniTask.Yield();
    }
}