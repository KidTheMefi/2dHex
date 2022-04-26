using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroupView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _selectView;
        
    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }

    public void ShowSelect(bool view)
    {
        _selectView.SetActive(view);
    }
}
