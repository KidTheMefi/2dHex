using System;

namespace GameTime
{
    public interface ITickHandler 
    {
        public event Action Tick;
    }
}
