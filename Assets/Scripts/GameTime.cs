using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameTime
{
    private TimeClock _clock;
    public static readonly float MovementTimeModificator = 0.3f;
    public event Action Tick = delegate { };
    
    private bool timePlay;
    public GameTime(TimeClock clock)
    {
        _clock = clock;
    }

    public void DoTick()
    {
        Tick.Invoke();
        _clock.Tick().Forget();
    }
}
