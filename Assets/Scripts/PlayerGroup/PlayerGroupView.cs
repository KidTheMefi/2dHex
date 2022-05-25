using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroupView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private ParticleSystem _particleTrails;

    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }

    public void EnableTrails(bool value)
    {
        if (value)
        {
            _particleTrails.Play();
        }
        else
        {
            _particleTrails.Pause();
        }
        
    }
}
