using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;


// Q = axialCoordinate.x;
// R = axialCoordinate.y;
public class Hex
{
    public readonly Vector2Int OddOffsetCoordinate;
    public readonly Vector2Int AxialCoordinate;

    private int _movementCost = 1;
    private LandType _hexLandTypeHex = LandType.Water;
    public int MovementCost => _movementCost;
    
    public LandType LandTypeHex => _hexLandTypeHex;
 

    private readonly float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2; // it just work)))

    public Hex(Vector2Int axialCoordinate, Vector2Int oddOffsetCoordinate)
    {
        OddOffsetCoordinate = oddOffsetCoordinate;
        AxialCoordinate = new Vector2Int(axialCoordinate.x, axialCoordinate.y);
    }

    public bool IsPassible()
    {
        switch (LandTypeHex)
        {
            case LandType.Mountain:
                return false;
            case LandType.Water:
                return false;
            case LandType.SnowMountain:
                return false;
            default: return true;
        }
    }
    
    public void SetLandType(LandType landType)
    {
        _hexLandTypeHex = landType;
    }

    public int GetMovementCost()
    {
        switch (LandTypeHex)
        {
            case LandType.Grass:
                return 1;
            case LandType.Forrest:
                return 3;
            case LandType.Hill:
                return 5;
            case LandType.Snow:
                return 2;
            case LandType.SnowForrest:
                return 4;
            case LandType.SnowHill:
                return 6;
            default:
                return 1;
        }
    }

    public Vector3 Position()
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
