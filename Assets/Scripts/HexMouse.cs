using System;
using Interfaces;
using UnityEngine;
using Zenject;

public class HexMouse : IHexMouseEvents, IInitializable
{
    public event Action<Vector2Int> HighlightedHexChanged;
    public event Action<Vector2Int> HighlightedHexClicked;

    private Vector2Int _currentHexHighlight;

    private Camera _camera;
    private IHexStorage _iHexStorage;
    private HexHighlight _hexHighlight;
    private TestInputActions _inputActions;
    private PanelScript _panelScript;

    public HexMouse(IHexStorage iHexStorage,
        HexHighlight hexHighlightView,
        TestInputActions inputActions,
        PanelScript panelScript,
        Camera camera)
    {
        _iHexStorage = iHexStorage;
        _hexHighlight = hexHighlightView;
        _inputActions = inputActions;
        _panelScript = panelScript;
        _camera = camera;
    }

    public void Initialize()
    {
        _panelScript.MouseOnPanel += MouseOnPanel;
        _inputActions.CameraInput.MousePosition.performed += context => MouseToHexUpdate();
        _inputActions.CameraInput.LeftClick.performed += context => MouseHexClicked();
    }

    public void MouseToHexUpdate()
    {
        var mousePosition = _camera.ScreenToWorldPoint(_inputActions.CameraInput.MousePosition.ReadValue<Vector2>());
        MouseHexHighlighted(mousePosition);
    }
    
    private void MouseOnPanel(bool value)
    {
        EnableHexHighlight(value);
        if (value)
        {
            _inputActions.CameraInput.MousePosition.Enable();
            _inputActions.CameraInput.LeftClick.Enable();
        }
        else
        {
            _inputActions.CameraInput.MousePosition.Disable();
            _inputActions.CameraInput.LeftClick.Disable();
        }
    }

    private void EnableHexHighlight(bool value)
    {
        _hexHighlight.gameObject.SetActive(value);
    }

    private void MouseHexHighlighted(Vector3 mousePosition)
    {
        var hex = HexUtils.GetAxialFromWorldCoordinates(mousePosition);

        if (_currentHexHighlight != hex && _iHexStorage.HexAtAxialCoordinateExist(hex))
        {
            _currentHexHighlight = hex;
            _hexHighlight.transform.position = _iHexStorage.GetHexAtAxialCoordinate(hex).Position;
            HighlightedHexChanged?.Invoke(hex);
        }
    }

    private void MouseHexClicked()
    {
        HighlightedHexClicked?.Invoke(_currentHexHighlight);
    }
}