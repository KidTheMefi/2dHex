using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameTime
{
    public class NightMask : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Color _dayColor;
        [SerializeField] private Color _nightColor;

        private INightTime _nightTime;

        [Inject]
        public void Construct(INightTime nightTime)
        {
            _nightTime = nightTime;

            _nightTime.NightTimeChange += EnableNightMask;
        }
        
        
        private void EnableNightMask()
        {
            if (_nightTime.IsNightTime())
            {
                _image.DOFade(0.5f, 0.5f);
                //_image.DOColor(_nightColor, 0.5f);
            }
            else
            {
                _image.DOFade(0, 0.5f);
                //_image.DOColor(_dayColor, 0.5f);
            }
        }
    }
}
