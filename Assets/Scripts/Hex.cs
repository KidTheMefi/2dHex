using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

public class Hex 
{
    public event Action<Hex, Sprite> LandTypeSpriteChanged = delegate { };
    public event Action<Hex> OnMouseEnter = delegate { };
    
    public readonly Vector2Int AxialCoordinate;
    public readonly Vector3 Position;
    
    private LandTypeProperty _landTypeProperty;
    public LandTypeProperty LandTypeProperty => _landTypeProperty;

    public void SetLandTypeProperty(LandTypeProperty landTypeProperty)
    {
        _landTypeProperty = landTypeProperty;
        LandTypeSpriteChanged.Invoke(this, landTypeProperty.GetSprite());
    }

    public Hex(Vector2Int axialCoordinate)
    {
        Position = HexUtils.CalculatePosition(axialCoordinate);
    }

}
