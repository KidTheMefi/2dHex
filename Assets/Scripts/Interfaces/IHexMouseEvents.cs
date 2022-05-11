using System;
using UnityEngine;

namespace Interfaces
{
    public interface IHexMouseEvents 
    {
        public event Action<Vector2Int> HighlightedHexChanged;
        public event Action<Vector2Int> HighlightedHexClicked;
        public event Action HighlightedHexDoubleClicked;
    }
}
