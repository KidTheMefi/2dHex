using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

public class Hex 
{
    private readonly float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2; // it just work)))
    
    public readonly Vector2Int OddOffsetCoordinate;
    public readonly Vector2Int AxialCoordinate;
    public readonly Vector3 Position;
    
    private LandTypeProperty _landTypeProperty;
    public LandTypeProperty LandTypeProperty => _landTypeProperty;

    public void SetLandTypeProperty(LandTypeProperty landTypeProperty)
    {
        _landTypeProperty = landTypeProperty;
    }
    

    public Hex(Vector2Int axialCoordinate, Vector2Int oddOffsetCoordinate)
    {
        OddOffsetCoordinate = oddOffsetCoordinate;
        AxialCoordinate = new Vector2Int(axialCoordinate.x, axialCoordinate.y);

        Position = CalculatePosition();
    }


    private Vector3 CalculatePosition()
    {
        float radius = 0.5f; // =0.5 to clean grid 
        //float radius = 0.525f; // small grid
        float height = radius * 2;
        float width = WIDTH_MULTIPLIER * height;

        float vert = height * 0.74f;
        float horiz = width;

        return new Vector3(horiz * (AxialCoordinate.x + AxialCoordinate.y / 2f), vert * AxialCoordinate.y, 0);
    }
    
}
