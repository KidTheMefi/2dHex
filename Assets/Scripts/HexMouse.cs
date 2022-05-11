using System;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class HexMouse : IHexMouseEvents
{
    public event Action<Vector2Int> HighlightedHexChanged;
    public event Action<Vector2Int> HighlightedHexClicked;
    public event Action HighlightedHexDoubleClicked;
    private IHexStorage _iHexStorage;

    private Vector2Int _currentHexHighlight;
    private Vector2Int _hexClicked;
    private Transform _hexHighlightView;

    public HexMouse(IHexStorage iHexStorage, [Inject(Id = "hexHighlight")]Transform hexHighlightView)
    {
        _iHexStorage = iHexStorage;
        _hexHighlightView = hexHighlightView;
    }
    
    
    public void MouseHexHighlighted(Vector3 mousePosition)
    {
        
        var hex = HexUtils.GetAxialFromWorldCoordinates(mousePosition);

        if (_currentHexHighlight != hex && _iHexStorage.HexAtAxialCoordinateExist(hex))
        {
            _currentHexHighlight = hex;
            //_hexHighlightView.position = HexUtils.CalculatePosition(hex);
            _hexHighlightView.position =  _iHexStorage.GetHexAtAxialCoordinate(hex).Position;
            HighlightedHexChanged?.Invoke(hex);
        }
    }
    
    public void MouseHexClicked()
    {
        if (_hexClicked != _currentHexHighlight)
        { 
            HighlightedHexClicked?.Invoke(_currentHexHighlight);
        }
        else
        {
            HighlightedHexDoubleClicked?.Invoke();
        }
        
        _hexClicked = _currentHexHighlight;
    }
    
    /*private void MouseHexHighlighted(InputAction.CallbackContext callbackContext)
    {
        var hex = HexUtils.GetAxialFromWorldCoordinates(_camera.ScreenToWorldPoint(callbackContext.ReadValue<Vector2>()));

        if (_currentHexHighlight != hex && _iHexStorage.HexAtAxialCoordinateExist(hex))
        {
            _currentHexHighlight = hex;
            _hexHighlightView.position = _iHexStorage.GetHexAtAxialCoordinate(hex).Position;
            HighlightedHexChanged?.Invoke(hex);
        }
    }*/

   
}