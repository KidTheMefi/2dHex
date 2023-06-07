using System;
using GameEvents.MapObjectDescriptionSignal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DefaultNamespace
{
    public class MapObjectDescriptionPlate : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _nameText;
        [SerializeField]
        private TextMeshProUGUI _descriptionText;
        [SerializeField]
        private Image _image;

        private IDescriptionSignal _signal;

        [Inject]
        private void Construct(IDescriptionSignal signal)
        {
            _signal = signal;
            
            _signal.ShowDescription += SignalOnShowDescription;
            _signal.HideDescription+= SignalOnHideDescription;
        }
        private void SignalOnShowDescription(MapObjectDescriptionSignal.DescriptionStruct description)
        {
            CleanDescription();
            _image.sprite = description.spritePicture;
            _nameText.text = description.name;
            _descriptionText.text = description.description;
            gameObject.SetActive(true);
        }
        private void SignalOnHideDescription()
        {
            gameObject.SetActive(false);
            CleanDescription();
        }

        private void CleanDescription()
        {
            _image.sprite = null;
            _nameText.text = null;
            _descriptionText.text = null;
        }

        private void OnDestroy()
        {
            _signal.ShowDescription -= SignalOnShowDescription;
            _signal.HideDescription -= SignalOnHideDescription;
        }

    }
}