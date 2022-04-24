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
    
    private float _cameraDragSpeed = 20f;
    [SerializeField] private float _cameraZoom;
    [SerializeField] private float _maxZoom;
    [SerializeField] private float _minZoom;

    public Vector2 scrollTest;
    
    private void Awake()
    {
        _cameraZoom = _camera.orthographicSize;
        _inputActions = new TestInputActions();
        _inputActions.MouseInput.MiddleClickDown.performed += MiddleMouseDown;
        _inputActions.MouseInput.MiddleClickUp.performed += MouseUp;
        
        _inputActions.MouseInput.Scroll.performed += context => Scroll(context);
        _inputActions.Enable();
    }


    private void Scroll(InputAction.CallbackContext callbackContext)
    {
        
        scrollTest = callbackContext.ReadValue<Vector2>().normalized/2;
       var zoomAdd = scrollTest.y * Mathf.Sqrt(_cameraZoom)/2;
       
       
       _cameraZoom -= zoomAdd;

        if (_cameraZoom < _minZoom)
        {
            _cameraZoom = _minZoom;
        }
        else if (_cameraZoom > _maxZoom)
        {
            _cameraZoom = _maxZoom;
        }
        
        CameraZoomSimple();
    }
    
    private Vector3 GetWorldPosition(Vector2 screenPosition)
    {
        return _camera.ScreenToWorldPoint(screenPosition);
        
    }

    private async void MiddleMouseDown(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            await AsyncCameraMovement();
        }
    }
    
    private void MouseUp(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed || callbackContext.canceled)
        {
            _cancelTokenSource.Cancel();
        }
    }

    private void CameraMovementTest()
    {
        
    }

    private void CameraZoomSimple()
    {
        _dragStartPosition = GetWorldPosition(_inputActions.MouseInput.MousePosition.ReadValue<Vector2>());

        _camera.orthographicSize = _cameraZoom;
        
        _currentPosition = GetWorldPosition(_inputActions.MouseInput.MousePosition.ReadValue<Vector2>());
        transform.position +=  _dragStartPosition - _currentPosition;
        
    }

    private async UniTask AsyncCameraMovement()
    {
        _cancelTokenSource = new CancellationTokenSource();
        _dragStartPosition = GetWorldPosition(_inputActions.MouseInput.MousePosition.ReadValue<Vector2>());
        
        await UniTask.WaitUntil(() =>
        {
            _currentPosition = GetWorldPosition(_inputActions.MouseInput.MousePosition.ReadValue<Vector2>());
            Vector3 point = transform.position + _dragStartPosition - _currentPosition;
            transform.position =Vector3.Lerp(transform.position, point, _cameraDragSpeed*Time.deltaTime); 
            
            return _cancelTokenSource.IsCancellationRequested;
        });
    }
    
  
    
}
