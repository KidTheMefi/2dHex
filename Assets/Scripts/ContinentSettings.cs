using System;
using System.Collections.Generic;
using UnityEngine;

public enum BiomType
{
    None, Grass, Winter, Desert 
}

[CreateAssetMenu(menuName = "HexGame/ContinentSettings")]
public class ContinentSettings : ScriptableObject
{
    [SerializeField] private BiomType _biomType;
    [SerializeField] private List<ContinentLandTypeSetting> _landsTypeSettings;
    [SerializeField] private LandTypeProperty _defaultLandType;
    
    public BiomType BiomType => _biomType;
    public List<ContinentLandTypeSetting> Lands => _landsTypeSettings;
    public LandTypeProperty DefaultLandType => _defaultLandType;
}

[Serializable]
public class ContinentLandTypeSetting
{
    [SerializeField] private LandTypeProperty _landType ;
    [SerializeField, Range(1,100)] private int _percent;

    public LandTypeProperty LandType => _landType;
    public int Percent => _percent;
}