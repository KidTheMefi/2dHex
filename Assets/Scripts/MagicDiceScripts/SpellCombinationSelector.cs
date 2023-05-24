using System;
using System.Collections.Generic;
using DefaultNamespace;
using DiceCubePrototype;
using MagicSkills;
using UnityEngine;
using UnityEngine.UI;

public class SpellCombinationSelector : MonoBehaviour
{
    public event Action<IMagicSpell> SpellSelected = delegate(IMagicSpell spell) { };
    private bool _isSubscribedToDiceSelect;
    private Dictionary<DiceCube, Image> _selectedRunesOnPlate = new Dictionary<DiceCube, Image>();
    private ObjectPool<Image> _runesPool;
            
    [SerializeField]
    private Image _iconPrefab;
    [SerializeField]
    private Transform _panel;
        

    private void Start()
    {
        _runesPool = new ObjectPool<Image>(CreateRunesIcon, TurnIconOn, TurnIconOff, 10);
    }
        

    private Image CreateRunesIcon()
    {
        var icon = Instantiate(_iconPrefab, transform);
        icon.gameObject.SetActive(false);
        icon.transform.SetParent(_panel);
        return icon;
    }

    private void TurnIconOff(Image icon)
    {
        icon.gameObject.SetActive(false);
    }
    private void TurnIconOn(Image icon)
    {
        icon.gameObject.SetActive(true);
    }

    public void CleanCombination(bool used)
    {
        Queue<DiceCube> diceOnCombination = new Queue<DiceCube>();
        foreach (var cube in _selectedRunesOnPlate.Keys)
        {
            diceOnCombination.Enqueue(cube);
        }

        while (diceOnCombination.Count != 0)
        {
            var cube = diceOnCombination.Dequeue();
            RemoveFromCombination(cube);
            cube.SetCubeState(used? DiceState.Used : DiceState.CanBeSelected);
        }
        CheckCombination();
    }

    public void SubscribeToDiceClick(bool value)
    {
        if (value == _isSubscribedToDiceSelect)
        {
            return;
        }

        if (value)
        {
            CubeDiceSignal.GetInstance().MouseDownOnDice += OnMouseDownOnDice;
        }
        else
        {
            CubeDiceSignal.GetInstance().MouseDownOnDice -= OnMouseDownOnDice;
        }

        _isSubscribedToDiceSelect = value;
    }

    private void OnDestroy()
    {
        CubeDiceSignal.GetInstance().MouseDownOnDice -= OnMouseDownOnDice;
    }

    private void OnMouseDownOnDice(DiceCube diceCube)
    {
        if (diceCube.RuneOnTop == null)
        {
            return;
        }

        if (_selectedRunesOnPlate.ContainsKey(diceCube))
        {
            RemoveFromCombination(diceCube);
        }
        else
        {
            AddToCombination(diceCube);
        }
        CheckCombination();
    }


    private void AddToCombination(DiceCube diceCube)
    {
        var icon = _runesPool.GetObject();
        icon.sprite = diceCube.RuneOnTop.RuneOnDiceSprite;
        _selectedRunesOnPlate.Add(diceCube, icon);
        diceCube.Selected(true);
        //UpdateIconPosition();
    }

    private void CheckCombination()
    {
        List<string> selectedCombination = new List<string>();

        foreach (var dice in _selectedRunesOnPlate.Keys)
        {
            selectedCombination.Add(dice.RuneOnTop.RuneName);
        }

        var spell = MagicSpellStatic.GetSpellWithCombination(selectedCombination.ToArray());
        SpellSelected.Invoke(spell);
    }

    private void RemoveFromCombination(DiceCube diceCube)
    {
        _runesPool.ReturnObject(_selectedRunesOnPlate[diceCube]);
        _selectedRunesOnPlate.Remove(diceCube);
        diceCube.Selected(false);
        //UpdateIconPosition();
    }

    private void UpdateIconPosition()
    {
        float i = 0;
        var iconsCount = _selectedRunesOnPlate.Values.Count;
        var redundant = iconsCount / 2;
        i = iconsCount % 2 == 0 ? -redundant + 0.5f : -redundant;
        foreach (var icon in _selectedRunesOnPlate.Values)
        {
            icon.transform.position = _panel.transform.position + Vector3.right * i;
            i++;
        }
    }
}