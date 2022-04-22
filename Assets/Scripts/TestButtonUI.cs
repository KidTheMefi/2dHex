using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TestButtonUI : MonoBehaviour
{
    [SerializeField]
    private Button button;

    public void Init(Action action)
    {
        button.onClick.AddListener(action.Invoke);
    }

    private void OnDestroy()
    {
        //button.onClick?.RemoveAllListeners();
    }

    public class Factory : PlaceholderFactory<TestButtonUI>
    {
        
    }
}