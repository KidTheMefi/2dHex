using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class CameraMovement : IInitializable
{
    private Camera _camera;
    

    private MapBorder _mapBorder;
    private HexMouse _hexMouse;
    private TestInputActions _inputActions;
    private bool _wasdMoving = false;
    
    private int _wasdVectorMultiply = 1;
    private float _minZoom = 3;
    private float _cameraZoom;
    private float _maxZoom;
    //private float _cameraDragSpeed = 20f;
    
    public CameraMovement(IMapBorder mapBorder, Camera camera, HexMouse hexMouse, TestInputActions inputActions)
    {
        _mapBorder = mapBorder.GetMapBorderWorld();
        _camera = camera;
        _hexMouse = hexMouse;
        _inputActions = inputActions;
    }

    public void Initialize()
    {
        _camera.transform.position = new Vector3((_mapBorder.XMax + _mapBorder.XMin), _mapBorder.YMax + _mapBorder.YMin, _camera.transform.position.z) / 2f;

        _maxZoom = (_mapBorder.YMax - _mapBorder.YMin) / 2;
        _camera.orthographicSize = _maxZoom;
        _cameraZoom = _camera.orthographicSize;

        _inputActions.CameraInput.WASDPress.started += CameraMoveWASD;
        _inputActions.CameraInput.WASDPress.canceled += x => _wasdMoving = false;

        _inputActions.CameraInput.Scroll.performed += Scroll;
        _inputActions.CameraInput.Scroll.Enable();
        _inputActions.Enable();

    }

    private void CameraMoveWASD(InputAction.CallbackContext callbackContext)
    {
        if (!_wasdMoving)
        {
            AsyncCameraMovementWASD().Forget();
        }
    }

    private async UniTask AsyncCameraMovementWASD()
    {
        _wasdMoving = true;
        await UniTask.WaitUntil(() =>
        {
            Vector2 wasdVector2 = _inputActions.CameraInput.WASDMove.ReadValue<Vector2>();
            Vector3 targetPoint = _camera.transform.position + (Vector3)wasdVector2 * _wasdVectorMultiply;

            targetPoint = CameraOnBordersPositionUpdate(targetPoint);

            float lerpValue = Time.deltaTime * _camera.orthographicSize * 2f;

            _camera.transform.position = Vector3.Lerp(_camera.transform.position, targetPoint, lerpValue);
            _hexMouse.MouseToHexUpdate();
            return !_wasdMoving;
        });
    }

    private void Scroll(InputAction.CallbackContext callbackContext)
    {
        var scrollTest = callbackContext.ReadValue<Vector2>().normalized;
        var zoomAdd = scrollTest.y * Mathf.Sqrt(_cameraZoom) / 4;

        _cameraZoom -= zoomAdd;
        _cameraZoom = _cameraZoom < _minZoom ? _minZoom : _cameraZoom > _maxZoom ? _maxZoom : _cameraZoom;

        CameraZoomSimple();
    }

    private Vector3 GetWorldPosition(Vector2 screenPosition)
    {
        return _camera.ScreenToWorldPoint(screenPosition);
    }

    private void CameraZoomSimple()
    {
        var mouseOldPosition = GetWorldPosition(_inputActions.CameraInput.MousePosition.ReadValue<Vector2>());

        _camera.orthographicSize = _cameraZoom;

        var currentPosition = GetWorldPosition(_inputActions.CameraInput.MousePosition.ReadValue<Vector2>());
        var newPos = _camera.transform.position + mouseOldPosition - currentPosition;
        _camera.transform.position = CameraOnBordersPositionUpdate(newPos);
        _hexMouse.MouseToHexUpdate();
    }

    private Vector3 CameraOnBordersPositionUpdate(Vector3 pos)
    {
        //TODO: maybe smth with less IF
        Vector3 newCamPos = pos;

        if (pos.y > _mapBorder.YMax - _camera.orthographicSize)
        {
            newCamPos.y = _mapBorder.YMax - _camera.orthographicSize;
        }
        if (pos.y < _mapBorder.YMin + _camera.orthographicSize)
        {
            newCamPos.y = _mapBorder.YMin + _camera.orthographicSize;
        }

        if (pos.y > _mapBorder.YMax - _camera.orthographicSize && pos.y < _mapBorder.YMin + _camera.orthographicSize)
        {
            newCamPos.y = (_mapBorder.YMax + _mapBorder.YMin) / 2;
        }

        if (pos.x + _camera.orthographicSize * _camera.aspect > _mapBorder.XMax)
        {
            newCamPos.x = _mapBorder.XMax - _camera.orthographicSize * _camera.aspect;
        }
        if (pos.x - _camera.orthographicSize * _camera.aspect < _mapBorder.XMin)
        {
            newCamPos.x = _mapBorder.XMin + _camera.orthographicSize * _camera.aspect;
        }

        if (pos.x + _camera.orthographicSize * _camera.aspect > _mapBorder.XMax && pos.x - _camera.orthographicSize * _camera.aspect < _mapBorder.XMin)
        {
            newCamPos.x = (_mapBorder.XMax + _mapBorder.XMin) / 2;
        }
        return newCamPos;
    }

    #region Mouse "Drag" Camera

    /*private void MiddleMouseDown(InputAction.CallbackContext callbackContext)
    {
        AsyncCameraMovement().Forget();
    }
    
    private void MouseUp(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed || callbackContext.canceled)
        {
            _cancelTokenSource.Cancel();
        }
    }
    private async UniTask AsyncCameraMovement()
    {
        _cancelTokenSource = new CancellationTokenSource();
        var dragStartPosition = GetWorldPosition(_inputActions.CameraInput.MousePosition.ReadValue<Vector2>());
    
        await UniTask.WaitUntil(() =>
        {
            var currentPosition = GetWorldPosition(_inputActions.CameraInput.MousePosition.ReadValue<Vector2>());
            Vector3 point = transform.position + dragStartPosition - currentPosition;
            point = CameraOnBordersPositionUpdate(point);
            transform.position = Vector3.Lerp(transform.position, point, _cameraDragSpeed * Time.deltaTime);
    
            return _cancelTokenSource.IsCancellationRequested;
        });
    }*/

  #endregion


}