using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class HexView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TextMeshPro _coordinate;
    [SerializeField] private MeshRenderer _meshRenderer;

    //public SpriteRenderer SpriteRenderer => _spriteRenderer;

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
