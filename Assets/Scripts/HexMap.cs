using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HexMap
{
    [Serializable]
    public class PrefabSettings
    {
        [SerializeField] private LineRenderer _riverPrefab;
        [SerializeField] private GameObject _pathPointCircle;
        [SerializeField] private GameObject _startPathPointCircle;
        [SerializeField] private GameObject _endPathPointCircle;

        public LineRenderer RiverPrefab => _riverPrefab;
        public GameObject PathPoint => _pathPointCircle;
        public GameObject StartPathPoint => _startPathPointCircle;
        public GameObject EndPathPoint => _endPathPointCircle;
    }

    [SerializeField]
    private Button _button;
    private MapSetting _mapSetting;
    private Continent.Factory _continentFactory;

    private List<LineRenderer> _testRivers = new List<LineRenderer>();
    private List<Vector2Int> _tutorPath;
    private List<Vector2Int> _allContinentsHexes = new List<Vector2Int>();
    private HashSet<GameObject> _pathPoints = new HashSet<GameObject>();

    private List<Continent> _allContinents;

    private AStarSearch _pathFind;
    private LandGeneration _landGeneration;
    private IHexStorage _hexStorage;

    public HexMap(Continent.Factory continentFactory, AStarSearch aStarSearch, LandGeneration landGeneration, IHexStorage hexStorage, MapSetting mapSetting)
    {
        _continentFactory = continentFactory;
        _pathFind = aStarSearch;
        _landGeneration = landGeneration;
        _hexStorage = hexStorage;
        _mapSetting = mapSetting;
    }
    
    public void Restart()
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
        CreateContinents();
    }


    public async void CreateContinents()
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


        /*List<Vector2Int> testPoints;

        foreach (var continent in _allContinents)
        {
            if (continent.LandTypes.TryGetValue(LandType.Forrest, out testPoints))
            {
                Instantiate(_pathPointCircle, GetHexAtAxialCoordinate(testPoints[Random.Range(0, testPoints.Count)]).Position, Quaternion.identity);
            }
        }*/

    }

    /*public void DrawRandomRivers(int count)
    {
        if (_testRivers.Count != 0)
        {
            foreach (var river in _testRivers)
            {
                Destroy(river.gameObject);
            }
            _testRivers.Clear();
        }

        Debug.Log("continents count " + _allContinents.Count);
        foreach (var continent in _allContinents)
        {
            for (int i = 0; i < count; i++)
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
            var currentRiver = Instantiate(_riverPrefab, this.transform.parent);
            currentRiver.endWidth = 0.2f;

            foreach (var pathPoint in riverPositions)
            {
                positions.Add(_hexStorage.GetHexAtAxialCoordinate(pathPoint).Position + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f)));
            }

            currentRiver.positionCount = positions.Count;
            currentRiver.SetPositions(positions.ToArray());
            _testRivers.Add(currentRiver);
        }
    }

    /*public void PathFindTest()
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