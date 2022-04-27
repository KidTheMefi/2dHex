using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Cysharp.Threading.Tasks;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class HexMouseController : IInitializable, ITickable
{
    public event Action<Vector2Int> HighlightedHexChanged;
    public event Action<Vector2Int> HighlightedHexClicked;
    private IHexStorage _iHexStorage;
    private TestInputActions _inputActions;
    private Camera _camera;

    private Vector2Int _currentHexHighlight;
    private Transform _hexHighlightView;

    public HexMouseController(IHexStorage iHexStorage, TestInputActions inputActions, Camera camera, [Inject(Id = "hexHighlight")]Transform hexHighlightView)
    {
        _iHexStorage = iHexStorage;
        _inputActions = inputActions;
        _camera = camera;
        _hexHighlightView = hexHighlightView;
    }

    public void Initialize()
    {
        //_inputActions.CameraInput.MousePosition.performed += MouseHexHighlighted;
        _inputActions.CameraInput.LeftClick.performed += context => MouseHexClicked();
        _inputActions.Enable();
    }

    public void Tick()  // TODO: Tick = Update // maybe need rework
    {
        MouseHexHighlighted();
    }
    
    private void MouseHexHighlighted()
    {
        var mousePosition = _camera.ScreenToWorldPoint((_inputActions.CameraInput.MousePosition.ReadValue<Vector2>()));
        var hex = HexUtils.GetAxialFromWorldCoordinates(mousePosition);

        if (_currentHexHighlight != hex && _iHexStorage.HexAtAxialCoordinateExist(hex))
        {
            _currentHexHighlight = hex;
            _hexHighlightView.position = _iHexStorage.GetHexAtAxialCoordinate(hex).Position;
            HighlightedHexChanged?.Invoke(hex);
        }
    }
    
    private void MouseHexHighlighted(InputAction.CallbackContext callbackContext)
    {
        var hex = HexUtils.GetAxialFromWorldCoordinates(_camera.ScreenToWorldPoint(callbackContext.ReadValue<Vector2>()));

        if (_currentHexHighlight != hex && _iHexStorage.HexAtAxialCoordinateExist(hex))
        {
            _currentHexHighlight = hex;
            _hexHighlightView.position = _iHexStorage.GetHexAtAxialCoordinate(hex).Position;
            HighlightedHexChanged?.Invoke(hex);
        }
    }

    private void MouseHexClicked()
    {
        HighlightedHexClicked?.Invoke(_currentHexHighlight);
    }
}