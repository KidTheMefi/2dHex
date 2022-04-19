using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHexStorage
{
    public List<Hex> GetHexesAtAxialCoordinates(List<Vector2Int> axialCoordinates);

    public Hex GetHexAtAxialCoordinate(Vector2Int axial);
    public Hex GetHexAtOffsetCoordinate(Vector2Int offset);
    public bool HexAtAxialCoordinateExist(Vector2Int axial);
    public bool HexAtOffsetCoordinateExist(Vector2Int offset);
}
