using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DiceCubePrototype;
using ScriptableScripts.CubeAndRuneScriptable;
using UnityEngine;
using UnityEngine.UI;


public enum ThrowerState
{
    WaitingForRoll, DiceInMotion, DiceStopped
}
public class ThrowerScript : MonoBehaviour
{
    public event Action<ThrowerState> ThrowerStateChanges = delegate(ThrowerState state) { };
    [SerializeField]
    private List<DiceCube> _diceCubes;
    [SerializeField]
    private Button _rollButton;
    public List<DiceCube> DiceCubes => _diceCubes;
    [SerializeField]
    private DiceCube _diceCubePrefab;
    [SerializeField]
    private DiceListSetupScriptable _diceSetupList;
    
    
    public void SetRollButtonInteractable(bool value)
    {
        _rollButton.interactable = value;
    }
    
    private void Start()
    {
       // _diceSetupList.
        var setupList = _diceSetupList.GetCubesSetup();

        for (int i = 0; i < setupList.Count; i++)
        {
            var dice = Instantiate(_diceCubePrefab, transform);
            dice.Setup(setupList[i], new Vector3(-2.55f + i, 0,0));
            _diceCubes.Add(dice);
        }
        _rollButton.onClick.AddListener(ThrowAll);
    }

    public void ResetDicesState()
    {
        ThrowerStateChanges.Invoke(ThrowerState.WaitingForRoll);
        foreach (var dice in _diceCubes)
        {
            if (dice.isActiveAndEnabled)
            {
                //dice.transform.rotation = new Quaternion(0, 0, 0, 0);
                dice.SetCubeState(DiceState.Default);
                //dice.BackToStartPosition();
            }
        }
    }
    private void ThrowAll()
    {
        ThrowAllAsync().Forget();
    }
    private async UniTask ThrowAllAsync()
    {
        _rollButton.interactable = false;
        ThrowerStateChanges.Invoke(ThrowerState.DiceInMotion);
        UniTask[] tasks = new UniTask[_diceCubes.Count];
        
        foreach (var dice in _diceCubes)
        {
            if (dice.isActiveAndEnabled)
            {
                dice.BackToStartPosition();
            }
        }
        
        for (int i = 0; i < _diceCubes.Count; i++)
        {
            if (_diceCubes[i].isActiveAndEnabled)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
                tasks[i] = _diceCubes[i].RollDice();
            }
        }
        
        await UniTask.WhenAll(tasks);
        
        /*tasks = new UniTask[_diceCubes.Count];
        for (int i = 0; i < _diceCubes.Count; i++)
        {
            if (_diceCubes[i].isActiveAndEnabled)
            {
                tasks[i] = _diceCubes[i].RotateForBetterLook();
            }
        }
        await UniTask.WhenAll(tasks);*/
        //_rollButton.interactable = true;
        ThrowerStateChanges.Invoke(ThrowerState.DiceStopped);
    }
}
