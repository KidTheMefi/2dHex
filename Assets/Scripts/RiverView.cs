using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RiverView : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
{
    [SerializeField]
    private LineRenderer _lineRenderer;
    
    IMemoryPool _pool;
    
    
    public void Dispose()
    {
        
        _pool.Despawn(this);
    }

    public void OnDespawned()
    {
        _pool = null;
        _lineRenderer.SetPositions(Array.Empty<Vector3>());
    }

    public void OnSpawned(IMemoryPool pool)
    {
        _pool = pool;
    }

    public void SetRiver(List<Vector3> positions)
    {
        _lineRenderer.endWidth = 0.2f;
        _lineRenderer.positionCount = positions.Count;
        _lineRenderer.SetPositions(positions.ToArray());
    }

    public class Factory : PlaceholderFactory<RiverView>
    {
    }
}
