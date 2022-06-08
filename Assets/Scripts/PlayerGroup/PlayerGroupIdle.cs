using Cysharp.Threading.Tasks;
using Interfaces;
using UI;
using UnityEngine;

namespace PlayerGroup
{
    public class PlayerGroupIdle : IPlayerGroupState
    {
        private Vector2Int _pathTarget;
        private readonly Vector2Int _unClickable = new Vector2Int(999,999);

        private PlayerPathFind _playerPathFind;
        private IHexMouseEvents _hexMouseEvents;
        private PlayerGroupStateManager _playerGroupStateManager;
        private PlayerUIEnergy _playerUIEnergy;

        public PlayerGroupIdle(
            PlayerPathFind playerPathFind,
            IHexMouseEvents hexMouseEvents,
            PlayerGroupStateManager playerGroupStateManager, 
            PlayerUIEnergy playerUIEnergy)
        {
            _playerPathFind = playerPathFind;
            _hexMouseEvents = hexMouseEvents;
            _playerGroupStateManager = playerGroupStateManager;
            _playerUIEnergy = playerUIEnergy;
        }

        private async UniTask PathFind(Vector2Int target)
        {
            await _playerPathFind.PathFindTest(target);
        }
    
        private void OnDoubleClick()
        {
            if (_playerPathFind.GetPath().Length!=0)
            {
                _playerGroupStateManager.ChangeState(PlayerState.Moving);
            }
        }
    
        public UniTask OnGameTick()
        {
            Debug.LogWarning("GameTick on Idle state!");
            throw new System.NotImplementedException();
        }
        public void OnStopPressed()
        {
            if (_playerPathFind.GetPath().Length!=0)
            {
                OnDoubleClick();
            }
            else
            {
                _playerGroupStateManager.ChangeState(PlayerState.Rest);
            }
        }

        private void MouseEventsHexClicked(Vector2Int hex)
        {
            if (hex != _pathTarget)
            {
                PathFind(hex).Forget();
                _pathTarget = hex;
            }
            else 
            {
                OnDoubleClick();
                _pathTarget = _unClickable;
            }
        }
    
        public void EnterState()
        {
            //Debug.Log("Entered Idle state");
            _playerUIEnergy.SetRestSliderInteractable(true);
            _hexMouseEvents.HighlightedHexClicked += MouseEventsHexClicked;
        }
        public void ExitState()
        {
            _playerUIEnergy.SetRestSliderInteractable(false);
            _hexMouseEvents.HighlightedHexClicked -= MouseEventsHexClicked;
        }
    }
}
