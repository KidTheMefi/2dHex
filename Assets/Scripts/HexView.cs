using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HexView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TextMeshPro _coordinate;

    public SpriteRenderer SpriteRenderer => _spriteRenderer;

    public void ViewAxialCoordinate(string coordinate)
    {
        _coordinate.text = coordinate;
    }
}
