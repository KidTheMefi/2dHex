using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();

    private TestInputActions _inputActions;

    private Vector3 _dragStartPosition;
    private Vector3 _currentPosition;

    private bool _wasdMoving = false;
    private float _cameraDragSpeed = 20f;
    private int _wasdVectorMultiply = 2;
    [SerializeField] private float _cameraZoom;
    [SerializeField] private float _maxZoom;
    [SerializeField] private float _minZoom;

    public Vector2 scrollTest;

    private void Awake()
    {
        _cameraZoom = _camera.orthographicSize;
        _inputActions = new TestInputActions();
        _inputActions.MouseInput.MiddleClickDown.started += MiddleMouseDown;
        _inputActions.MouseInput.MiddleClickDown.canceled += MouseUp;

        _inputActions.MouseInput.WASDPress.started += CameraMoveWASD;
        _inputActions.MouseInput.WASDPress.canceled += x => _wasdMoving = false;

        _inputActions.MouseInput.Scroll.performed += Scroll;
        _inputActions.Enable();
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
        _cancelTokenSource = new CancellationTokenSource();
        _wasdMoving = true;
        _dragStartPosition = GetWorldPosition(_inputActions.MouseInput.MousePosition.ReadValue<Vector2>());

        await UniTask.WaitUntil(() =>
        {
            Vector2 _wasdVector2 = _inputActions.MouseInput.WASDMove.ReadValue<Vector2>();
            Vector3 point = transform.position + (Vector3)_wasdVector2 * _wasdVectorMultiply;
            float lerpValue = Time.deltaTime * _camera.orthographicSize;

            transform.position = Vector3.Lerp(transform.position, point, lerpValue);

            return !_wasdMoving;
        });
    }

    private void Scroll(InputAction.CallbackContext callbackContext)
    {
        scrollTest = callbackContext.ReadValue<Vector2>().normalized;
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
        _dragStartPosition = GetWorldPosition(_inputActions.MouseInput.MousePosition.ReadValue<Vector2>());

        _camera.orthographicSize = _cameraZoom;

        _currentPosition = GetWorldPosition(_inputActions.MouseInput.MousePosition.ReadValue<Vector2>());
        transform.position += _dragStartPosition - _currentPosition;

    }

    private async UniTask AsyncCameraMovement()
    {
        _cancelTokenSource = new CancellationTokenSource();
        _dragStartPosition = GetWorldPosition(_inputActions.MouseInput.MousePosition.ReadValue<Vector2>());

        await UniTask.WaitUntil(() =>
        {
            _currentPosition = GetWorldPosition(_inputActions.MouseInput.MousePosition.ReadValue<Vector2>());
            Vector3 point = transform.position + _dragStartPosition - _currentPosition;
            transform.position = Vector3.Lerp(transform.position, point, _cameraDragSpeed * Time.deltaTime);

            return _cancelTokenSource.IsCancellationRequested;
        });
    }



}