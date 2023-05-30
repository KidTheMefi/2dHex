using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class HexView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TextMeshPro _coordinate;
    [SerializeField] private Collider2D _fogCollider;
    [SerializeField] private Color _darkFogColor;
    [SerializeField] private Color _fogColor;
    [SerializeField] private Sprite _hexFogSprite;

    private Sprite _hexSprite;
    
    private bool _fogOn = false;

    public void SetHexViewSprite(Sprite sprite)
    {
        //_spriteRenderer.sprite = sprite;
        _hexSprite = sprite;
    }

    public void SetTileVisible(TileVisibility tileVisibility)
    {
        switch (tileVisibility)
        {
            case TileVisibility.Discovered:
                _spriteRenderer.sprite = _hexSprite;
                _spriteRenderer.color = _fogColor;
                _spriteRenderer.sortingLayerID = SortingLayer.NameToID("MapTilesInvisible");
                _fogCollider.enabled = false;
                break;
            case TileVisibility.VisibleAlways:
            case TileVisibility.VisibleNow:
                _spriteRenderer.sprite = _hexSprite;
                _spriteRenderer.color = Color.white;
                _spriteRenderer.sortingLayerID = SortingLayer.NameToID("MapTiles");
                _fogCollider.enabled = true;
                break;
            case TileVisibility.Undiscovered:
                _spriteRenderer.color = _darkFogColor;
                _spriteRenderer.sprite = _hexFogSprite;
                _spriteRenderer.sortingLayerID = SortingLayer.NameToID("Fog");
                _fogCollider.enabled = true;
                break;
        }
        
    }
    
    /*
    public void ChangeFogStatus(bool value = false)
    {
        
        _fogOn = value ?  value : !_fogOn;

            if (_fogOn)
            {
                _spriteRenderer.color = _darkFogColor;
                _spriteRenderer.sprite = _hexFogSprite;
                _spriteRenderer.sortingLayerID = SortingLayer.NameToID("Fog");
            }
            else
            {
                _spriteRenderer.color = Color.white;
                _spriteRenderer.sprite = _hexSprite;
                _spriteRenderer.sortingLayerID = SortingLayer.NameToID("MapTiles");
            }
            _fogCollider.enabled = _fogOn;
    }
    
    public void SetHexVisible(bool set)
    {
        if (_fogOn)
        {
            _spriteRenderer.sprite = _hexSprite;
            
            if (set)
            {
                _spriteRenderer.color = Color.white;
                _spriteRenderer.sortingLayerID = SortingLayer.NameToID("MapTiles");
            }
            else
            {
                _spriteRenderer.color = _fogColor;
                _spriteRenderer.sortingLayerID = SortingLayer.NameToID("MapTilesInvisible");
            }
            _fogCollider.enabled = set;
        }
    }*/

    public void TextAtHex(string coordinate)
    {
        _coordinate.text = coordinate;
    }

    public class Factory : PlaceholderFactory<HexView>
    {

    }
}