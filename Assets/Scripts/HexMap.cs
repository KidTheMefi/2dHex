using System;
using System.Collections;
using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class HexMap : MonoBehaviour, IHexStorage
{
    [SerializeField] private HexView HexPrefab;
    [SerializeField] private GameObject _pathPointCircle;
    [SerializeField] private LineRenderer _riverPrefab;
    [SerializeField] private GameObject _startPathPointCircle;
    [SerializeField] private GameObject _endPathPointCircle;
    
    [SerializeField] private LandTypeProperty _DefaultlandTypeProperty;

    [SerializeField] private ContinentSettings _continentSettings;
    [SerializeField] private ContinentSettings _secondContinentSettings;
    [SerializeField] private ContinentSettings _thirdContinentSettings;
    
    [SerializeField] private Vector2Int MapResolution;

    private GameObject _centerPoint;
    private List<LineRenderer> _testRivers = new List<LineRenderer>();
    private List<Vector2Int> _tutorPath;
    private List<Vector2Int> _allContinentsHexes = new List<Vector2Int>();

    private HashSet<GameObject> _pathPoints = new HashSet<GameObject>();
    
    private Hex[,] _hexStorageOddOffset;
    private Dictionary<Hex, HexView> _hexToHexViews;

    private AStarSearch _pathFind;
    private LandGeneration _landGeneration;
    private Continent _firstTestContinent;
    private Continent _secondTestContinent;
    private Continent _thirdTestContinent;
    
    void Start()
    {
        
        _pathFind = new AStarSearch(this);
        _landGeneration = new LandGeneration(this);
        _firstTestContinent = new Continent(this, _landGeneration);
        _secondTestContinent = new Continent(this, _landGeneration);
        _thirdTestContinent = new Continent(this, _landGeneration);
        GenerateMapGrid();
    }
    
    private void GenerateMapGrid() //more like generate hex grid
    {
        _hexStorageOddOffset = new Hex[MapResolution.x, MapResolution.y];
        _hexToHexViews = new Dictionary<Hex, HexView>();

        for (int column = 0; column < MapResolution.x; column++)
        {
            for (int row = 0; row < MapResolution.y; row++)
            {
                Vector2Int oddCoordinate = new Vector2Int(column, row);
                Hex hex = new Hex(HexUtils.OffsetOddToAxial(oddCoordinate), oddCoordinate);

                HexView hexTileView = Instantiate(
                    HexPrefab,
                    hex.Position(),
                    Quaternion.identity,
                    this.transform);
                
                hexTileView.gameObject.name = oddCoordinate.ToString();
                hexTileView.TextAtHex(HexUtils.OffsetOddToAxial(oddCoordinate).ToString());
                
                hex.SetLandTypeProperty(_DefaultlandTypeProperty);
                _hexStorageOddOffset[column, row] = hex;
                _hexToHexViews.Add(hex, hexTileView);
            }
        }

        foreach (var hex in _hexStorageOddOffset)
        {
            UpdateHexesSprite(hex);
        }
    }

    public Vector2Int CenterOfMap()
    {
        return HexUtils.OffsetOddToAxial(MapResolution.x * 2 / 4, MapResolution.y / 2);
    }

    public void Restart()
    {
        Debug.Log(_hexToHexViews.Count);
        foreach (var hex in _hexToHexViews)
        {
            hex.Key.SetLandTypeProperty(_DefaultlandTypeProperty);
            hex.Value.SpriteRenderer.sprite = hex.Key.LandTypeProperty.GetSprite(); //SpriteSettings.GetSprite(LandType.Water);
        }
        
        _allContinentsHexes?.Clear();
        CreateContinents();
    }
    
    public async void CreateContinents()
    {
        Vector2Int secondContStartPos = HexUtils.OffsetOddToAxial(MapResolution.x * 1 / 4, MapResolution.y / 2);
        await _secondTestContinent.CreateContinent(secondContStartPos , _secondContinentSettings);
        _allContinentsHexes.AddRange(_secondTestContinent.AllHexes);
        
        Vector2Int thirdContStartPos = HexUtils.OffsetOddToAxial(MapResolution.x / 2, MapResolution.y * 9/ 10);
        await _thirdTestContinent.CreateContinent(thirdContStartPos , _thirdContinentSettings, _allContinentsHexes);
        _allContinentsHexes.AddRange(_thirdTestContinent.AllHexes);
        
        await _firstTestContinent.CreateContinent(CenterOfMap(), _continentSettings, _allContinentsHexes);
        _allContinentsHexes.AddRange(_firstTestContinent.AllHexes);
        
        Debug.Log(_allContinentsHexes.Count);
        UpdateHexesSprite(_allContinentsHexes);
    }

    public void DrawRandomRivers(int count)
    { 
        if (_testRivers.Count != 0)
        {
            foreach (var river in _testRivers)
            {
                Destroy(river.gameObject);
            }
            _testRivers.Clear();
        }

        for (int i = 0; i < count; i++)
        {
            DrawRandomRiver();
        }
    }
    
    public void DrawRandomRiver()
    {
       
        List<Vector3> positions = new List<Vector3>();
        var currentRiver = Instantiate(_riverPrefab, this.transform.parent);

        currentRiver.endWidth = 0.2f;
        var starPathPos = _allContinentsHexes[Random.Range(0, _allContinentsHexes.Count)];
        var endPathPos = _allContinentsHexes[Random.Range(0, _allContinentsHexes.Count)];
        
        if (_pathFind.TryPathFind(starPathPos, endPathPos, out var riverPositions))
        {
            foreach (var pathPoint in riverPositions)
            {
                positions.Add(GetHexAtAxialCoordinate(pathPoint).Position()+new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f)));
            }
        }
        currentRiver.positionCount = positions.Count;
        currentRiver.SetPositions(positions.ToArray());
        _testRivers.Add(currentRiver);
    }
    
    public void PathFindTest()
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
        
        _pathPoints.Add(Instantiate(_startPathPointCircle, GetHexAtAxialCoordinate(starPathPos).Position(), Quaternion.identity));
        _pathPoints.Add(Instantiate(_endPathPointCircle, GetHexAtAxialCoordinate(endPathPos).Position(), Quaternion.identity));

        if (_pathFind.TryPathFind(starPathPos, endPathPos, out _tutorPath))
        {
            foreach (var pathPoint in _tutorPath)
            {
                var hex = GetHexAtAxialCoordinate(pathPoint);
                //_pathPoints.Add(Instantiate(_pathPointCircle, hex.Position(), Quaternion.identity));
                
                _hexToHexViews[hex].TextAtHex(hex.LandTypeProperty.MovementCost.ToString());
                _hexToHexViews[hex].SetMeshRendererActive(true);
            }
        }
    }

    private void UpdateHexesSprite(List<Vector2Int> hexes)
    {
        foreach (var hex in hexes)
        {
            UpdateHexesSprite(hex);
        }
    }
    
    private void UpdateHexesSprite(Vector2Int hexAxial)
    {
        UpdateHexesSprite(GetHexAtAxialCoordinate(hexAxial));
    }

    private void UpdateHexesSprite(Hex hex)
    { 
        _hexToHexViews[hex].SpriteRenderer.sprite = hex.LandTypeProperty.GetSprite();
    }
    
    public List<Hex> GetHexesAtAxialCoordinates(List<Vector2Int> axialCoordinates)
    {
        List<Hex> hexes = new List<Hex>();
        foreach (var axial in axialCoordinates)
        {
            if (HexAtAxialCoordinateExist(axial)) // КОСТЫЛЬ! TODO: check and fix 
            {
                hexes.Add(GetHexAtAxialCoordinate(axial));
            }
            else
            {
                Debug.LogWarning("[getHex] No Hex at Axial: " + axial);
                Debug.LogWarning("[getHex] No Hex at Offset: " + HexUtils.AxialToOffsetOdd(axial));
            }
        }
        return hexes;
    }

    public Hex GetHexAtAxialCoordinate(Vector2Int axial)
    {
       return GetHexAtOffsetCoordinate(HexUtils.AxialToOffsetOdd(axial));
    }

    public Hex GetHexAtOffsetCoordinate(Vector2Int offset)
    {
        if (!HexAtOffsetCoordinateExist(offset))
        {
            throw new System.ArgumentException("Wrong Coordinate. Hex doesn't exist");
            //Debug.LogWarning("there is no hex at: " + offset);
            //return null;
        }
        return _hexStorageOddOffset[offset.x, offset.y];
    }

    public bool HexAtAxialCoordinateExist(Vector2Int axial)
    {
        return HexAtOffsetCoordinateExist(HexUtils.AxialToOffsetOdd(axial));
    }
    
    public bool HexAtOffsetCoordinateExist(Vector2Int offset)
    {
        if (offset.x >= 0 && offset.x < MapResolution.x && offset.y >= 0 && offset.y < MapResolution.y)
        {
            return true;
        }
        return false;
    }
    
}