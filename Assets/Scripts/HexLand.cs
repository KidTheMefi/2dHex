using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HexLand
{
    [SerializeField] private LandType landType;
    [SerializeField] private List<Sprite> _sprites;

    public LandType LandType => landType;
    public List<Sprite> Sprites => _sprites;
}
