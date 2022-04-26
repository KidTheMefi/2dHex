using System.Collections.Generic;
using UnityEngine;

namespace Interfaces
{
    public interface IHexStorage
    {
        public List<Hex> GetHexesAtAxialCoordinates(List<Vector2Int> axialCoordinates);

        public Vector2Int CenterOfMap();
        public Hex GetHexAtAxialCoordinate(Vector2Int axial);
        public bool HexAtAxialCoordinateExist(Vector2Int axial);

        public Dictionary<Hex, HexView> HexToHexView();
        public Dictionary<HexView, Hex> HexViewToHex();

    }
}
