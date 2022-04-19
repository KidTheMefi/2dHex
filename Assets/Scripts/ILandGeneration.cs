using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILandGeneration
{
    public List<Vector2Int> CreateContinentLand(Vector2Int center, int startScale, int minTilesNumber, List<Vector2Int> unavailableHexes = null);
    public List<Vector2Int> CreateLandTypeAtContinent(List<Vector2Int> continent, int minTilesNumber);
}
