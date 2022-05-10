using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<bool> MouseOnPanel = delegate(bool b) { };

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseOnPanel.Invoke(true);
        Debug.Log("Mouse enter");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        MouseOnPanel.Invoke(false);
        Debug.Log("Mouse exit");
    }
}

   
