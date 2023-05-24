using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BattleFieldScripts;
using CharactersScripts;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MovementScript
{
    public enum MoveState
    {
        Default,
        MoveDisable,
        BeginMove,
        SuccessMove,
        CanceledMove
    }
    
    public class ChangePosition : MonoBehaviour
    {
        public event Action<MoveState> MoveStateChange = delegate(MoveState state) { }; 
        private delegate void Subscribe(bool value);
        [SerializeField]
        private TeamField _teamField;
        [SerializeField]
        private Button _moveButton;
        [SerializeField]
        private Button _moveCancelButton;
        [SerializeField]
        private TextMeshProUGUI _moveCountText;
        private int _moveCount = 1;
        [SerializeField]
        private int _moveCountDefault;

        private MoveState _moveState = MoveState.Default;
        
        private bool _isSubscribed;
        private CancellationTokenSource _ctsSelect;
        private CancellationTokenSource _ctsMove;

        private Character _movingCharacter;
        private BattleFieldPosition _startPosition;
        private BattleFieldPosition _finishPosition;
        
        public void EnableMove(bool updateMoveCount = false)
        {
            if (updateMoveCount)
            {
                _moveCount = _moveCountDefault;
                _moveState = MoveState.Default;
                UpdateCountText();
            }

            if (_moveCount == 0)
            {
                return;
            }
            _moveButton.interactable = true;
            _moveCancelButton.interactable = false;
        }

      
        
        private void Awake()
        {
            _isSubscribed = false;
            _moveButton.onClick.AddListener(() =>
            {
                BeginMove().Forget();
                _moveButton.interactable = false;
                _moveCancelButton.interactable = true;
            });
            
            _moveCancelButton.onClick.AddListener(() =>
            {
                _ctsMove.Cancel();
                _moveButton.interactable = true;
                _moveCancelButton.interactable = false;
            });
            _moveButton.interactable = false;
            _moveCancelButton.interactable = false;
        }
        private async UniTask BeginMove()
        {
            _moveState = MoveState.BeginMove;
            MoveStateChange.Invoke(MoveState.BeginMove);
            _ctsMove = new CancellationTokenSource();
            var moved  = await ExecuteSpellAsync(_ctsMove.Token);

            if (_moveState == MoveState.MoveDisable)
            {
                return;
            }
            _moveState = moved ? MoveState.SuccessMove : MoveState.CanceledMove;
            MoveStateChange.Invoke(_moveState);

            if (moved)
            {
                MoveSuccess();
            }
        }
        
        private async UniTask<bool> ExecuteSpellAsync(CancellationToken cancelMove)
        {
            await UniTask.Yield();
            _teamField.SetPossibleTargets(new TargetingPossibility(_teamField).GetPossibleTargets(PossibleTarget.AnyPositionWithCharacter));

            var moveSuccess = await MoveWaitAsync(SubscribeForCharacterSelect, cancelMove);
            if (!moveSuccess)
            {
                return false;
            }
            _teamField.SetPossibleTargets(PossiblePositionToMove(_teamField.GetBattlePositionVector(_startPosition)));

            moveSuccess = await MoveWaitAsync(SubscribeForNewPosition, cancelMove);
            if (!moveSuccess)
            {
                return false;
            }
            _moveCancelButton.interactable = false;
            return true;
        }

        private async UniTask<bool> MoveWaitAsync(Subscribe subscribeMethod, CancellationToken cancelMove)
        {
            subscribeMethod(true);
            _ctsSelect = new CancellationTokenSource();
            await UniTask.WaitUntil(() => _ctsSelect.IsCancellationRequested || cancelMove.IsCancellationRequested);
            subscribeMethod(false);
            if (cancelMove.IsCancellationRequested)
            {
                MoveCanceled();
                return false;
            }
            return true;
        }

        private void SubscribeForCharacterSelect(bool value)
        {
            if (_isSubscribed == value)
            {
                return;
            }

            if (value)
            {
                _teamField.Confirm += CancelToken;
                _teamField.MouseExitPosition += RemoveCharacter;
                _teamField.MouseOnPosition += SelectMoveCharacter;
            }
            else
            {
                _teamField.Confirm -= CancelToken;
                _teamField.MouseExitPosition -= RemoveCharacter;
                _teamField.MouseOnPosition -= SelectMoveCharacter;
            }
            _isSubscribed = value;
        }

        private void SubscribeForNewPosition(bool value)
        {
            if (_isSubscribed == value)
            {
                return;
            }

            if (value)
            {
                _teamField.Confirm += CancelToken;
                _teamField.MouseExitPosition += BackToStartPosition;
                _teamField.MouseOnPosition += MoveToPosition;
            }
            else
            {
                _teamField.Confirm -= CancelToken;
                _teamField.MouseExitPosition -= BackToStartPosition;
                _teamField.MouseOnPosition -= MoveToPosition;
            }
            _isSubscribed = value;
        }
        private void BackToStartPosition()
        {
            if (_startPosition.CharacterOnPosition == _movingCharacter)
            {
                return;
            }
            
            _finishPosition.RemoveFromPosition(_movingCharacter);
            if (!_startPosition.IsEmpty())
            {
                var switchCharacter = _startPosition.CharacterOnPosition;
                _startPosition.RemoveFromPosition(switchCharacter);
                _teamField.AddCharacterToPosition(switchCharacter, _finishPosition);
            }
            _teamField.AddCharacterToPosition(_movingCharacter, _startPosition);
        }

        private void MoveToPosition(BattleFieldPosition newPosition)
        {
            _startPosition.RemoveFromPosition(_movingCharacter);
            _finishPosition = newPosition;

            if (!newPosition.IsEmpty())
            {
                
                var switchCharacter = newPosition.CharacterOnPosition;
                newPosition.RemoveFromPosition(switchCharacter);
                _teamField.AddCharacterToPosition(switchCharacter, _startPosition);
            }
            _teamField.AddCharacterToPosition(_movingCharacter, _finishPosition);
        }


        private List<Vector2Int> PossiblePositionToMove(Vector2Int positionInFieldArray)
        {
            var centerPosition = positionInFieldArray;

            Vector2Int[] crossPosition =
            {
                centerPosition - Vector2Int.up,
                centerPosition + Vector2Int.up,
                centerPosition + Vector2Int.left,
                centerPosition - Vector2Int.left,
            };

            return crossPosition.ToList();
        }

        private void SelectMoveCharacter(BattleFieldPosition position)
        {
            _startPosition = position;
            _movingCharacter = _startPosition.CharacterOnPosition;
        }
        private void RemoveCharacter()
        {
            _startPosition = null;
            _movingCharacter = null;
        }
        private void CancelToken()
        {
            _ctsSelect?.Cancel();
        }

        public void DisableMove()
        {
            _moveState = MoveState.MoveDisable;
            _moveButton.interactable = false;
            _moveCancelButton.interactable = false;
            CancelToken();
        }

        private void MoveSuccess()
        {
            _moveCount--;
            UpdateCountText();
            if (_moveCount == 0)
            {
                DisableMove();
            }
        }
        private void UpdateCountText()
        {
            _moveCountText.text = _moveCount.ToString();
        }
        
        private void MoveCanceled()
        {
            Debug.Log("Moving Canceled");
                _teamField.SetPossibleTargets(new List<Vector2Int>());

                if (_startPosition != null)
                {
                    BackToStartPosition();
                }
        }

        private void OnDestroy()
        {
            _teamField.Confirm -= CancelToken;
            _teamField.MouseExitPosition -= RemoveCharacter;
            _teamField.MouseOnPosition -= SelectMoveCharacter;
            _teamField.MouseExitPosition -= BackToStartPosition;
            _teamField.MouseOnPosition -= MoveToPosition;
        }
    }
}