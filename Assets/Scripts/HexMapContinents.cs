using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;


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
        await CreateContinents();
        ContinentsGenerated.Invoke();
    }

    private async UniTask  CreateContinents()
    {
        _allContinents = new List<Continent>();

        foreach (var continent in _mapSetting.ContinentsAtMap)
        {
            var newContinent = _continentFactory.Create();
            int tilesCount = _mapSetting.MapResolution().x * _mapSetting.MapResolution().y * continent.Percent / 100;
            Vector2Int continentStartPos = HexUtils.OffsetOddToAxial(_mapSetting.MapResolution().x * continent.StartPointXInPercent / 100,
                _mapSetting.MapResolution().y * continent.StartPointYInPercent / 100);

            await newContinent.CreateContinent(continentStartPos, continent.Settings, tilesCount, _allContinentsHexes);

            _allContinentsHexes.AddRange(newContinent.AllHexes);
            _allContinents.Add(newContinent);
        }
        await UniTask.Yield();
    }
}