using System;
using UnityEngine;

namespace Interfaces
{
    public interface IPlayerGroupEvents
    {
        public event Action<Vector2Int> StoppedOnPosition;
    }
}