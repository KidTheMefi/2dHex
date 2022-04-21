using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HexGame/ContinentSettings")]
public class ContinentSettings : ScriptableObject
{
    [SerializeField] private int _minHexesCount;
    [SerializeField] private List<ContinentSetting> _continentSettings;
    [SerializeField] private LandTypeProperty _defaultLandType;
    
    public List<ContinentSetting> Lands => _continentSettings;
    public LandTypeProperty DefaultLandType => _defaultLandType;
    public int HexCount => _minHexesCount;
}


[Serializable]
public class ContinentSetting
{
    [SerializeField] private LandTypeProperty _landType ;
    [SerializeField, Range(1,100)] private int _percent;

    public LandTypeProperty LandType => _landType;
    public int Percent => _percent;
}