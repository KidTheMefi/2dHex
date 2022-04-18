using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeightedGraph<TL>
{
    int Cost(Vector2Int axial);
    IEnumerable<TL> Neighbors(TL id);
}
