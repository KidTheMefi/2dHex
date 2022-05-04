using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using Zenject;



public class HexMapGrid :  IInitializable, IHexStorage, IMapBorder
{
    private MapSetting _mapSetting;
    private HexView.Factory _hexViewFactory;
    
    private GameObject _centerPoint;
    
    private Hex[,] _hexStorageOddOffset;
    private Dictionary<Hex, HexView> _hexToHexViews = new Dictionary<Hex, HexView>();
    private Dictionary<HexView, Hex> _hexViewToHex = new Dictionary<HexView, Hex>();
    
    private float _borderIndent = 1f;
    
    private Vector2Int _mapResolution;
    public HexMapGrid(MapSetting mapSetting, HexView.Factory hexViewFactory)
    {
        _mapSetting = mapSetting;
        _hexViewFactory = hexViewFactory;
    }
    
    
    public void Initialize()
    {
        GenerateMapGrid();
    }

    private void GenerateMapGrid() //more like generate hex grid
    {
        if (_mapSetting == null)
        {
            Debug.Log("didnt injected");
        }
        _mapResolution = _mapSetting.MapResolution();
        _hexStorageOddOffset = new Hex[_mapResolution.x, _mapResolution.y];

        for (int column = 0; column < _mapResolution.x; column++)
        {
            for (int row = 0; row < _mapResolution.y; row++)
            {
                Vector2Int oddCoordinate = new Vector2Int(column, row);
                Hex hex = new Hex(HexUtils.OffsetOddToAxial(oddCoordinate));
                HexView hexTileView = _hexViewFactory.Create();
                hexTileView.transform.position = hex.Position;
                

                hexTileView.gameObject.name = oddCoordinate.ToString();
                hexTileView.TextAtHex(HexUtils.OffsetOddToAxial(oddCoordinate).ToString());
                hex.LandTypeSpriteChanged += ChangeHexSprite;
                hex.SetLandTypeProperty(_mapSetting.DefaultLandTypeProperty);
                _hexStorageOddOffset[column, row] = hex;
                _hexToHexViews.Add(hex, hexTileView);
                _hexViewToHex.Add(hexTileView, hex);
            }
        }
    }

    private void ChangeHexSprite(Hex hex, Sprite sprite)
    {
        if (_hexToHexViews.TryGetValue(hex, out var hexView))
        {
            hexView.SetHexViewSprite(sprite);
        }
    }
    
    public Vector2Int CenterOfMap()
    {
        return HexUtils.OffsetOddToAxial(_mapResolution.x  / 2, _mapResolution.y / 2);
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
        if (!HexAtAxialCoordinateExist(axial))
        {
            throw new System.ArgumentException("Wrong Axial Coordinate. Hex doesn't exist");
            //Debug.LogWarning("there is no hex at: " + offset);
            //return null;
        }

        var offset = HexUtils.AxialToOffsetOdd(axial);
        return _hexStorageOddOffset[offset.x, offset.y];
    }

    public bool HexAtAxialCoordinateExist(Vector2Int axial)
    {
        var offset = HexUtils.AxialToOffsetOdd(axial);
        if (offset.x >= 0 && offset.x < _mapResolution.x && offset.y >= 0 && offset.y < _mapResolution.y)
        {
            return true;
        }
        return false;
    }
    
    public Dictionary<Hex, HexView> HexToHexView()
    {
        return _hexToHexViews;
    }
    public Dictionary<HexView, Hex> HexViewToHex()
    {
        return _hexViewToHex;
    }

    private void OnDestroy()
    {
        foreach (var tile in _hexToHexViews)
        {
            tile.Key.LandTypeSpriteChanged -= ChangeHexSprite;
        }
    }

    public MapBorder GetMapBorderWorld()
    {
        var minPoint = HexUtils.CalculatePosition(new Vector2Int(0, 0)) - Vector3.one*_borderIndent;
        var maxPointOffset = HexUtils.OffsetOddToAxial(new Vector2Int(_mapSetting.MapResolution().x, _mapSetting.MapResolution().y));
        var maxPoint = HexUtils.CalculatePosition(maxPointOffset) + Vector3.one*_borderIndent;
        return new MapBorder(minPoint, maxPoint);
    }

    #region  HexView Getter

    //HexView getter (unused for now)
    /*
   public HexView GetHexView(Hex hex)
   {
       if (_hexToHexViews.TryGetValue(hex, out var hexView))
       {
           return hexView;
       }
       else
       {
           throw new System.ArgumentException("Wrong Coordinate. HexView doesn't exist");
       }
   }
   
   public HexView GetHexView(Vector2Int hexAxial)
   {
       return GetHexView(GetHexAtAxialCoordinate(hexAxial));
   }

   public List<HexView> GetHexViews(List<Vector2Int> hexesAxial)
   {
       List<HexView> hexViews = new List<HexView>();
       foreach (var hex in hexesAxial)
       {
           hexViews.Add(GetHexView(hex));
       }
       return hexViews;
   }
   */

  #endregion
    
}

public struct MapBorder
{
    public float XMin { get; }
    public float YMin { get; }
    public float XMax { get; }
    public float YMax { get; }
    
    public override string ToString() => $"({XMin}, {YMin}, {XMax}, {YMax})";
    public MapBorder(Vector2 minPoint, Vector2 maxPoint)
    {
        XMin = minPoint.x;
        YMin = minPoint.y;
        XMax = maxPoint.x;
        YMax = maxPoint.y;
    }
}
