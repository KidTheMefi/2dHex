using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace GameTime
{
    public enum TimeStates
    {
        Default,
        Fast,
        VeryFast
    }

    public class InGameTime : IInitializable, INightTime, ITickHandler
    {
        public event Action Tick = delegate { };
        public event Action NightTimeChange = delegate { };
        
        private TimeClock _clock;
        private Settings _settings;
        private float _tickTimeModificator = 0.05f;
        private bool _onPause = false;
    
        private bool _nightTime = true;
        private int _dayTimeStart = 36;
        private int _nightTimeStart = 84;
        
        public float TickSeconds => _tickTimeModificator;
    
        private int _timeInTicks = 0;
        /// <summary>
        /// 24 hours = 96 ticks
        /// 1 tick = 15minuts = 1/4 hour
        /// 9 hour = 36 ticks - start Day
        /// 21 hour = 84 ticks - start Night 
        /// </summary>
        private int _midnightTick = 96; 
        public InGameTime(TimeClock clock, Settings settings)
        {
            _clock = clock;
            _settings = settings;
        }

        public void SetTimeState(TimeStates time)
        {
            _tickTimeModificator = _settings.GetTickTime(time);
        }

        public void Initialize()
        {
            _clock.SetTime(_timeInTicks);
            _tickTimeModificator = _settings.GetTickTime(TimeStates.Default);
            DOTween.PauseAll();
        }

        public void Pause(bool value)
        {
            if (value != _onPause)
            {
                if (value)
                {
                    DOTween.PauseAll();
                }
                else
                {
                    DOTween.PlayAll();
                }
                _onPause = value;
            }
        }
    
        public void DoTick()
        {
            Tick.Invoke();
            _timeInTicks++;
            if (_timeInTicks == _nightTimeStart)
            {
                _nightTime = true;
                NightTimeChange.Invoke();
            }
            else if (_timeInTicks == _dayTimeStart)
            {
                _nightTime = false;
                NightTimeChange.Invoke();
            }
            _timeInTicks = _timeInTicks == _midnightTick ? 0 : _timeInTicks;
            _clock.Tick(_timeInTicks, _tickTimeModificator);
        }

        public bool IsNightTime()
        {
            return _nightTime;
        }

        public static string ConvertTicksToHoursAndMinutes(float value)
        {
            int hours = Mathf.FloorToInt(value / 4);
            int minute = Mathf.RoundToInt(value % 4) * 15;
            return hours.ToString("D2") + ":" + minute.ToString("D2");
        }

        [Serializable]
        public class Settings
        {
            [SerializeField, Range(0.01f, 1)] private float _defaultTickTime;
            [SerializeField, Range(0.01f, 1)] private float _restTickTime;
            [SerializeField, Range(0.01f, 1)] private float _sleepTickTime;

            public float GetTickTime(TimeStates time)
            {
                switch (time)
                {
                    case TimeStates.Fast:
                        return _restTickTime;
                    case TimeStates.VeryFast:
                        return _sleepTickTime;
                    default: return _defaultTickTime;
                }
            }
        }
    }
}