using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum MapSize
{
    Small, Default, Big
}
[CreateAssetMenu(menuName = "HexGame/MapSettings")]
public class MapSetting : ScriptableObject
{
    [SerializeField] private MapSize _mapSize;
    [SerializeField] private List<ContinentAtMap> _continentsAtMap;

    public List<ContinentAtMap> ContinentsAtMap => _continentsAtMap;
    
    public Vector2Int MapResolution()
    {
        switch (_mapSize)
        {
            case MapSize.Small:
                return new Vector2Int(45, 30);
            case MapSize.Default:
                return new Vector2Int(90, 60);
            case MapSize.Big:
                return new Vector2Int(135, 90);
            default:
                return new Vector2Int(90, 60);
        }
    }
}

[Serializable]
public class ContinentAtMap
{
    [SerializeField, Range(1,100)] private int _percent;
    
    //the left down point is 0, 0; the right up point is 100, 100
    [SerializeField, Range(0,100)] private int _StartPointXInPercent;
    [SerializeField, Range(0,100)] private int _StartPointYInPercent;
    [SerializeField] private ContinentSettings settings;

    public int Percent => _percent;
    public int StartPointXInPercent => _StartPointXInPercent;
    public int StartPointYInPercent => _StartPointYInPercent;
    public ContinentSettings Settings => settings;


}
