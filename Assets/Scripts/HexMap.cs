using System;
using System.Collections;
using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class HexMap : MonoBehaviour, IWeightedGraph<Vector2Int>
{
    [SerializeField] private HexView HexPrefab;
    [SerializeField] private GameObject _pathPointCircle;
    [SerializeField] private GameObject _startPathPointCircle;
    [SerializeField] private GameObject _endPathPointCircle;
    
    [SerializeField] private SpriteSettings SpriteSettings;

    [SerializeField] private Vector2Int MapResolution;
    [SerializeField] private int _minContinentTilesCount;

    private HashSet<Vector2Int> _mountains = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> _forrest = new HashSet<Vector2Int>();
    private HashSet<GameObject> _pathPoints = new HashSet<GameObject>();
    
    private Hex[,] _hexStorageOddOffset;
    private Dictionary<Hex, HexView> _hexToHexViews;
    
    void Start()
    {
        
        GenerateMap();
        
    }

      #region FindPath

    private bool Passable(Vector2Int axial)
    {
        return !_mountains.Contains(axial);
    }

    public int Cost(Vector2Int axial)
    {
        return GetHexAtAxialCoordinate(axial).MovementCost;
    }
    
    public IEnumerable<Vector2Int> Neighbors(Vector2Int axial)
    {
        foreach (var dir in HexUtils.AxialDirectionVectors) 
        {
            Vector2Int next = new Vector2Int(axial.x + dir.x, axial.y + dir.y);
            if (HexAtAxialCoordinateExist(next) && Passable(next)) 
            {
                yield return next;
            }
        }
    }
    
   
    
      #endregion
    
    public void Restart()
    {
        foreach (var hex in _hexToHexViews)
        {
            hex.Value.SpriteRenderer.sprite = SpriteSettings.GetSprite(TerrainType.Water);
        }

        var continent = CreateContinent(HexUtils.OffsetOddToAxial(MapResolution.x*2/4, MapResolution.y / 2), 2,_minContinentTilesCount);
        SetTilesSprites(GetHexesAtAxialCoordinates(continent), TerrainType.Grass);
        
        List<Vector2Int> continentMountain = CreateMountainsAtContinent(continent, continent.Count*10/100);
        
        _mountains.Clear();
        foreach (var axial in continentMountain)
        {
            _mountains.Add(axial);
        }
        SetTilesSprites(GetHexesAtAxialCoordinates(continentMountain), TerrainType.Mountain);
        
        List<Vector2Int> continentForrest = CreateMountainsAtContinent(continent, continent.Count*20/100);
        foreach (var forrest in GetHexesAtAxialCoordinates(continentForrest))
        {
            forrest.SetMovementCost(3);
        }
        
        SetTilesSprites(GetHexesAtAxialCoordinates(continentForrest), TerrainType.Forrest);
        
        List<Vector2Int> continentHills = CreateMountainsAtContinent(continent, continent.Count*10/100);
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

    public void PathFindTest()
    {
        foreach (var hex in _hexToHexViews)
        {
            hex.Value.SpriteRenderer.sprite = SpriteSettings.GetSprite(TerrainType.Water);
        }
        
        var testContinent = HexUtils.GetAxialAreaAtRange(HexUtils.OffsetOddToAxial(MapResolution.x*2/4, MapResolution.y / 2), 6);
        SetTilesSprites(GetHexesAtAxialCoordinates(testContinent), TerrainType.Grass);
        
        
        foreach (var point in _pathPoints)
        {
            Destroy(point);
        }
        
        var starPathPos = new Vector2Int(24,30);
        var endPathPos = new Vector2Int(32,30);

        starPathPos = testContinent[Random.Range(0, testContinent.Count)];
        endPathPos = testContinent[Random.Range(0, testContinent.Count)];
        _mountains.Clear();
        var mountain = HexUtils.GetAxialLine(new Vector2Int(26, 34), new Vector2Int(35, 26));
        mountain = HexUtils.GetAxialRingWithRadius(new Vector2Int(30, 30), 3);
        SetTilesSprites(GetHexesAtAxialCoordinates(mountain), TerrainType.Mountain);
        foreach (var mount in mountain)
        {
            _mountains.Add(mount);
        }
        _pathPoints.Add(Instantiate(_startPathPointCircle, GetHexAtAxialCoordinate(starPathPos).Position(), Quaternion.identity));
        _pathPoints.Add(Instantiate(_endPathPointCircle, GetHexAtAxialCoordinate(endPathPos).Position(), Quaternion.identity));

        Debug.Log(HexUtils.AxialDistance(starPathPos + Vector2Int.up, endPathPos));
        Debug.Log(HexUtils.AxialDistance(starPathPos + Vector2Int.right, endPathPos));
        
        var astar = new AStarSearch(this, starPathPos, endPathPos);

        Vector2Int drawPathPoint = endPathPos;
        while (!(drawPathPoint == starPathPos))
        {
            if (!astar.cameFrom.TryGetValue(drawPathPoint, out drawPathPoint))
            {
                Debug.Log(starPathPos + " Unreachable from " + endPathPos);
                break;
            }
            _pathPoints.Add(Instantiate(_pathPointCircle, GetHexAtAxialCoordinate(drawPathPoint).Position(), Quaternion.identity));
        }
        
    }

    private void SetTilesSprites(List<Hex> hexes, TerrainType terrainType)
    {
        foreach (var hex in hexes)
        {
            _hexToHexViews[hex].SpriteRenderer.sprite = SpriteSettings.GetSprite(terrainType);
        }
    }

    private void GenerateMap() //more like generate hex grid
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
                
                var terrain = TerrainType.Water;
                hexTileView.SpriteRenderer.sprite = SpriteSettings.GetSprite(terrain);
                hexTileView.gameObject.name = oddCoordinate.ToString();
                hexTileView.ViewAxialCoordinate(HexUtils.OffsetOddToAxial(oddCoordinate).ToString());

                _hexStorageOddOffset[column, row] = hex;
                _hexToHexViews.Add(hex, hexTileView);
            }
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
            Debug.LogWarning("there is no hex at: " + offset);
            return null;
        }
        return _hexStorageOddOffset[offset.x, offset.y];
    }

    private bool HexAtAxialCoordinateExist(Vector2Int axial)
    {
        return HexAtOffsetCoordinateExist(HexUtils.AxialToOffsetOdd(axial));
    }
    
    private bool HexAtOffsetCoordinateExist(Vector2Int offset)
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
                if (HexAtAxialCoordinateExist(axial)&&continent.Contains(axial))
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