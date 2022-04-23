using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseInputTest : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Camera _camera;

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
