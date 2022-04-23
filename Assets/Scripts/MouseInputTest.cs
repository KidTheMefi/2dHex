using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MouseInputTest : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IDragHandler, IBeginDragHandler
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
    
    public void OnPointerDown(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }
    
    private Vector3 GetWorldPosition(Vector2 screenPosition)
    {
        return _camera.ScreenToWorldPoint(screenPosition);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Drag begin");
        _dragStartPosition= GetWorldPosition(eventData.position);
        //_clickDropPosition = GetWorldPosition(eventData.position);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        _currentPosition = _camera.ScreenToWorldPoint(eventData.position);
        Vector3 point =  _cameraParent.position + _dragStartPosition - _currentPosition;
        point.z = _cameraParent.transform.position.z;
        _cameraParent.position = point;
    }
    
}
