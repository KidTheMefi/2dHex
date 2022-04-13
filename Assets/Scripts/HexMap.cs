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
        
        
        


        
        
        var secondcontinent = CreateContinent(HexUtils.OffsetOddToAxial(MapResolution.x/4, MapResolution.y / 2), 2,200);
        SetTilesSprites(GetHexesAtAxialCoordinates(secondcontinent), TerrainType.Desert);

        var thirdContinent = CreateContinent(HexUtils.OffsetOddToAxial(MapResolution.x*3/4, MapResolution.y / 2), 2,300);
        SetTilesSprites(GetHexesAtAxialCoordinates(thirdContinent), TerrainType.Desert);

        var poleNContinent = CreateContinent(HexUtils.OffsetOddToAxial(MapResolution.x/2, MapResolution.y* 9/10 ), 2,300);
        SetTilesSprites(GetHexesAtAxialCoordinates(poleNContinent), TerrainType.Snow);

        Vector2Int randomOffset = new Vector2Int(Random.Range(0, 10), Random.Range(0, 10));
        var line = HexUtils.GetAxialLine(new Vector2Int(0, 0), HexUtils.OffsetOddToAxial(randomOffset) );
        SetTilesSprites(GetHexesAtAxialCoordinates(line), TerrainType.Snow);
        
        var continent = CreateContinent(HexUtils.OffsetOddToAxial(MapResolution.x*2/4, MapResolution.y / 2), 2,_minContinentTilesCount);
        SetTilesSprites(GetHexesAtAxialCoordinates(continent), TerrainType.Grass);
        
        List<Vector2Int> continentMountain = CreateMountainsAtContinent(continent, continent.Count*20/100);  // 30%
        SetTilesSprites(GetHexesAtAxialCoordinates(continentMountain), TerrainType.Mountain);
        
        List<Vector2Int> continentHills = CreateMountainsAtContinent(continent, continent.Count*30/100);
        SetTilesSprites(GetHexesAtAxialCoordinates(continentHills), TerrainType.Hill);
        
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
                Debug.LogWarning("[getHex] No Hex at: " + axial);
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
            //throw new System.ArgumentException("Wrong Coordinate. Hex doesn't exist");
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
     
     
    private List<Vector2Int> CreateContinentPart(Vector2Int center, int startScale)
    {
        List<Vector2Int> continentTilesCoordinate =  CreatePieceOffLand(center, startScale);

        for (int i = 0; i < 5; i++)
        {
            
            
            Vector2Int nextCenter = RandomAxialAtRadius(center, Random.Range(startScale, startScale+3));
            int nextScaleMin = HexUtils.AxialDistance(center, nextCenter) - startScale+1;
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
        List<Vector2Int> landCoordinates = HexUtils.GetAxialAreaAtRange(center, startScale);
        List<Vector2Int> results = new List<Vector2Int>();
        foreach (var axial in HexUtils.GetAxialAreaAtRange(RandomAxialAtRadius(center, startScale), startScale - 1))
        {
            landCoordinates.Remove(axial);
        }
        
        foreach (var axial in landCoordinates)
        {
            if (HexAtAxialCoordinateExist(axial))
            {
                results.Add(axial);
            }
        }
        
        if (results.Count == 0)
        {
            results.Add(center);
        }
        return results;
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
                    continent.Remove(axial); // look at this dude!
                }
            }
        }
        return continentMountains;
    }
    
    private List<Vector2Int> CreatePieceOffMountain(Vector2Int center, int startScale)
    {
        List<Vector2Int> landCoordinates = HexUtils.GetAxialAreaAtRange(center, startScale);
        List<Vector2Int> results = new List<Vector2Int>();
        for (int i = 0; i < 3; i++)
        {
            landCoordinates.RemoveAt(Random.Range(0,landCoordinates.Count));
        }

        foreach (var axial in landCoordinates)
        {
            if (HexAtAxialCoordinateExist(axial))
            {
                results.Add(axial);
            }
        }
        
        if (results.Count == 0)
        {
            results.Add(center);
        }
        
        return results;
    }

  #endregion
}