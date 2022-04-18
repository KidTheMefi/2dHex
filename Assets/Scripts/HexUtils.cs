using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexUtils
{
    public static readonly Vector2Int[] AxialDirectionVectors =
    {
        new Vector2Int (1, 0),
        new Vector2Int (1, -1),
        new Vector2Int (0, -1),
        new Vector2Int (-1, 0),
        new Vector2Int (-1, 1),
        new Vector2Int(0, 1)
    };

    public static List<Vector2Int> GetAxialRingWithRadius(Vector2Int centerAxial, int radius)
    {
        var hexes = new List<Vector2Int>();
        var hexAxial = centerAxial + (AxialDirectionVectors[4] * radius);

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < radius; j++)
            {
                hexAxial = AxialNeighbor(hexAxial, i);
                hexes.Add(hexAxial);
            }
        }
        return hexes;
    }

    public static List<Vector2Int> GetAxialAreaAtRange(Vector2Int centerAxial, int range)
    {
        var hexes = new List<Vector2Int>();
        for (int q = -range; q <= range; q++)
        {
            for (int r = Mathf.Max(-range, -q - range); r <= Mathf.Min(range, -q + range); r++)
            {
                Vector2Int hexAxial = new Vector2Int(centerAxial.x + q, centerAxial.y + r);
                hexes.Add(hexAxial);
            }
        }
        return hexes;
    }

    public static List<Vector2Int> GetAxialLine(Vector2Int start, Vector2Int end)
    {
        //HAVE PROBLEMS AT THE EDGE OF MAP!!!
        // TODO: SMTHG

        int n = AxialDistance(start, end);
        List<Vector2Int> hexes = new List<Vector2Int>();
        Vector2 nudge = new Vector2(0.00001f, 0.00001f); //to round point that’s on an edge
        float step = 1f / Mathf.Max(n, 1);

        for (int i = 0; i <= n; i++)
        {
            hexes.Add(AxialRound(Vector2.Lerp(start + nudge, end+ nudge, step * i)));
        }
        return hexes;
    }

    public static Vector2Int AxialRound(Vector2 axial)
    {
        return CubeToAxial(CubeRound(AxialToCube(axial)));
    }

    public static Vector3Int CubeRound(Vector3 cube)
    {
        int x = Mathf.RoundToInt(cube.x);
        int y = Mathf.RoundToInt(cube.y);
        int z = Mathf.RoundToInt(cube.z);

        float xDiff = Mathf.Abs((x - cube.x));
        float yDiff = Mathf.Abs((y - cube.y));
        float zDiff = Mathf.Abs((z - cube.z));

        if (xDiff > yDiff && xDiff > zDiff)
        {
            x = -y - z;
        }
        else if (yDiff > zDiff)
        {
            y = -x - z;
        }
        else
        {
            z = -x - y;
        }
        return new Vector3Int(x, y, z);
    }

    public static Vector2Int CubeToAxial(Vector3Int cube)
    {
        return new Vector2Int(cube.x, cube.y);
    }

    public static Vector3 AxialToCube(Vector2 axial)
    {
        return new Vector3(axial.x, axial.y, -axial.x - axial.y);
    }

    public static Vector2Int AxialNeighbor(Vector2Int hexAxial, int direction)
    {
        return (hexAxial + AxialDirectionVectors[direction]);
    }

    public static int AxialDistance(Vector2Int a, Vector2Int b)
    {
        Vector2Int vec = a - b;
        return (Mathf.Abs(vec.x) + Mathf.Abs(vec.x + vec.y) + Mathf.Abs(vec.y)) / 2;
    }

    public static Vector2Int AxialToOffsetOdd(Vector2Int axialCordinates)
    {
        var col = axialCordinates.x + (axialCordinates.y - (axialCordinates.y & 1)) / 2;
        return new Vector2Int(col, axialCordinates.y);
    }

    public static Vector2Int OffsetOddToAxial(Vector2Int oddCordinates)
    {
        var q = oddCordinates.x - (oddCordinates.y - (oddCordinates.y & 1)) / 2;
        return new Vector2Int(q, oddCordinates.y);
    }

    public static Vector2Int OffsetOddToAxial(int x, int y)
    {
        var q = x - (y - (y & 1)) / 2;
        return new Vector2Int(q, y);
    }

    public static Vector2Int AxialToOffsetOdd(int x, int y)
    {
        var col = x + (y - (y & 1)) / 2;
        return new Vector2Int(col, y);
    }

    public static Vector2Int RandomAxialDirection()
    {
        return AxialDirectionVectors[Random.Range(0, 6)];
    }

}