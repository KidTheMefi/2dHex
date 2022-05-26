using System;
using TMPro;
using UnityEngine;
using Zenject;

public class PathPoint : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
{
    [SerializeField] private TextMeshPro _pathPointText;
    IMemoryPool _pool;

    public void SetText(string text)
    {
        _pathPointText.text = text;
    }


    public void Dispose()
    {
        _pool.Despawn(this);
    }

    public void OnDespawned()
    {
        _pool = null;
        _pathPointText.text = "";
        transform.position = new Vector3();
    }

    public void OnSpawned(IMemoryPool pool)
    {
        _pool = pool;
    }

    public void SetPathPoint(Vector3 position, string text)
    {
        transform.position = position;
        _pathPointText.text = text;
    }

    
    public class Factory : PlaceholderFactory<PathPoint>
    {
    }
}
