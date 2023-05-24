using System.Collections.Generic;
using MagicSkills;
using ScriptableScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MagicBookScripts
{
    public class SpellBook : MonoBehaviour
    {
        protected SpellInBook[] _spellInBook;
        protected int _page;

        [SerializeField]
        private GameObject _bookView;
        [SerializeField]
        private Button _nextPageButton;
        [SerializeField]
        private Button _previousPageButton;
        [SerializeField]
        private BookmarkButton _bookmarkPrefab;
        [SerializeField]
        private Transform _bookMarksParent;
        [SerializeField]
        private Sprite _allElementsSprite;
        [SerializeField]
        private EssenceView essenceViewPrefab;
        
        protected List<IMagicSpell> _spells;
        protected List<BookmarkButton> _bookmarkButtons;
        protected List<string> _currentDiceRunesCombination = new List<string>();
        protected Dictionary<string, EssenceView> _manaViews = new Dictionary<string, EssenceView>();
    
        private BookmarkButton _selectedBookmarkButton;
        
        private void Awake()
        {
            Initialize(false);
        }

         protected void Initialize(bool withManaView)
        {
            _spells = MagicSpellStatic.MagicSpellsSortByElements;
            _spellInBook = _bookView.GetComponentsInChildren<SpellInBook>();
            _page = 0;
            _nextPageButton.onClick.AddListener(NextPage);
            _previousPageButton.onClick.AddListener(PreviousPage);
            _bookmarkButtons = new List<BookmarkButton>();

            var allSpellByElements = CreateBookmarkButton(_allElementsSprite, ShowAllSpellsByElements);
            foreach (var rune in Runes._runeScriptableList)
            {
                CreateRuneBookmark(rune, withManaView);
            }

            BookmarkSelected(allSpellByElements);
        }

        private void CreateRuneBookmark(RuneScriptable rune, bool withManaView = true)
        {
            var bookmarkButton = CreateBookmarkButton(rune.RuneOnDiceSprite, () =>
            {
                ShowAllSpellsWithRune(rune.RuneName);
            });
            bookmarkButton.name = rune.RuneName + "Bookmark";

            if (!withManaView)
            {
                return;
            }
            var manaView = Instantiate(essenceViewPrefab, bookmarkButton.transform);
            manaView.Setup(rune.RuneName);
            _manaViews.Add(rune.RuneName, manaView);
        }
    
        

        protected BookmarkButton CreateBookmarkButton(Sprite sprite, UnityAction action)
        {
            var bookMark = Instantiate(_bookmarkPrefab, _bookMarksParent);
            bookMark.gameObject.SetActive(true);
            bookMark.SetupBookmark(() =>
            {
                BookmarkSelected(bookMark);
                action.Invoke();
            }, sprite);
            _bookmarkButtons.Add(bookMark);
            return bookMark;
        }

        private void BookmarkSelected(BookmarkButton bookmark)
        {
            foreach (var button in _bookmarkButtons)
            {
                button.Select(button == bookmark);
            }
            _selectedBookmarkButton = bookmark;
        }

        public void ShowBook()
        {
            _bookView.SetActive(!_bookView.activeSelf);
            if (_bookView.activeSelf)
            {
                if (_selectedBookmarkButton!= null)
                {
                    _selectedBookmarkButton.InvokeAction();
                }
                ShowPage();
            }
        }


        private void ShowAllSpellsByElements()
        {
            _spells = MagicSpellStatic.MagicSpellsSortByElements;
            _page = 0;
            ShowPage();
        }

        private void ShowAllSpellsWithRune(string runeName)
        {
            _spells = MagicSpellStatic.GetAllSpellsWithRune(runeName);
            _page = 0;
            ShowPage();
        }
        
        protected virtual void ShowPage()
        {
            CheckPage();
            for (int i = 0; i < _spellInBook.Length; i++)
            {
                if (_spells.Count > i + _page * _spellInBook.Length)
                {
                    var spell = _spells[i + _page * _spellInBook.Length];
                    var combination = spell.GetCombination();
                    _spellInBook[i].SetSpell(spell);
                }
                else
                {
                    _spellInBook[i].gameObject.SetActive(false);
                }
            }
        }

        protected void CheckPage()
        {
            var nextPagePossible = _spells.Count / (float)_spellInBook.Length > _page + 1;

            _nextPageButton.interactable = nextPagePossible;

            _page = _page < 0 ? 0 : _page;
            _previousPageButton.interactable = (_page > 0);
        }

        private void NextPage()
        {
            _page++;
            ShowPage();
        }

        private void PreviousPage()
        {
            _page--;
            ShowPage();
        }
    }
}