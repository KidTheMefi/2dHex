using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TestButtonUI : MonoBehaviour
{
    [SerializeField]
    private Button _button;
    [SerializeField]
    private TextMeshProUGUI _buttonText;

    [Inject]
    public void Construct(Action action, string buttonText = "button")
    {
        _button.onClick.AddListener(action.Invoke);
        _buttonText.text = buttonText;
    }

    public void SetInteractable(bool value)
    {
        _button.interactable = value;
    }

    private void OnDestroy()
    {
        _button.onClick?.RemoveAllListeners();
    }

    public class Factory : PlaceholderFactory<Action, string, TestButtonUI>
    {
       
    }
}