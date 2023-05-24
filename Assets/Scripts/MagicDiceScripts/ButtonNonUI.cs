using System;
using UnityEngine;


namespace DefaultNamespace
{
    public class ButtonNonUI : MonoBehaviour
    {
        
        private Action OnClick;

        private bool _interactable = true;
        
        private Color _defaultColor;
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        private Color _onMouseEnterColor;

        private void Awake()
        {
            _defaultColor = _spriteRenderer.color;
        }

        public void SetActive(bool value)
        {
            _spriteRenderer.color = _defaultColor;
                gameObject.SetActive(value);
        }
        
        public void SetupButton(Vector3 position, Action action)
        {
            transform.position = position;
            OnClick = action;
        }
        private void OnMouseDown()
        {
            if (_interactable)
            {
                OnClick?.Invoke();
                SetActive(false);
            }
        }

        private void OnMouseEnter()
        {
            _spriteRenderer.color = _onMouseEnterColor;
        }
        private void OnMouseExit()
        {
            _spriteRenderer.color = _defaultColor;
        }
    }
}