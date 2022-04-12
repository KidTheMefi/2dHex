using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class HexMapTiles : MonoBehaviour, IPointerClickHandler
{
    private void OnMouseDown()
    {
        Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickPos.z = 0;
        Debug.Log("click");
        Debug.Log("ClickWorldPosition " + clickPos);
        Vector3Int clickGridPos = _grid.WorldToCell(clickPos);
        Debug.Log("CellPosition " + clickGridPos);
        Debug.Log("CellPositionInWorld " + _grid.CellToWorld(clickGridPos));
        player.position = _grid.CellToWorld(clickGridPos);
    }
    [SerializeField] private Grid _grid;
    [SerializeField] private Transform player;
    [SerializeField] private Tilemap _tilemap;
    private Tile _tile;
    public void Start()
    {
        //Debug.Log(_grid.LocalToCell(player.position));
        //player.position = _grid.LocalToCell(player.position);
        //_tilemap.
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("click");
        Camera.main.ScreenToWorldPoint(eventData.position);
    }
}
