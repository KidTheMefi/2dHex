using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class HexView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TextMeshPro _coordinate;
    [SerializeField] private SpriteRenderer _fogRenderer;
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
        //_spriteRenderer.sprite = sprite;
    }

    public void ChangeFogStatus()
    {
        _fogOn = !_fogOn;
        
        if (_fogOn)
        {
            _spriteRenderer.color = _darkFogColor;
            _spriteRenderer.sprite = _hexFogSprite;
            _fogRenderer.enabled = _fogOn;
            _fogCollider.enabled = _fogOn;
            _spriteRenderer.sortingLayerID = SortingLayer.NameToID("Fog");
        }
        else
        {
            _spriteRenderer.sortingLayerID = SortingLayer.NameToID("MapTiles");
            _spriteRenderer.color = Color.white;
            _spriteRenderer.sprite = _hexSprite;
            _fogRenderer.enabled = _fogOn;
            _fogCollider.enabled = _fogOn;
        }
    }
    
    public void SetHexVisible(bool set)
    {
        if (_fogOn)
        {
            _spriteRenderer.sprite = _hexSprite;
            
            if (set)
            {
                _spriteRenderer.color = _fogColor;
                _spriteRenderer.sortingLayerID = SortingLayer.NameToID("MapTilesInvisible");
            }
            else
            {
                _fogRenderer.enabled = set;
                _spriteRenderer.color = Color.white;
                _spriteRenderer.sortingLayerID = SortingLayer.NameToID("MapTiles");
            }
            _fogCollider.enabled = set;
        }
    }

    public void TextAtHex(string coordinate)
    {
        _coordinate.text = coordinate;
    }

    public class Factory : PlaceholderFactory<HexView>
    {

    }
}