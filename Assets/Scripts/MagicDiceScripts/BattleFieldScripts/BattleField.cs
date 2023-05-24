using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using TeamCreation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BattleFieldScripts
{
    public enum Field
    {
        PlayerField,
        EnemyField,
    }

    public class BattleField : MonoBehaviour
    {
        public event Action EndGame = delegate { };
        private static BattleField _instance;
        public static BattleField Instance => _instance;

        [SerializeField]
        private TeamField _leftTeamField;
        [SerializeField]
        private TeamField _rightTeamField;

        private TeamField _currentTeam;
        private TeamField _currentEnemyTeam;

        //public TeamField leftTeamField => _leftTeamField;
        //public TeamField rightTeamField => _rightTeamField;
        private CancellationTokenSource _characterAttacksCanceletion;

        [SerializeField]
        private SavedTeam _playerTeam;
        [SerializeField]
        private SavedTeam _computerTeam;


        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                SetCurrentTurnTeam(_leftTeamField, _rightTeamField);
            }
            else
            {
                Destroy(this);
            }
        }

        public TeamField GetField(Field field)
        {
            switch (field)
            {
                case Field.PlayerField: return _currentTeam;
                case Field.EnemyField: return _currentEnemyTeam;
            }
            throw new InvalidOperationException();
        }
        private void SetCurrentTurnTeam(TeamField currentTeam, TeamField enemyTeam)
        {
            _currentTeam = currentTeam;
            _currentEnemyTeam = enemyTeam;
            _currentTeam.TeamDeadEvent += OnTeamDeadEvent;
            _currentEnemyTeam.TeamDeadEvent += OnTeamDeadEvent;
            _currentTeam.HighLightWithColor(new Color(0.1f, 0.3f, 0.1f, 0.1f));
            _currentEnemyTeam.HighLightWithColor(new Color(0.3f, 0.1f, 0.1f, 0.1f));
        }
        private void OnTeamDeadEvent(TeamField deadTeam)
        {
            _characterAttacksCanceletion?.Cancel();
            if (deadTeam == _rightTeamField)
            {
                Debug.Log("Player win");
                ProgressionHandler.GetInstance().NextLevel();
                _playerTeam.SaveTeam(_leftTeamField.GetTeamScriptable());
                SceneManager.LoadScene(1, LoadSceneMode.Single);
            }
            else
            {
                Debug.Log("Player lose");
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                   #endif
                Application.Quit();
            }
            _currentTeam.TeamDeadEvent -= OnTeamDeadEvent;
            _currentEnemyTeam.TeamDeadEvent -= OnTeamDeadEvent;
            EndGame.Invoke();
        }


        public void FillBattleField()
        {
            _leftTeamField.FillWithCharacters(_playerTeam.CharactersInPosition);
            //_leftTeamField.FillWithRandomCharacters();
            _rightTeamField.FillWithCharacters(_computerTeam.CharactersInPosition);
            UpdateAttacks();
        }

        public void UpdateAttacks()
        {
            _leftTeamField.UpdateCharactersAttack();
            _rightTeamField.UpdateCharactersAttack();
        }

        public async UniTask<bool> EndTurnAsync()
        {
            _characterAttacksCanceletion = new CancellationTokenSource();
            TeamTurnChange();
            await TeamTurnAttackAsync(_characterAttacksCanceletion.Token);
            if (_characterAttacksCanceletion.IsCancellationRequested)
            {
                return false;
            }

            TeamTurnChange();
            await TeamTurnAttackAsync(_characterAttacksCanceletion.Token);
            return !_characterAttacksCanceletion.IsCancellationRequested;
        }
        private void TeamTurnChange()
        {
            (_currentTeam, _currentEnemyTeam) = (_currentEnemyTeam, _currentTeam);
            _currentTeam.HighLightWithColor(new Color(0.1f, 0.3f, 0.1f, 0.1f));
            _currentEnemyTeam.HighLightWithColor(new Color(0.3f, 0.1f, 0.1f, 0.1f));
        }

        private async UniTask TeamTurnAttackAsync(CancellationToken ctsToken)
        {
            foreach (var position in _currentTeam.PositionDictionary.Values)
            {
                if (!position.IsEmpty())
                {
                    if (ctsToken.IsCancellationRequested)
                    {
                        return;
                    }
                    await position.CharacterOnPosition.CharacterAttackHandler.AttackAsync();
                    if (ctsToken.IsCancellationRequested)
                    {
                        return;
                    }
                    position.CharacterOnPosition.CharacterAttackHandler.SelectAttack();
                }
            }
        }

        public TeamField GetEnemyFieldFor(TeamField field)
        {
            return field == _leftTeamField ? _rightTeamField : _leftTeamField;
        }
        private void OnDestroy()
        {
            _currentTeam.TeamDeadEvent -= OnTeamDeadEvent;
            _currentEnemyTeam.TeamDeadEvent -= OnTeamDeadEvent;
        }
    }
}