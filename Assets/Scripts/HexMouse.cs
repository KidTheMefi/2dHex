using System;
using Interfaces;
using UnityEngine;

public class HexMouse : IHexMouseEvents
{
    public event Action<Vector2Int> HighlightedHexChanged;
    public event Action<Vector2Int> HighlightedHexClicked;
    public event Action HighlightedHexDoubleClicked;
    private IHexStorage _iHexStorage;

    private Vector2Int _currentHexHighlight;
    private HexHighlight _hexHighlight;

    public HexMouse(IHexStorage iHexStorage, HexHighlight hexHighlightView)
    {
        _iHexStorage = iHexStorage;
        _hexHighlight = hexHighlightView;
    }

    public void EnableHexHighlight(bool value)
    {
        _hexHighlight.gameObject.SetActive(value);
    }
    
    public void MouseHexHighlighted(Vector3 mousePosition)
    {
        var hex = HexUtils.GetAxialFromWorldCoordinates(mousePosition);

        if (_currentHexHighlight != hex && _iHexStorage.HexAtAxialCoordinateExist(hex))
        {
            _currentHexHighlight = hex;
            _hexHighlight.transform.position =  _iHexStorage.GetHexAtAxialCoordinate(hex).Position;
            HighlightedHexChanged?.Invoke(hex);
        }
    }
    
    public void MouseHexClicked()
    {
        HighlightedHexClicked?.Invoke(_currentHexHighlight);
    }
}