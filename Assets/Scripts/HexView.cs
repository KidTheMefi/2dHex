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
    [SerializeField] private Color _fogColor;

    private bool _fogOn = true;
    public void SetHexViewSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }

    public void ChangeFogStatus()
    {
        _fogOn = !_fogOn;

        _spriteRenderer.color = Color.white;
        if (_fogOn)
        {
            _fogRenderer.enabled = _fogOn;
            _spriteRenderer.sortingLayerID = SortingLayer.NameToID("MapTiles");
        }
        else
        {
            _fogRenderer.enabled = _fogOn;
        }
    }
    
    public void SetHexVisible(bool set)
    {
        if (_fogOn)
        {
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