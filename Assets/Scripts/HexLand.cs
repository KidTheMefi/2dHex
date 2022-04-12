using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HexLand
{
    [SerializeField] private TerrainType _terrainType;
    [SerializeField] private List<Sprite> _sprites;

    public TerrainType TerrainType => _terrainType;
    public List<Sprite> Sprites => _sprites;
}
