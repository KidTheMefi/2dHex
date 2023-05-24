using System;
using System.Threading;
using BattleFieldScripts;
using Cysharp.Threading.Tasks;
using MagicSkills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public enum SpellState
    {
        SpellSelected,
        SpellPlaying,
        SpellPlayed,
        SpellCanceled
    }

    public class SpellExecutor : MonoBehaviour
    {
        public event Action<SpellState> SpellStateChange = delegate(SpellState state) { };
        [SerializeField]
        private TMP_Dropdown _spellDropdown;
        [SerializeField]
        protected Button _executeButton;
        [SerializeField]
        protected Button _cancelButton;

        [SerializeField]
        private TextMeshProUGUI _spellNameText;

        private CancellationTokenSource _cancellationTokenSource;
        private IMagicSpell _currentSelectedSpell;

        private void Awake()
        {
            _executeButton.gameObject.SetActive(false);
            _cancelButton.gameObject.SetActive(false);
            _executeButton.onClick.AddListener(() => ExecuteSpellAsync().Forget());
            _cancelButton.onClick.AddListener(CancelSpell);
            /*
            _spellDropdown.ClearOptions();
            _spellDropdown.AddOptions(MagicSpellStatic.GetCharacterFaceEffectsNames());*/
        }

        private void DisableButtons()
        {
            _executeButton.gameObject.SetActive(false);
            _cancelButton.gameObject.SetActive(false);
        }

        public void SetSpell(IMagicSpell iMagicSpell)
        {
            if (iMagicSpell == null)
            {
                DisableButtons();
            }
            else
            {
                _executeButton.gameObject.SetActive(true);
                _cancelButton.gameObject.SetActive(false);
            }
            _currentSelectedSpell = iMagicSpell;
            UpdateSpellText();
        }

        private void UpdateSpellText()
        {
            string text = null;
            if (_currentSelectedSpell != null)
            {
                text = $"{SomeStatic.AddSpacesToSentence(_currentSelectedSpell?.GetType().Name)}";
            }
            _spellNameText.text = text;
        }

        protected void CancelSpell()
        {
            _cancellationTokenSource.Cancel();
            _executeButton.gameObject.SetActive(true);
            _cancelButton.gameObject.SetActive(false);
        }
        private async UniTask<bool> ExecuteSpellAsync()
        {
            _executeButton.gameObject.SetActive(false);
            _cancelButton.gameObject.SetActive(true);
            _cancellationTokenSource = new CancellationTokenSource();
            
            if (_currentSelectedSpell == null)
            {
                return false;
            }
            SpellStateChange.Invoke(SpellState.SpellPlaying);
            var spellHandler = CreateMagicSpellHandler(_currentSelectedSpell);
            var spellPlayed = await spellHandler.ExecuteSpellAsync(_cancellationTokenSource.Token);
            _currentSelectedSpell = spellPlayed ? null : _currentSelectedSpell;
            DisableButtons();
            SpellStateChange.Invoke(spellPlayed ? SpellState.SpellPlayed : SpellState.SpellCanceled);
            
            return spellPlayed;
        }

        protected virtual MagicSpellHandler CreateMagicSpellHandler(IMagicSpell magicSpell)
        {
            return new MagicSpellHandler(_currentSelectedSpell);
        }

        public async UniTask<bool> ExecuteSpellFromBookAsync(string spell)
        {
            SpellStateChange.Invoke(SpellState.SpellCanceled);
            _currentSelectedSpell = MagicSpellStatic.GetSpellWithName(spell);
            UpdateSpellText();
            return await ExecuteSpellAsync();
        }
    }
}