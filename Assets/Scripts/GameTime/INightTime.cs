using System;
using UnityEngine;

namespace GameTime
{
    public interface INightTime
    {
        public event Action NightTimeChange;
        public bool IsNightTime();
    }
}
