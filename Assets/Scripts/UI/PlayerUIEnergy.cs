using System;
using PlayerGroup;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class PlayerUIEnergy : MonoBehaviour
    {
        private PlayerGroupModel _playerGroupModel;
        public event Action<int, bool> StartRest;

        [SerializeField] private TextMeshProUGUI _energyText;
        [SerializeField] private Slider _energySlider;
        [SerializeField] private Button _restEnableButton;
        [SerializeField] private GameObject _rest;
        [SerializeField] private Slider _restSlider;
        [SerializeField] private Toggle _restToggle;
        [SerializeField] private Button _restStartButton;
        [SerializeField] private TextMeshProUGUI _restText;

        private bool _restUI = false;
        private bool _sleep = false;
        
        [Inject]
        public void Construct(PlayerGroupModel playerGroupModel)
        {
            _playerGroupModel = playerGroupModel;
        }

        public void Start()
        {
            _restUI = _rest.activeSelf;
            _playerGroupModel.EnergyChanged += PlayerGroupModelOnEnergyChanged;
            _restEnableButton.onClick.AddListener(OnRestButtonClick);
            _restToggle.onValueChanged.AddListener(OnRestToggleClick);
            _restSlider.onValueChanged.AddListener(OnRestSliderValueChange);
            EnergyInit();
        }

        public void SetActionAtButton(Action<int, bool> action)
        {
            StartRest = action;
            _restStartButton.onClick.AddListener(OnStartButtonClick);
        }

        public void SetRestSliderInteractable(bool value)
        {
            if (value)
            {
                _restSlider.onValueChanged.AddListener(OnRestSliderValueChange);
            }
            else
            {
                _restSlider.onValueChanged.RemoveListener(OnRestSliderValueChange);
            }
            _restStartButton.interactable = value;
            _restSlider.interactable = value;
            _restToggle.interactable = value;
            
        }

        public void AddRestSliderValue(int value)
        {
            _restSlider.value += value;
            _restText.text = _restSlider.value.ToString();
        }

        private void OnStartButtonClick()
        {
            if (_playerGroupModel.State == PlayerState.Idle)
            {
                StartRest?.Invoke(Mathf.RoundToInt(_restSlider.value), _sleep);
            }
        }

        private void OnRestButtonClick()
        {
            _restUI = !_restUI;
            _rest.SetActive(_restUI);
        }

        private void OnRestSliderValueChange(float value)
        {
            if (_sleep)
            {
                _restSlider.value = _restSlider.value < _playerGroupModel.MinTimeSleed ? _playerGroupModel.MinTimeSleed : _restSlider.value;
            }
            _restText.text = _restSlider.value.ToString();
        }
        
        private void OnRestToggleClick(bool sleep)
        {
            _sleep = sleep;
            if (_sleep)
            {
                _restSlider.value = _restSlider.value < _playerGroupModel.MinTimeSleed ? _playerGroupModel.MinTimeSleed : _restSlider.value;
            }
        }

        private void EnergyInit()
        {
            _energySlider.maxValue = _playerGroupModel.MaxEnergy;
            _energySlider.value = _playerGroupModel.Energy;
            _energyText.text = _playerGroupModel.Energy.ToString();
        }
        private void PlayerGroupModelOnEnergyChanged(int energy)
        {
            _energySlider.value = _playerGroupModel.Energy;
            _energyText.text = _playerGroupModel.Energy.ToString();
        }
    }
}
