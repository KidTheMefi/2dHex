using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MouseInputTest : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _cameraParent;

    private Vector3 _dragStartPosition;
    private Vector3 _currentPosition;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        var worldClickPosition = GetWorldPosition(eventData.position);
        Debug.Log(HexUtils.GetAxialFromWorldCoordinates(worldClickPosition));
    }

    private Vector3 GetWorldPosition(Vector2 screenPosition)
    {
        return _camera.ScreenToWorldPoint(screenPosition);
    }
}
