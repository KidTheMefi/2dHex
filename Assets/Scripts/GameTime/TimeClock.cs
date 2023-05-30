using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace GameTime
{
    public class TimeClock : MonoBehaviour
    {
        [SerializeField]
        private Transform _arrowHour;
        [SerializeField]
        private Transform _arrowMinuts;
        [SerializeField]
        private TextMeshPro _timeText;

        public async void Tick(int timeInTicks, float tickSeconds)
        {
        
            int hours = timeInTicks;
            int minute = Mathf.RoundToInt(timeInTicks % 4);
        
            _arrowHour.DORotateQuaternion(Quaternion.Euler(Vector3.back*hours*7.5f), tickSeconds).SetEase(Ease.Linear);
            _arrowMinuts.DORotateQuaternion(Quaternion.Euler(Vector3.back * minute * 90f), tickSeconds).SetEase(Ease.Linear);
        
            await DOVirtual.DelayedCall(tickSeconds, () => { });
            _timeText.text = InGameTime.ConvertTicksToHoursAndMinutes(timeInTicks);
        }

        public void SetTime(int timeInTicks)
        {
            int hours = Mathf.FloorToInt(timeInTicks);
            int minute = Mathf.RoundToInt(timeInTicks % 4)*90;

            _timeText.text = InGameTime.ConvertTicksToHoursAndMinutes(timeInTicks);
            _arrowHour.rotation = Quaternion.Euler(Vector3.back * hours);
            _arrowMinuts.rotation = Quaternion.Euler(Vector3.back * minute);
        }
    
    }
}