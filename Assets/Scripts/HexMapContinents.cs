using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

public class HexMapContinents : IInitializable, IDisposable
{
    public event Action ContinentsGenereted = delegate { };
    private MapSetting _mapSetting;
    private Continent.Factory _continentFactory;

    private List<Vector2Int> _tutorPath;
    private List<Vector2Int> _allContinentsHexes = new List<Vector2Int>();
    
    private List<Continent> _allContinents;
    private IHexStorage _hexStorage;
    private TestButtonUI.Factory _buttonFactory;

    public List<Continent> AllContinents => _allContinents;

    public HexMapContinents(Continent.Factory continentFactory, IHexStorage hexStorage, MapSetting mapSetting, TestButtonUI.Factory buttonFactory)
    {
        _continentFactory = continentFactory;
        _hexStorage = hexStorage;
        _mapSetting = mapSetting;
        _buttonFactory = buttonFactory;
    }

    public async void GenerateStart()
    {
        /*if (_testRivers.Count != 0)
        {
            foreach (var river in _testRivers)
            {
                Destroy(river.gameObject);
            }
            _testRivers.Clear();
        }*/

        foreach (var hex in _hexStorage.GetAllTiles())
        {
            hex.Key.SetLandTypeProperty(_mapSetting.DefaultLandTypeProperty);
        }

        _allContinentsHexes?.Clear();
        _allContinents?.Clear();
        await CreateContinents();
        ContinentsGenereted.Invoke();
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

            /*List<Vector2Int> testPoints;
    
            foreach (var continent in _allContinents)
            {
                if (continent.LandTypes.TryGetValue(LandType.Forrest, out testPoints))
                {
                    Instantiate(_pathPointCircle, GetHexAtAxialCoordinate(testPoints[Random.Range(0, testPoints.Count)]).Position, Quaternion.identity);
                }
            }*/

    }
    
    public void Initialize()
    {
        var buttonUI = _buttonFactory.Create();
        buttonUI.Init(GenerateStart);
    }
    public void Dispose()
    {
        
    }
}