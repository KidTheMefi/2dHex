using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HexMap : MonoBehaviour
{
    [SerializeField] private HexView HexPrefab;
    [SerializeField] private SpriteSettings SpriteSettings;

    [SerializeField] private Vector2Int MapResolution;
    
    [SerializeField] private int _minContinentTilesCount;
    
    private Hex[,] _hexStorageOddOffset;
    private Dictionary<Hex, HexView> _hexToHexViews;
    private Vector2Int[] _axialDirectionVectors =
    {
        new Vector2Int(1, 0),
        new Vector2Int(1, -1),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0),
        new Vector2Int(-1, 1),
        new Vector2Int(0, 1)
    };
    void Start()
    {
        GenerateMap();
    }

    public void Restart()
    {
        foreach (var hex in _hexToHexViews)
        {
            hex.Value.SpriteRenderer.sprite = SpriteSettings.GetSprite(TerrainType.Water);
        }
        var continent = CreateContinent(OffsetOddToAxial(MapResolution.x*2/4, MapResolution.y / 2), 2,_minContinentTilesCount);
        SetTilesSprites(GetHexesAtAxialCoordinates(continent), TerrainType.Grass);
        
        


        List<Vector2Int> continentMountain = CreateMountainsAtContinent(continent, continent.Count*20/100);  // 30%
        SetTilesSprites(GetHexesAtAxialCoordinates(continentMountain), TerrainType.Mountain);
        
        List<Vector2Int> continentHills = CreateMountainsAtContinent(continent, continent.Count*30/100);
        SetTilesSprites(GetHexesAtAxialCoordinates(continentHills), TerrainType.Hill);
        
        Debug.Log(continent.Count);
        
        var secondcontinent = CreateContinent(OffsetOddToAxial(MapResolution.x/4, MapResolution.y / 2), 2,200);
        SetTilesSprites(GetHexesAtAxialCoordinates(secondcontinent), TerrainType.Desert);
        Debug.Log(secondcontinent.Count);
        
        var thirdContinent = CreateContinent(OffsetOddToAxial(MapResolution.x*3/4, MapResolution.y / 2), 2,300);
        SetTilesSprites(GetHexesAtAxialCoordinates(thirdContinent), TerrainType.Desert);
        Debug.Log(thirdContinent.Count);
        
        var poleNContinent = CreateContinent(OffsetOddToAxial(MapResolution.x/2, MapResolution.y* 9/10 ), 2,300);
        SetTilesSprites(GetHexesAtAxialCoordinates(poleNContinent), TerrainType.Snow);
        Debug.Log(thirdContinent.Count);

        Vector2Int randomOffset = new Vector2Int(Random.Range(0, 10), Random.Range(0, 10));
        var line = GetAxialLine(new Vector2Int(0, 0), OffsetOddToAxial(randomOffset) );
        SetTilesSprites(GetHexesAtAxialCoordinates(line), TerrainType.Snow);
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
                Hex hex = new Hex(OffsetOddToAxial(oddCoordinate), oddCoordinate);

                HexView hexTileView = Instantiate(
                    HexPrefab,
                    hex.Position(),
                    Quaternion.identity,
                    this.transform);
                
                var terrain = TerrainType.Water;
                hexTileView.SpriteRenderer.sprite = SpriteSettings.GetSprite(terrain);
                hexTileView.gameObject.name = oddCoordinate.ToString();
                hexTileView.ViewAxialCoordinate(OffsetOddToAxial(oddCoordinate).ToString());

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
            hexes.Add(GetHexAtAxialCoordinate(axial));
        }
        return hexes;
    }

    public Hex GetHexAtAxialCoordinate(Vector2Int axial)
    {
       return GetHexAtOffsetCoordinate(AxialToOffsetOdd(axial));
    }

    public Hex GetHexAtOffsetCoordinate(Vector2Int offset)
    {
        if (!HexAtOffsetCoordinateExist(offset))
        {
            throw new System.ArgumentException("Wrong Coordinate. Hex doesn't exist");
        }
        return _hexStorageOddOffset[offset.x, offset.y];
    }

    private bool HexAtAxialCoordinateExist(Vector2Int axial)
    {
        return HexAtOffsetCoordinateExist(AxialToOffsetOdd(axial));
    }
    
    private bool HexAtOffsetCoordinateExist(Vector2Int offset)
    {
        if (offset.x >= 0 && offset.x < MapResolution.x && offset.y >= 0 && offset.y < MapResolution.y)
        {
            return true;
        }
        return false;
    }

    private List<Vector2Int> GetAxialRingWithRadius(Vector2Int centerAxial, int radius)
    {
        var hexes = new List<Vector2Int>();
        var hexAxial = centerAxial + (_axialDirectionVectors[4] * radius);

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < radius; j++)
            {
                if (HexAtAxialCoordinateExist(hexAxial))
                {
                    hexes.Add(hexAxial);
                }
                hexAxial = AxialNeighbor(hexAxial, i);
            }
        }
        return hexes;
    }

    private List<Vector2Int> GetAxialAreaAtRange(Vector2Int centerAxial, int range)
    {
        var hexes = new List<Vector2Int>();
        for (int q = -range; q <= range; q++)
        {
            for (int r = Mathf.Max(-range, -q - range); r <= Mathf.Min(range, -q + range); r++)
            {
                Vector2Int hexAxial = new Vector2Int(centerAxial.x + q, centerAxial.y + r);
                if (HexAtAxialCoordinateExist(hexAxial))
                {
                    hexes.Add(hexAxial);
                }
            }
        }
        return hexes;
    }

    private List<Vector2Int> GetAxialLine(Vector2Int start, Vector2Int end)
    {

        int n = AxialDistance(start, end);
        List<Vector2Int> hexes = new List<Vector2Int>();

        float step = 1f / Mathf.Max(n, 1);

        for (int i = 0; i <= n; i++)
        {
            hexes.Add(AxialRound(Vector2.Lerp(start, end, step * i)));
        }
        return hexes;
    }

    private Vector2Int AxialRound(Vector2 axial)
    {
        return CubeToAxial(CubeRound(AxialToCube(axial)));
    }

    private Vector3Int CubeRound(Vector3 cube)
    {
        int x = Mathf.RoundToInt(cube.x);
        int y = Mathf.RoundToInt(cube.y);
        int z = Mathf.RoundToInt(cube.z);

        float xDiff = Mathf.Abs((x - cube.x));
        float yDiff = Mathf.Abs((y - cube.y));
        float zDiff = Mathf.Abs((z - cube.z));

        if (xDiff > yDiff && xDiff > zDiff)
        {
            x = -y - z;
        }
        else if (yDiff>zDiff)
        {
            y = -x - z;
        }
        else
        {
            z = -x - y;
        }
        return new Vector3Int(x, y, z);
    }

    private Vector2Int CubeToAxial(Vector3Int cube)
    {
        return new Vector2Int(cube.x, cube.y);
    }
    
    private Vector3 AxialToCube(Vector2 axial)
    {
        return new Vector3(axial.x, axial.y, -axial.x - axial.y);
    }
    
    private Vector2Int AxialNeighbor(Vector2Int hexAxial, int direction)
    {
        return (hexAxial + _axialDirectionVectors[direction]);
    }
    
    public int AxialDistance(Vector2Int a, Vector2Int b)
    {
        Vector2Int vec = a - b;
        return (Mathf.Abs(vec.x) + Mathf.Abs(vec.x + vec.y) + Mathf.Abs(vec.y)) / 2;
    }
    
    public Vector2Int AxialToOffsetOdd(Vector2Int axialCordinates)
    {
        var col = axialCordinates.x + (axialCordinates.y - (axialCordinates.y & 1)) / 2;
        return new Vector2Int(col, axialCordinates.y);
    }

    public Vector2Int OffsetOddToAxial(Vector2Int oddCordinates)
    {
        var q = oddCordinates.x - (oddCordinates.y - (oddCordinates.y & 1)) / 2;
        return new Vector2Int(q, oddCordinates.y);
    }

    public Vector2Int OffsetOddToAxial(int x, int y)
    {
        var q = x - (y - (y & 1)) / 2;
        return new Vector2Int(q, y);
    }
    
    public Vector2Int AxialToOffsetOdd(int x, int y)
    {
        var col = x + (y - (y & 1)) / 2;
        return new Vector2Int(col, y);
    }

    private Vector2Int RandomAxialDirection()
    {
        return _axialDirectionVectors[Random.Range(0, 6)];
    }

    private Vector2Int RandomAxialAtRadius(Vector2Int center, int radius)
    {
        List<Vector2Int> hexesAxial = GetAxialRingWithRadius(center, radius);
        return hexesAxial[Random.Range(0, hexesAxial.Count)];
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
        List<Vector2Int> continentTilesCoordinate =  CreatePieceOffLand(center, startScale);

        for (int i = 0; i < 5; i++)
        {
            Vector2Int nextCenter = RandomAxialAtRadius(center, Random.Range(startScale, startScale+3));
            int nextScaleMin = AxialDistance(center, nextCenter) - startScale+1;
            int nextScale = Random.Range(nextScaleMin, nextScaleMin + 1);

            CreatePieceOffLand(center, startScale);
            
            foreach (var axial in CreatePieceOffLand(nextCenter,nextScale))
            {
                if (!continentTilesCoordinate.Contains(axial))
                {
                    continentTilesCoordinate.Add(axial);
                }
            }
        }
        return continentTilesCoordinate;
    }

    private List<Vector2Int> CreatePieceOffLand(Vector2Int center, int startScale)
    {
        List<Vector2Int> landCoordinates = GetAxialAreaAtRange(center, startScale);
        
        foreach (var axial in GetAxialAreaAtRange(RandomAxialAtRadius(center, startScale), startScale - 1))
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
            foreach (var axial in CreatePieceOffMountain(continent[Random.Range(0, continent.Count)], 1))
            {
                if (continent.Contains(axial))
                {
                    continentMountains.Add(axial);
                    continent.Remove(axial);
                }
            }
        }
        return continentMountains;
    }
    
    private List<Vector2Int> CreatePieceOffMountain(Vector2Int center, int startScale)
    {
        List<Vector2Int> landCoordinates = GetAxialAreaAtRange(center, startScale);
        for (int i = 0; i < 3; i++)
        {
            landCoordinates.RemoveAt(Random.Range(0,landCoordinates.Count));
        }
        return landCoordinates;
    }

  #endregion
}