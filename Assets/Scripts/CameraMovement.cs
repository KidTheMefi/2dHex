using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();

    private TestInputActions _inputActions;

    private MapBorder _mapBorder;
    private bool _wasdMoving = false;
    private float _cameraDragSpeed = 20f;
    private int _wasdVectorMultiply = 2;
    [SerializeField] private float _cameraZoom;
    [SerializeField] private float _maxZoom;
    [SerializeField] private float _minZoom;

    private void Start()
    {
        _cameraZoom = _camera.orthographicSize;
        _inputActions = new TestInputActions();
        _inputActions.CameraInput.MiddleClickDown.started += MiddleMouseDown;
        _inputActions.CameraInput.MiddleClickDown.canceled += MouseUp;

        _inputActions.CameraInput.WASDPress.started += CameraMoveWASD;
        _inputActions.CameraInput.WASDPress.canceled += x => _wasdMoving = false;

        _inputActions.CameraInput.Scroll.performed += Scroll;
        _inputActions.Enable();
        
    }

    [Inject]
    private void GetBorders(IMapBorder mapBorder)
    {
        _mapBorder = mapBorder.GetMapBorderWorld();
    }

    private async void CameraMoveWASD(InputAction.CallbackContext callbackContext)
    {
        if (!_wasdMoving)
        {
            await AsyncCameraMovementWASD();
        }
    }

    private async UniTask AsyncCameraMovementWASD()
    {
        _wasdMoving = true;
        var pos = transform.position;

        var halfCameraHeight = _camera.orthographicSize;
        //Debug.Log(halfCameraHeight);
        //Debug.Log(halfCameraHeight*_camera.aspect);
        
        await UniTask.WaitUntil(() =>
        {
            Vector2 _wasdVector2 = _inputActions.CameraInput.WASDMove.ReadValue<Vector2>();
            Vector3 targetPoint = transform.position + (Vector3)_wasdVector2 * _wasdVectorMultiply;
            
            /*iif(targetPoint.y + halfCameraHeight  > _mapBorder.YMax || targetPoint.y - halfCameraHeight < _mapBorder.YMin)
            {
                Debug.Log((pos.y + halfCameraHeight) + " / " + _mapBorder.YMax);
                targetPoint.y = pos.y;
            }
            
            f(pos.x + halfCameraHeight*_camera.aspect  > _mapBorder.XMax || pos.x - halfCameraHeight*_camera.aspect < _mapBorder.XMin)
            {
                targetPoint.x = pos.x;
            }*/
            
            float lerpValue = Time.deltaTime * _camera.orthographicSize;

            transform.position = Vector3.Lerp(transform.position, targetPoint, lerpValue);
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

    private async void MiddleMouseDown(InputAction.CallbackContext callbackContext)
    {
        await AsyncCameraMovement();
    }

    private void MouseUp(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed || callbackContext.canceled)
        {
            _cancelTokenSource.Cancel();
        }
    }

    private void CameraZoomSimple()
    {
        var cameraOldPosition = GetWorldPosition(_inputActions.CameraInput.MousePosition.ReadValue<Vector2>());

        _camera.orthographicSize = _cameraZoom;

        var currentPosition = GetWorldPosition(_inputActions.CameraInput.MousePosition.ReadValue<Vector2>());
        transform.position += cameraOldPosition - currentPosition;

    }

    private async UniTask AsyncCameraMovement()
    {
        _cancelTokenSource = new CancellationTokenSource();
        var dragStartPosition = GetWorldPosition(_inputActions.CameraInput.MousePosition.ReadValue<Vector2>());

        
        await UniTask.WaitUntil(() =>
        {
            var currentPosition = GetWorldPosition(_inputActions.CameraInput.MousePosition.ReadValue<Vector2>());
            Vector3 point = transform.position + dragStartPosition - currentPosition;
            transform.position = Vector3.Lerp(transform.position, point, _cameraDragSpeed * Time.deltaTime);

            return _cancelTokenSource.IsCancellationRequested;
        });
    }
}