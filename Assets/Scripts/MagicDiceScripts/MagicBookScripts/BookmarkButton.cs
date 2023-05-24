using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MagicBookScripts
{
    public class BookmarkButton : MonoBehaviour
    {
        [SerializeField]
        private Image _backgroundImage;
        [SerializeField]
        private Image _runeImage;
        [SerializeField]
        private Button _button;

        [SerializeField]
        private Color _selectedColor;
        [SerializeField]
        private Color _defaultColor;

        public bool Selected => !_button.interactable;
        
        public void SetupBookmark( UnityAction onClickAction, Sprite sprite = null)
        {
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(onClickAction);
            _backgroundImage.color = _defaultColor;
            _runeImage.sprite = sprite != null ? sprite : _runeImage.sprite;
        }

        public void InvokeAction()
        {
            _button.onClick.Invoke();
        }

        public void Select(bool value)
        {
            _button.interactable = !value;
            _backgroundImage.color = value ? _selectedColor : _defaultColor;
        }
    }
}
