using System.Collections.Generic;
using ModestTree;
using UnityEngine;
using Zenject;


namespace PlayerGroup
{
    public enum PlayerState
    {
        Idle, Moving, Event, Rest
    }

    public class PlayerGroupStateManager : IInitializable
    {
        private bool _stopState;
        public bool StopState => _stopState;
        
        private IPlayerGroupState _currentStateHandler;
        private PlayerState _currentState = PlayerState.Event;
        private GameTime.GameTime _gameTime;
        private TestInputActions _testInputActions;
        List<IPlayerGroupState> _states;
        
        [Inject]
        public void Construct(
            PlayerGroupIdle idle,
            PlayerGroupMovement movement,
            PlayerGroupRest rest,
            PlayerGroupEvent groupEvent,
            GameTime.GameTime gameTime,
            TestInputActions testInputActions)
        {
            _states = new List<IPlayerGroupState>
            {
                // This needs to follow the enum order
                idle, movement, groupEvent, rest
            };
            _gameTime = gameTime;
            _testInputActions = testInputActions;
        }

        public PlayerState CurrentState => _currentState;

        public void Initialize()
        {
            Debug.Log("Initialize");
            _gameTime.Tick += OnGameTick;
            Assert.IsNull(_currentStateHandler);
            _testInputActions.ActionKey.Stop.performed += context => { OnStopPressed();};
            ChangeState(PlayerState.Idle);
            
        }
        
        private void OnGameTick()
        {
            _currentStateHandler.OnGameTick();
        }

        private void OnStopPressed()
        {
            _currentStateHandler.OnStopPressed();
        }
        
        public void ChangeState(PlayerState state)
        {
            if (_currentState == state)
            {
                Debug.Log("_currentState == state");
                return;
            }

            //Log.Trace("View Changing state from {0} to {1}", _currentState, state);
            
            _currentState = state;

            if (_currentStateHandler != null)
            {
                _currentStateHandler.ExitState();
                _currentStateHandler = null;
            }
            
            _stopState = false;

            _currentStateHandler = _states[(int)state];
            _currentStateHandler.EnterState();
        }
        
    }
}
