using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _moveTarget;

    private CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();
    private TestInputActions _inputActions;

    private MapBorder _mapBorder;
    private bool _wasdMoving = false;
    private float _cameraDragSpeed = 20f;
    private int _wasdVectorMultiply = 1;

    [SerializeField] private float _minZoom;
    [SerializeField] private float _cameraZoom;
    private float _maxZoom;
    

    private void Start()
    {
        transform.position = new Vector3((_mapBorder.XMax + _mapBorder.XMin), _mapBorder.YMax + _mapBorder.YMin, transform.position.z) / 2f;

        _maxZoom = (_mapBorder.YMax - _mapBorder.YMin) / 2;
        _camera.orthographicSize = _maxZoom;
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
    private void Init(IMapBorder mapBorder, TestInputActions inputActions)
    {
        _mapBorder = mapBorder.GetMapBorderWorld();
        _inputActions = inputActions;
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
            Vector3 targetPoint = transform.position + (Vector3)wasdVector2 * _wasdVectorMultiply;

            _moveTarget.position = targetPoint + Vector3.forward * 10;

            targetPoint = CameraOnBordersPositionUpdate(targetPoint);

            float lerpValue = Time.deltaTime * _camera.orthographicSize * 2f;

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

    private void MiddleMouseDown(InputAction.CallbackContext callbackContext)
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

    private void CameraZoomSimple()
    {
        var mouseOldPosition = GetWorldPosition(_inputActions.CameraInput.MousePosition.ReadValue<Vector2>());

        _camera.orthographicSize = _cameraZoom;

        var currentPosition = GetWorldPosition(_inputActions.CameraInput.MousePosition.ReadValue<Vector2>());
        var newPos = transform.position + mouseOldPosition - currentPosition;
        transform.position = CameraOnBordersPositionUpdate(newPos);
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
    }
}