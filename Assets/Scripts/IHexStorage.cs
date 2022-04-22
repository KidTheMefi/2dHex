using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHexStorage
{
    public List<Hex> GetHexesAtAxialCoordinates(List<Vector2Int> axialCoordinates);

    public Vector2Int CenterOfMap();
    public Hex GetHexAtAxialCoordinate(Vector2Int axial);
    public bool HexAtAxialCoordinateExist(Vector2Int axial);

    public Dictionary<Hex, HexView> GetAllTiles();

}
