using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class HexView : MonoBehaviour
{
    public event Action<HexView> OnMouseEnterAction = delegate(HexView view) { };
    public event Action<HexView> OnMouseDownAction = delegate(HexView view) { };
    
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TextMeshPro _coordinate;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private PolygonCollider2D _collider;


    private void OnMouseEnter()
    {
        OnMouseEnterAction.Invoke(this);
    }

    private void OnMouseDown()
    {
        Debug.Log("Click");
        OnMouseDownAction.Invoke(this);
    }

    public void SetHexViewSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }
    
    public void SetMeshRendererActive(bool set)
    {
        _meshRenderer.enabled = set;
    }
    public void TextAtHex(string coordinate)
    {
        _coordinate.text = coordinate;
    }
    
    public class Factory : PlaceholderFactory<HexView>
    {
        
    }

}
