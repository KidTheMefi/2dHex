using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class HexView : MonoBehaviour, IAsyncOnMouseEnterHandler
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TextMeshPro _coordinate;
    [SerializeField] private MeshRenderer _meshRenderer;

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

    public UniTask OnMouseEnterAsync()
    {
        throw new NotImplementedException();
    }
    
}
