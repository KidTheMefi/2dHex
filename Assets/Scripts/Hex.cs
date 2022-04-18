using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


// Q = axialCoordinate.x;
// R = axialCoordinate.y;
public class Hex
{
    public readonly Vector2Int OddOffsetCoordinate;
    public readonly Vector2Int AxialCoordinate;

    private int _movementCost = 1;
    public int MovementCost => _movementCost;
 

    private readonly float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2; // it just work)))

    public Hex(Vector2Int axialCoordinate, Vector2Int oddOffsetCoordinate)
    {
        OddOffsetCoordinate = oddOffsetCoordinate;
        AxialCoordinate = new Vector2Int(axialCoordinate.x, axialCoordinate.y);
    }

    public void SetMovementCost(int cost)
    {
        _movementCost = cost;
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
