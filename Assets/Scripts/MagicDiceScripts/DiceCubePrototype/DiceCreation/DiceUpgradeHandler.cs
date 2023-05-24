using System.Collections.Generic;
using ScriptableScripts;
using ScriptableScripts.CubeAndRuneScriptable;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DiceCubePrototype
{
    public class DiceUpgradeHandler : MonoBehaviour
    {
        [SerializeField]
        private EssenceView essenceViewPrefab;
        [SerializeField]
        private RunesInCreationButton _runeButtonPrefab;
        [SerializeField]
        private Transform _runesParent;
        [SerializeField]
        private Transform _essenceCostParent;
        [SerializeField]
        private CreatedCube _createdCube;
        [SerializeField]
        private Button _saveDiceButton;
        [SerializeField]
        private Button _resetDiceButton;
        [SerializeField]
        private TextMeshProUGUI _diceDescriptionTMPRO;

        private List<DiceFaceInCreation> _cubeDiceFaces = new List<DiceFaceInCreation>();
        private Dictionary<string, EssenceView> _essenceViews = new Dictionary<string, EssenceView>();
        private Dictionary<string, int> _spentEssence = new Dictionary<string, int>();
        private Dictionary<string, RunesInCreationButton> _runeButtons = new Dictionary<string, RunesInCreationButton>();
        private Vector3 layPosition;
        private DiceFaceInCreation _currentDiceFace;
        
        private EssenceView _essenceCostView;
        
        private void Awake()
        {
            _resetDiceButton.onClick.AddListener(ResetDice);
            _saveDiceButton.onClick.AddListener(SaveDice);
            foreach (var rune in Runes._runeScriptableList)
            {
                CreateRuneButton(rune);
            }
            //CreateRuneButton(Runes.EmptyRune);
            
            _essenceCostView = Instantiate(essenceViewPrefab, _essenceCostParent);
            _essenceCostView.transform.localPosition = Vector3.zero;
        }
        
        public void UpgradeDice(DiceForUpgrade diceForUpgrade)
        {
            _cubeDiceFaces = diceForUpgrade.CubeDiceFaces;
            foreach (var diceFace in _cubeDiceFaces)
            {
                diceFace.FaceSelected += DiceFaceOnFaceSelected;
                diceFace.FaceUnclicked += DiceFaceOnFaceUnClicked;
            }
            _saveDiceButton.interactable = true;
            _resetDiceButton.interactable = true;
            BeginCreation(diceForUpgrade.ScriptableCubeSetting);
        }
        
        public void StopUpgrade()
        {
            
            _saveDiceButton.interactable = false;
            _resetDiceButton.interactable = false;
            foreach (var diceFace in _cubeDiceFaces)
            {
                diceFace.FaceSelected -= DiceFaceOnFaceSelected;
                diceFace.FaceUnclicked -= DiceFaceOnFaceUnClicked;
            }
            ResetDice();
            _diceDescriptionTMPRO.text = null;
        }
        
        private void BeginCreation(CreatedCube createdCube)
        {
            _createdCube = createdCube;
            UpdateFacesFromScriptable();
            DisableAllRune();
            UpdateDescription();
        }
        private void UpdateFacesFromScriptable()
        {
            var runes = _createdCube.GetCubeRuneScriptable();
            for (int i = 0; i < runes.Length; i++)
            {
                if (i < _cubeDiceFaces.Count)
                {
                    _cubeDiceFaces[i].Selected(false);
                    _cubeDiceFaces[i].SetRune(runes[i]);
                }
            }
        }

        private void CreateRuneButton(RuneScriptable rune)
        {
            var bookmarkButton = CreateButton(rune, () =>
            {
                SpendSelectedEssence(rune.RuneName);
                ConfirmFaceRune(true);
                DisableAllRune();
            });

            bookmarkButton.SetupRuneButton(value =>
            {
                if (value)
                {
                    ShowRuneCost(bookmarkButton.transform, rune);
                    SetTempFaceRune(rune);
                }
                else
                {
                    ConfirmFaceRune(false);
                    HideRuneCost();
                }
            });

            bookmarkButton.name = rune.RuneName + " Rune";

            if (rune.RuneName == "Empty")
            {
                return;
            }
            var manaView = Instantiate(essenceViewPrefab, bookmarkButton.transform);
            manaView.Setup(rune.RuneName);
            manaView.SetValue(Random.Range(1,9));
            _essenceViews.Add(rune.RuneName, manaView);
            _spentEssence.Add(rune.RuneName, 0);
        }


        private RunesInCreationButton CreateButton(RuneScriptable rune, UnityAction action)
        {
            var runeButton = Instantiate(_runeButtonPrefab, _runesParent);
            runeButton.gameObject.SetActive(true);
            runeButton.SetupBookmark(() =>
            {
                //DisableAllRune();
                action.Invoke();
            }, rune.RuneOnDiceSprite);
            _runeButtons.Add(rune.RuneName, runeButton);
            return runeButton;
        }

        private void UpdateRuneInteractable()
        {
            if (_currentDiceFace == null)
            {
                DisableAllRune();
                return;
            }
            
            foreach (var runeName in _runeButtons.Keys)
            {
                if(_essenceViews.TryGetValue(runeName, out var essence) && _runeButtons.TryGetValue(runeName, out var runeButton))
                {
                    runeButton.Select(essence.EssenceValue < CountRuneCost(runeName));
                }
            }
            
            if (_currentDiceFace.RuneScriptable != null && _runeButtons.TryGetValue(_currentDiceFace.RuneScriptable.RuneName, out var runeSelected))
            {
                runeSelected.Select(true);
            }
        }

        private void DisableAllRune()
        {
            foreach (var button in _runeButtons.Values)
            {
                button.Select(true);
            }
        }

        private void ShowRuneCost(Transform runeTransform, RuneScriptable rune)
        {
            if (rune == null || rune.RuneName == "Empty")
            {
                HideRuneCost();
                return;
            }
            _essenceCostView.transform.SetParent(runeTransform);
            _essenceCostView.transform.localPosition = Vector3.up*30;
            _essenceCostView.gameObject.SetActive(true);
            _essenceCostView.Setup(rune.RuneName);
            _essenceCostView.SetValue(CountRuneCost(rune.RuneName));
        }

        private void HideRuneCost()
        {
            _essenceCostView.gameObject.SetActive(false);
            _essenceCostView.transform.SetParent(_essenceCostParent);
            _essenceCostView.transform.localPosition = Vector3.up;
            _essenceCostView.SetValue(0);
        }

        private int CountRuneCost(string runeName)
        {
            int cost = 1;
            foreach (var face in _cubeDiceFaces)
            {
                if (face.RuneScriptable != null && face.RuneScriptable.RuneName == runeName)
                {
                    cost += 2;
                }
            }
            return cost;
        }

        private void DiceFaceOnFaceUnClicked()
        {
            DiceFaceOnFaceSelected(null);
            DisableAllRune();
        }

        private void DiceFaceOnFaceSelected(DiceFaceInCreation diceFace)
        {
            foreach (var face in _cubeDiceFaces)
            {
                if (face != diceFace)
                {
                    face.Selected(false);
                    face.ConfirmRune(false);
                }
            }
            _currentDiceFace = diceFace;
            UpdateRuneInteractable();
        }

        private void SetTempFaceRune(RuneScriptable rune)
        {
            if (_currentDiceFace != null)
            {
                _currentDiceFace.SetTempRune(rune);
            }
        }

        private void ConfirmFaceRune(bool value)
        {
            if (_currentDiceFace != null)
            {
                _currentDiceFace.ConfirmRune(value);
            }
            if (value)
            {
                DiceFaceOnFaceSelected(null);
            }
            HideRuneCost();
            UpdateDescription();
        }

        private void SaveDice()
        {
            List<RuneScriptable> runes = new List<RuneScriptable>();
            foreach (var diceFace in _cubeDiceFaces)
            {
                if (diceFace.RuneScriptable == null || diceFace.RuneScriptable.RuneName == "Empty")
                {
                    runes.Add(null);
                }
                else
                {
                    runes.Add(diceFace.RuneScriptable);
                }
            }
            _createdCube.SaveRunes(runes);
            CleanSpentEssence();
        }

        private void ResetDice()
        {
            ConfirmFaceRune(false);
            UpdateFacesFromScriptable();
            ReturnSpentEssence();
            DisableAllRune();
        }

        private void SpendSelectedEssence(string essenceName)
        {
            var value = CountRuneCost(essenceName);
            if(_essenceViews.TryGetValue(essenceName, out var essence))
            {
                essence.AddValue(-value);
            }
            if(_spentEssence.ContainsKey(essenceName))
            {
                _spentEssence[essenceName] += value;
            }
        }

        private void CleanSpentEssence()
        {
            foreach (var essenceName in _essenceViews.Keys)
            {
                if (_spentEssence.ContainsKey(essenceName))
                {
                    _spentEssence[essenceName] = 0;
                }
            }
        }

        private void ReturnSpentEssence()
        {
            foreach (var essenceName in _essenceViews.Keys)
            {
                if (_spentEssence.ContainsKey(essenceName))
                {
                    var essenceValue = _spentEssence[essenceName];
                    _essenceViews[essenceName].AddValue(essenceValue);
                    _spentEssence[essenceName] = 0;
                }
            }
        }

        private void UpdateDescription()
        {
            string description = null;
            
            foreach (var face in _cubeDiceFaces)
            {
                description += face.RuneScriptable != null ? face.RuneScriptable.RuneName + "\n": null;
            }
            _diceDescriptionTMPRO.text = description;
        }
    }
}