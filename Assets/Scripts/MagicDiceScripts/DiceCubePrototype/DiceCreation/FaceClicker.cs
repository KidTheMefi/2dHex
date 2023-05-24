using System.Collections;
using System.Collections.Generic;
using DiceCubePrototype;
using UnityEngine;
using UnityEngine.EventSystems;

public class FaceClicker : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Camera _mainCamera;
    [SerializeField]
    private LayerMask _selectMask;
    [SerializeField]
    private int _raycastDistance;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        var mousePos = _mainCamera.ScreenToWorldPoint(eventData.position);
        RaycastClick(mousePos);
    }

    private void RaycastClick(Vector3 pos)
    {
        var hitResult = Physics.Raycast(pos, Vector3.forward, out var raycastHit, _raycastDistance, _selectMask);

        if (!hitResult)
        {
            return;
        }
        if (raycastHit.collider.TryGetComponent<DiceFaceInCreation>(out var diceFace))
        {
            diceFace.Selected(true);
        }
    }
}