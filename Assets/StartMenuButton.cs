using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;


enum StartButtonAction
{
    NewGame, LoadGame
}
public class StartMenuButton : MonoBehaviour
{
    [SerializeField]
    private Button _button;

    [SerializeField]
    private StartButtonAction _startButtonAction;
    
    void Start()
    {
        switch (_startButtonAction)
        {
            case StartButtonAction.NewGame:
                _button.onClick.AddListener(SceneChanger.GetInstance().NewGameBegin);
                break;
            case StartButtonAction.LoadGame:
                _button.onClick.AddListener(SceneChanger.GetInstance().LoadGame);
                break;
        }
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
