using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EssenceView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textMeshPro;
    private float _essenceValue;
    public float EssenceValue => _essenceValue;
    private string _manaType;
    

    public void Setup(string manaType)
    {
        _manaType = manaType;
    }

    private void UpdateText()
    {
        _textMeshPro.text = _essenceValue.ToString();
    }

    public void SetValue(float value)
    {
        _essenceValue = value < 0 ? 0: value;
        UpdateText();
    }
    
    public void AddValue(float value)
    {
        _essenceValue += value;
        _essenceValue = _essenceValue < 0 ? 0: _essenceValue;
        UpdateText();
    }

    public bool TrySpendMana(float value)
    {
        if (value > _essenceValue)
        {
            return false;
        }
        _essenceValue -= value;
        UpdateText();
        return true;
    }
}
