using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

public class Hex 
{
    public event Action<Hex, Sprite> LandTypeSpriteChanged = delegate { };
    
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
        Position = CalculatePosition(axialCoordinate);
    }

    private Vector3 CalculatePosition( Vector2Int axialCoordinate)
    {
        float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2; // it just work)))
        float radius = 0.5f; // =0.5 to clean grid 
        //float radius = 0.525f; // small grid
        float height = radius * 2;
        float width = WIDTH_MULTIPLIER * height;

        float vert = height * 0.74f;
        float horiz = width;

        return new Vector3(horiz * (axialCoordinate.x + axialCoordinate.y / 2f), vert * axialCoordinate.y, 0);
    }
    
}
