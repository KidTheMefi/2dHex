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
    
    CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
    
    private TestInputActions _inputActions;
    
    private Vector3 _dragStartPosition;
    private Vector3 _currentPosition;
    
    private void Awake()
    {
        _inputActions = new TestInputActions();
        _inputActions.MouseInput.MapClick.performed += MouseDown;
        _inputActions.MouseInput.MapClickUp.performed += MouseUp;
        
        _inputActions.Enable();
    }
    
    private Vector3 GetWorldPosition(Vector2 screenPosition)
    {
        return _camera.ScreenToWorldPoint(screenPosition);
    }

    private async void MouseDown(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            Debug.Log("Click performed");
            await AsyncCameraMovement();
        }
    }
    
    private void MouseUp(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed || callbackContext.canceled)
        {
            Debug.Log("mouse up");
            cancelTokenSource.Cancel();
        }
    }
    
    private async UniTask AsyncCameraMovement()
    {
        cancelTokenSource = new CancellationTokenSource();
        _dragStartPosition = GetWorldPosition(_inputActions.MouseInput.MapClickVector.ReadValue<Vector2>());
        
        await UniTask.WaitUntil(() =>
        {
            _currentPosition = GetWorldPosition(_inputActions.MouseInput.MapClickVector.ReadValue<Vector2>());
            Vector3 point = transform.position + _dragStartPosition - _currentPosition;
            transform.position =Vector3.Lerp(transform.position, point, 20f*Time.deltaTime); 
            
            return cancelTokenSource.IsCancellationRequested;
        });
    }
    
  
    
}
