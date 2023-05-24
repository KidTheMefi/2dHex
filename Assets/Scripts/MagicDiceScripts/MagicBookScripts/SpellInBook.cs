using System;
using System.Collections.Generic;
using MagicSkills;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MagicBookScripts
{
    public class SpellInBook : MonoBehaviour
    {
        [SerializeField]
        private List<Image> _runesImages;
        [SerializeField]
        private TextMeshProUGUI _nameText;
        [SerializeField]
        private TextMeshProUGUI _descriptionText;
        [SerializeField]
        private Button _button;
        private UnityAction _clickAction;

        private string _spellName;

        private void Awake()
        {
            _button.onClick.AddListener(OnButtonClick);
        }
        public void SetSpell(IMagicSpell spell, bool canCast = false, UnityAction unityAction = null)
        {
            _clickAction = unityAction;
            _button.interactable = canCast;
            gameObject.SetActive(true);
            CleanRunes();
            CleanText();
            ShowRunes(spell.GetCombination());
            _spellName = spell.GetType().Name;
            _nameText.text = SomeStatic.AddSpacesToSentence(_spellName);
            _descriptionText.text = $" {SomeStatic.AddSpacesToSentence(spell.GetField().ToString())} \n" +
                $"{SomeStatic.AddSpacesToSentence(spell.GetPossibleTarget().ToString())} \n" +
                $"{SomeStatic.AddSpacesToSentence(spell.GetTargetingForm().GetType().Name)}";
        }

        private void OnButtonClick()
        {
            _clickAction?.Invoke();
            //SpellInBookSignals.GetInstance().InvokePlaySpellFromBook(_spellName);
        }
        
        private void CleanText()
        {
            _descriptionText.text = null;
            _nameText.text = null;
        }
        private void CleanRunes()
        {
            foreach (var runesImage in _runesImages)
            {
                runesImage.sprite = null;
                runesImage.gameObject.SetActive(false);
            }   
        }

        private void ShowRunes(string[] runes)
        {
            for (int i = 0; i < runes.Length; i++)
            {
                var image = _runesImages[i];
                image.sprite = Runes.GetRuneScriptable(runes[i]).RuneOnDiceSprite;
                image.gameObject.SetActive(true);
            }
        }
    }
}
