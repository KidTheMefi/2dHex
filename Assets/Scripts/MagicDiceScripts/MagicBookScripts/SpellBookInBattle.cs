using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using DiceCubePrototype;
using MagicSkills;
using UnityEngine;

namespace MagicBookScripts
{
    public class SpellBookInBattle : SpellBook
    {
        [SerializeField]
        private Sprite _canCastFromBookSprite;
        [SerializeField]
        private Sprite _possibleSpellsSprite;
        [SerializeField]
        private ThrowerScript _throwerScript;
        [SerializeField]
        private SpellExecutor _spellExecutor;

        private bool _canCast;



        private void Awake()
        {
            Initialize(true);
            CreateBookmarkButton(_possibleSpellsSprite, ShowAllPossibleSpellsFromCombination);
            CreateBookmarkButton(_canCastFromBookSprite, ShowAllSpellsThatCanBeCast);
            _canCast = false;
            _throwerScript.ThrowerStateChanges += OnThrowerStateChanges;
        }

        private void OnThrowerStateChanges(ThrowerState state)
        {
            switch (state)
            {
                case ThrowerState.WaitingForRoll:
                    _canCast = false;
                    foreach (var diceCube in _throwerScript.DiceCubes)
                    {
                        if (diceCube.DiceState != DiceState.Used && diceCube.RuneOnTop != null && _manaViews.TryGetValue(diceCube.RuneOnTop.RuneName, out var manaView))
                        {
                            manaView.AddValue(0.5f);
                        }
                    }
                    break;
                case ThrowerState.DiceStopped:
                    _canCast = true;
                    break;
            }
        }

        private void ShowAllPossibleSpellsFromCombination()
        {
            UpdateCurrentRunes();

            List<IMagicSpell> possibleSpells = new List<IMagicSpell>();
            var someList = SomeStatic.Combinations(_currentDiceRunesCombination).ToList();
            foreach (var combo in someList)
            {
                var possibleSpell = MagicSpellStatic.GetSpellNameWithCombination(combo);
                if (possibleSpell != null && !possibleSpells.Contains(possibleSpell))
                {
                    possibleSpells.Add(possibleSpell);
                }
            }
            possibleSpells.Sort(SomeStatic.CompareSpellsByRunesCount);
            _spells = possibleSpells;
            _page = 0;
            ShowPage();
        }

        private void ShowAllSpellsThatCanBeCast()
        {
            _spells = new List<IMagicSpell>();
            var spells = MagicSpellStatic.MagicSpellsSortByElements;
            foreach (var spell in spells)
            {
                if (CanCastFromBook(spell.GetCombination()))
                {
                    _spells.Add(spell);
                }
            }
            _page = 0;
            ShowPage();
        }

        private void UpdateCurrentRunes()
        {
            _currentDiceRunesCombination.Clear();
            foreach (var dice in _throwerScript.DiceCubes)
            {
                if (dice.DiceState == DiceState.CanBeSelected && dice.RuneOnTop != null)
                {
                    _currentDiceRunesCombination.Add(dice.RuneOnTop.RuneName);
                }
            }
        }

        private bool CanCastFromBook(string[] combination)
        {
            if (!_canCast)
            {
                return false;
            }

            Dictionary<string, int> manaCost = CombinationToManaCost(combination);

            bool canCast = true;
            foreach (var manaType in manaCost.Keys)
            {
                canCast = (canCast && manaCost[manaType] <= _manaViews[manaType].EssenceValue);
            }
            return canCast;
        }

        private async UniTask CastFromBookAsync(IMagicSpell magicSpell)
        {
            ShowBook();
            var castSuccess = await _spellExecutor.ExecuteSpellFromBookAsync(magicSpell.GetType().Name);
            if (castSuccess)
            {
                Dictionary<string, int> manaCost = CombinationToManaCost(magicSpell.GetCombination());
                foreach (var manaType in manaCost.Keys)
                {
                    _manaViews[manaType].TrySpendMana(manaCost[manaType]);
                }
            }
        }


        private Dictionary<string, int> CombinationToManaCost(string[] combination)
        {
            Dictionary<string, int> manaCost = new Dictionary<string, int>();

            foreach (var rune in combination)
            {
                if (!manaCost.ContainsKey(rune))
                {
                    manaCost.Add(rune, 1);
                }
                else
                {
                    manaCost[rune] += 1;
                }
            }
            return manaCost;
        }

        protected override void ShowPage()
        {
            CheckPage();
            for (int i = 0; i < _spellInBook.Length; i++)
            {
                if (_spells.Count > i + _page * _spellInBook.Length)
                {
                    var spell = _spells[i + _page * _spellInBook.Length];
                    var combination = spell.GetCombination();
                    _spellInBook[i].SetSpell(spell, CanCastFromBook(combination), () => CastFromBookAsync(spell).Forget());
                }
                else
                {
                    _spellInBook[i].gameObject.SetActive(false);
                }
            }
        }

        private void OnDestroy()
        {
            _throwerScript.ThrowerStateChanges -= OnThrowerStateChanges;
        }
    }
}