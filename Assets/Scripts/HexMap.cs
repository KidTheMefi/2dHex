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
    
    [SerializeField] private SpriteSettings SpriteSettings;
    [SerializeField] private ContinentSettings _continentSettingsSettings;
    
    [SerializeField] private Vector2Int MapResolution;
    [SerializeField] private int _minContinentTilesCount;

    private GameObject _centerPoint;
    private List<LineRenderer> _testRivers = new List<LineRenderer>();
    private List<Vector2Int> _tutorPath;

    private HashSet<GameObject> _pathPoints = new HashSet<GameObject>();
    
    private Hex[,] _hexStorageOddOffset;
    private Dictionary<Hex, HexView> _hexToHexViews;

    private AStarSearch _pathFind;
    private LandGeneration _landGeneration;
    private Continent _firstTestContinent;
    void Start()
    {
        
        _pathFind = new AStarSearch(this);
        _landGeneration = new LandGeneration(this);
        _firstTestContinent = new Continent(this, _landGeneration);
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
                
                var terrain = LandType.Water;
                hexTileView.SpriteRenderer.sprite = SpriteSettings.GetSprite(terrain);
                hexTileView.gameObject.name = oddCoordinate.ToString();
                hexTileView.TextAtHex(HexUtils.OffsetOddToAxial(oddCoordinate).ToString());

                _hexStorageOddOffset[column, row] = hex;
                _hexToHexViews.Add(hex, hexTileView);
            }
        }
    }

    public Vector2Int CenterOfMap()
    {
        return HexUtils.OffsetOddToAxial(MapResolution.x * 2 / 4, MapResolution.y / 2);
    }

    public void CreateContinent()
    {
        _firstTestContinent.CreateContinent(CenterOfMap(), _minContinentTilesCount, _continentSettingsSettings);

        UpdateHexesSprite(_firstTestContinent.AllHexes);
    }
    
    public void Restart()
    {
        foreach (var hex in _hexToHexViews)
        {
            hex.Value.SpriteRenderer.sprite = SpriteSettings.GetSprite(LandType.Water);
            hex.Key.SetLandType(LandType.Water);
        }

        CreateContinent();
        /*
        _continentReachableHex = CreateContinent(HexUtils.OffsetOddToAxial(MapResolution.x*2/4, MapResolution.y / 2), 2,_minContinentTilesCount);
        var continent = new List<Vector2Int>();


        if (_centerPoint != null)
        {
            Destroy(_centerPoint);
        }
        Vector2Int centerAxial = CenterOf(_continentReachableHex);
        _centerPoint = Instantiate(_pathPointCircle, GetHexAtAxialCoordinate(centerAxial).Position(), Quaternion.identity);

        
        foreach (var hex in _continentReachableHex)
        {
            continent.Add(hex);
            GetHexAtAxialCoordinate(hex).SetPassible(true);
        }
        SetTilesSprites(GetHexesAtAxialCoordinates(continent), TerrainType.Grass);
        
        List<Vector2Int> continentMountain = CreateMountainsAtContinent(continent, continent.Count*10/100);
        _continentMountain = continentMountain;
        foreach (var axial in continentMountain)
        {
            GetHexAtAxialCoordinate(axial).SetPassible(false);
        }
        
        SetTilesSprites(GetHexesAtAxialCoordinates(continentMountain), TerrainType.Mountain);
        
        List<Vector2Int> continentForrest = CreateMountainsAtContinent(continent, continent.Count*20/100);
        foreach (var forrest in GetHexesAtAxialCoordinates(continentForrest))
        {
            forrest.SetMovementCost(3);
        }
        
        SetTilesSprites(GetHexesAtAxialCoordinates(continentForrest), TerrainType.Forrest);
        
        List<Vector2Int> continentHills = CreateMountainsAtContinent(continent, continent.Count*10/100);
        foreach (var hills in GetHexesAtAxialCoordinates(continentHills))
        {
            hills.SetMovementCost(5);
        }
        SetTilesSprites(GetHexesAtAxialCoordinates(continentHills), TerrainType.Hill);

        
        /* var secondcontinent = CreateContinent(HexUtils.OffsetOddToAxial(MapResolution.x/4, MapResolution.y / 2), 2,200);
         SetTilesSprites(GetHexesAtAxialCoordinates(secondcontinent), TerrainType.Desert);
 
         var thirdContinent = CreateContinent(HexUtils.OffsetOddToAxial(MapResolution.x*3/4, MapResolution.y / 2), 2,300);
         SetTilesSprites(GetHexesAtAxialCoordinates(thirdContinent), TerrainType.Desert);
 
         var poleNContinent = CreateContinent(HexUtils.OffsetOddToAxial(MapResolution.x/2, MapResolution.y* 9/10 ), 2,300);
         SetTilesSprites(GetHexesAtAxialCoordinates(poleNContinent), TerrainType.Snow);*/

        /*TODO: LineDrawingExample
         
         Vector2Int randomOffset1 = HexUtils.OffsetOddToAxial(Random.Range(0, MapResolution.x), Random.Range(0, MapResolution.y));
        Vector2Int randomOffset2= HexUtils.OffsetOddToAxial(Random.Range(0, MapResolution.x), Random.Range(0, MapResolution.y));
        
        var line = HexUtils.GetAxialLine(randomOffset1, randomOffset2);
        SetTilesSprites(GetHexesAtAxialCoordinates(line), TerrainType.Snow);*/
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
        var starPathPos = _firstTestContinent.AllHexes[Random.Range(0, _firstTestContinent.AllHexes.Count)];
        var endPathPos = _firstTestContinent.AllHexes[Random.Range(0, _firstTestContinent.AllHexes.Count)];
        
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
        
        var starPathPos = _firstTestContinent.AllHexes[Random.Range(0, _firstTestContinent.AllHexes.Count)];
        var endPathPos = _firstTestContinent.AllHexes[Random.Range(0, _firstTestContinent.AllHexes.Count)];
        
        _pathPoints.Add(Instantiate(_startPathPointCircle, GetHexAtAxialCoordinate(starPathPos).Position(), Quaternion.identity));
        _pathPoints.Add(Instantiate(_endPathPointCircle, GetHexAtAxialCoordinate(endPathPos).Position(), Quaternion.identity));

        if (_pathFind.TryPathFind(starPathPos, endPathPos, out _tutorPath))
        {
            foreach (var pathPoint in _tutorPath)
            {
                var hex = GetHexAtAxialCoordinate(pathPoint);
                //_pathPoints.Add(Instantiate(_pathPointCircle, hex.Position(), Quaternion.identity));
                
                _hexToHexViews[hex].TextAtHex(hex.GetMovementCost().ToString());
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
        Hex hex = GetHexAtAxialCoordinate(hexAxial);
        _hexToHexViews[hex].SpriteRenderer.sprite = SpriteSettings.GetSprite(hex.LandTypeHex);
    }

    private void SetTilesSprites(List<Hex> hexes, LandType landType)
    {
        foreach (var hex in hexes)
        {
            _hexToHexViews[hex].SpriteRenderer.sprite = SpriteSettings.GetSprite(landType);
        }
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
    

    #region LandGeneration

     private List<Vector2Int> CreateContinent(Vector2Int center, int startScale, int minTilesNumber)
    {
        List<Vector2Int> continentTilesCoordinate = CreateContinentPart(center, startScale);
        
        while (continentTilesCoordinate.Count<minTilesNumber)
        {
            var continentAdd = CreateContinentPart(continentTilesCoordinate[Random.Range(0, continentTilesCoordinate.Count)], 2);

            foreach (var axial in continentAdd)
            {
                if (!continentTilesCoordinate.Contains(axial))
                {
                    continentTilesCoordinate.Add(axial);
                }
            }
        }
        
        return continentTilesCoordinate;
    }


     private List<Vector2Int> CreateContinentPart(Vector2Int center, int startScale)
    {
        List<Vector2Int> continentTilesCoordinate =  new List<Vector2Int>();

        for (int i = 0; i < 5; i++) // magic 5. TODO: smth with that
        {
            Vector2Int nextCenter = RandomAxialAtRadius(center, Random.Range(startScale, startScale+3));
            int nextScaleMin = HexUtils.AxialDistance(center, nextCenter) - startScale+1;
            int nextScale = Random.Range(nextScaleMin, nextScaleMin + 1);

            foreach (var axial in CreatePieceOffLand(nextCenter,nextScale))
            {
                if (!continentTilesCoordinate.Contains(axial)&& HexAtAxialCoordinateExist(axial))
                {
                    continentTilesCoordinate.Add(axial);
                }
            }
        }
        return continentTilesCoordinate;
    }
     
     private Vector2Int RandomAxialAtRadius(Vector2Int center, int radius)
     {
         List<Vector2Int> axialAtRadius = new List<Vector2Int>();
         foreach (var axial in HexUtils.GetAxialRingWithRadius(center, radius))
         {
             if (HexAtAxialCoordinateExist(axial))
             {
                 axialAtRadius.Add(axial);
             }
         }

         return axialAtRadius[Random.Range(0, axialAtRadius.Count)];
     }

    private List<Vector2Int> CreatePieceOffLand(Vector2Int center, int startScale)
    {
        List<Vector2Int> landCoordinates = HexUtils.GetAxialAreaAtRange(center, startScale);
        
        foreach (var axial in HexUtils.GetAxialAreaAtRange(RandomAxialAtRadius(center, startScale), startScale - 1))
        {
            landCoordinates.Remove(axial);
        }
        return landCoordinates;
    }
    
    private List<Vector2Int> CreateMountainsAtContinent(List<Vector2Int> continent, int minTilesNumber)
    {
        //not single responsibility !!!!
        List<Vector2Int> continentMountains = new List<Vector2Int>();
        while (continentMountains.Count<minTilesNumber)
        {
            foreach (var axial in CreatePieceOffMountain(continent[Random.Range(0, continent.Count)]))
            {
                if (continent.Contains(axial))
                {
                    continentMountains.Add(axial);
                    continent.Remove(axial); // look at this dude!
                }
            }
        }
        return continentMountains;
    }
    
    private List<Vector2Int> CreatePieceOffMountain(Vector2Int center)
    {
        List<Vector2Int> landCoordinates = HexUtils.GetAxialAreaAtRange(center, 1);
   
        for (int i = 0; i < 3; i++)
        {
            landCoordinates.RemoveAt(Random.Range(0,landCoordinates.Count));
        }
        return landCoordinates;
    }

  #endregion
}