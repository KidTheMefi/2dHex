using System;
using System.Collections.Generic;
using CharactersScripts;
using DefaultNamespace;
using ScriptableScripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BattleFieldScripts
{
    public class TeamField : MonoBehaviour, IBattleFieldEvents
    {
        public event Action<TeamField> TeamDeadEvent = delegate(TeamField field) { };
        public event Action<BattleFieldPosition> MouseOnPosition = delegate(BattleFieldPosition position) { };
        public event Action Confirm =  delegate { };
        public event Action MouseExitPosition = delegate { };

        [SerializeField]
        private BattleFieldPosition PositionPlacePrefab;
        [SerializeField]
        private ButtonNonUI _acceptButton;
        [SerializeField]
        private ButtonNonUI _cancelButton;
        [SerializeField]
        private SpriteRenderer _highlightRenderer;
        
        private Vector2Int[][] _positionsVector;
        public Vector2Int[][] PositionsVector => _positionsVector;
        private Dictionary<Vector2Int, BattleFieldPosition> _positionDictionary;
        public Dictionary<Vector2Int, BattleFieldPosition> PositionDictionary => _positionDictionary;
        private List<Character> _aliveCharacters;

        private List<Vector2Int> _possibleTarget = new List<Vector2Int>();
        private Action<List<Vector2>> Targeting;
        private bool _isSubscribedToMouse;

        public bool PlayerField;
        [SerializeField, Range(1, 9)]
        private int _randomCharacter;

        [SerializeField]
        private Vector2 _distanceBetweenPositions; 


        private void Awake()
        {
            //_distanceBetweenPositions = new Vector2(1.5f, 2.5f);
            _aliveCharacters = new List<Character>();
            _positionDictionary = new Dictionary<Vector2Int, BattleFieldPosition>();
            _positionsVector = new Vector2Int[3][];

            for (int i = 0; i < _positionsVector.Length; i++)
            {
                _positionsVector[i] = new Vector2Int[3];
                for (int j = 0; j < _positionsVector[i].Length; j++)
                {
                    _positionsVector[i][j] = new Vector2Int(i, j);
                    var pos = Instantiate(PositionPlacePrefab, transform);
                    pos.transform.localPosition = new Vector3(i*_distanceBetweenPositions.x, -j*_distanceBetweenPositions.y, 0);
                    pos.name = $"{i} | {j}";
                    _positionDictionary.Add(_positionsVector[i][j], pos);
                }
            }
        }

        public void FillWithCharacters(Dictionary<Vector2Int, CharacterScriptable> teamScriptable)
        {
            if (teamScriptable == null)
            {
                return;
            }
            foreach (var posWithCharacter in teamScriptable)
            {
                Character character = CharacterFactoryWithPool.Instance.CreateCharacter(posWithCharacter.Value);
                AddCharacterToPosition(character, posWithCharacter.Key);
            }
            UpdateCharactersAttack();
        }


        public void UpdateCharactersAttack()
        {
            foreach (var position in _positionDictionary.Values)
            {
                if (!position.IsEmpty())
                {
                    position.CharacterOnPosition.CharacterAttackHandler.SelectAttack();
                }
            }
        }

        public void HighLightWithColor(Color color)
        {
            _highlightRenderer.color = color;
        }

        private void SubscribeToMouseOnPosition(bool value)
        {
            if (value == _isSubscribedToMouse)
            {
                return;   
            }
        
            if (value)
            {
                BattleFieldPositionSignal.GetInstance().MouseExitPosition += UpdateSelect;
                BattleFieldPositionSignal.GetInstance().MouseOnPosition += OnTargetPosition;
                BattleFieldPositionSignal.GetInstance().SelectPosition += OnTargetSelected;
            }
            else
            {
                BattleFieldPositionSignal.GetInstance().MouseExitPosition -= UpdateSelect;
                BattleFieldPositionSignal.GetInstance().MouseOnPosition -= OnTargetPosition; 
                BattleFieldPositionSignal.GetInstance().SelectPosition -= OnTargetSelected;
                
            }
            _isSubscribedToMouse = value;
        }
        
        private void UpdateSelect()
        {
            SetButtonsActive(false);
            MouseExitPosition.Invoke();
            ResetPositionSelectState();
            foreach (var pos in _positionDictionary)
            {
                if (_possibleTarget.Contains(pos.Key))
                {
                    pos.Value.ChangeState(PositionState.CanBeTarget);
                }
            }
        }
        
        private void OnTargetPosition(BattleFieldPosition position)
        {
            position.ChangeState(PositionState.Target);
            MouseOnPosition.Invoke(position);
        }

        private void OnTargetSelected(BattleFieldPosition position)
        {
            SetButtonsActive(true);
            _acceptButton.SetupButton(position.transform.position + new Vector3(0.5f,-1.1f), () =>
            {
                Confirm.Invoke();
                SetButtonsActive(false);
                ResetPositionSelectState();
            });
            _cancelButton.SetupButton(position.transform.position + new Vector3(-0.5f,-1.1f), () =>
            {
                SetButtonsActive(false);
                UpdateSelect();
                SubscribeToMouseOnPosition(true);
            });
            
            ResetPositionSelectState();
            SubscribeToMouseOnPosition(false);
        }

        private void SetButtonsActive(bool value)
        {
            _acceptButton.SetActive(value);
            _cancelButton.SetActive(value);
        }
        

        private void AddCharacterToPosition(Character character, Vector2Int position)
        {
            if (_positionDictionary.TryGetValue(position, out var positionIn) && positionIn.IsEmpty())
            {
                positionIn.AddCharacter(character);
                character.SetPositionOnBattleField(position, this);

                if (!_aliveCharacters.Contains(character))
                {
                    
                    _aliveCharacters.Add(character);
                    character.CharacterRemoveEvent += OnCharacterRemoveEvent;
                }
                UpdateCharactersAttack();
            }
        }
        
        private void OnCharacterRemoveEvent(Character character)
        {
            character.CharacterRemoveEvent -= OnCharacterRemoveEvent;
            _aliveCharacters.Remove(character);
            if (_aliveCharacters.Count == 0)
            {
                TeamDeadEvent.Invoke(this);
            }
        }

        public void AddCharacterToPosition(Character character, BattleFieldPosition position)
        {
            AddCharacterToPosition(character, GetBattlePositionVector(position));
        }

        private List<Vector2Int> EmptyPositions()
        {
            List<Vector2Int> emptyPositions = new List<Vector2Int>();

            foreach (var characterOnPosition in _positionDictionary)
            {
                if (characterOnPosition.Value.IsEmpty())
                {
                    emptyPositions.Add(characterOnPosition.Key);
                }
            }
            return emptyPositions;
        }

        private Vector2Int GetEmptyPosition()
        {
            
            var emptyPositions = EmptyPositions();
            return emptyPositions[Random.Range(0, emptyPositions.Count)];
        }

        private void ResetPositionSelectState()
        {
            foreach (var position in _positionDictionary)
            {
                position.Value.ChangeState(PositionState.Default);
                position.Value.Effected(false);
            }
        }
        
        public void SetPossibleTargets(List<Vector2Int> targets)
        {
            SubscribeToMouseOnPosition(true);
            _possibleTarget = targets;
            UpdateSelect();
        }

        public Vector2Int GetBattlePositionVector(BattleFieldPosition battleFieldPosition)
        {
            foreach (var position in _positionDictionary)
            {
                if (position.Value != battleFieldPosition) continue;
                return position.Key;
            }

            return new Vector2Int(-100, -100);
        }
        
        public bool TryGetPositionFromArray(Vector2Int arrayPosition, out BattleFieldPosition battleFieldPosition)
        {
            battleFieldPosition = null;
            return _positionDictionary.TryGetValue(arrayPosition, out battleFieldPosition);
        }
        
        public Dictionary<Vector2Int, CharacterScriptable> GetTeamScriptable()
        {
            Dictionary<Vector2Int, CharacterScriptable> characterPositions = new Dictionary<Vector2Int, CharacterScriptable>();
            
            foreach (var position in PositionDictionary)
            {
                if (!position.Value.IsEmpty() && !position.Value.CharacterOnPosition.CharacterScriptable.SummonedCreature)
                {
                    characterPositions.Add(position.Key, position.Value.CharacterOnPosition.CharacterScriptable);
                }
            }

            return characterPositions;
        }

        private void OnDestroy()
        {
            BattleFieldPositionSignal.GetInstance().MouseExitPosition -= UpdateSelect;
            BattleFieldPositionSignal.GetInstance().MouseOnPosition -= OnTargetPosition; 
            BattleFieldPositionSignal.GetInstance().SelectPosition -= OnTargetSelected;
        }
    }
}