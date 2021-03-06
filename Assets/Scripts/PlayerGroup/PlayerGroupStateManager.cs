using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
        private IPlayerGroupState _currentStateHandler;
        private PlayerState _currentState = PlayerState.Event;
        private GameTime.GameTime _gameTime;
        List<IPlayerGroupState> _states;
        
        [Inject]
        public void Construct(
            PlayerGroupIdle idle,
            PlayerGroupMovement movement,
            PlayerGroupRest rest,
            PlayerGroupEvent groupEvent,
            GameTime.GameTime gameTime)
        {
            _states = new List<IPlayerGroupState>
            {
                // This needs to follow the enum order
                idle, movement, groupEvent, rest
            };
            _gameTime = gameTime;
        }

        public PlayerState CurrentState => _currentState;

        public void Initialize()
        {
            Debug.Log("Initialize");
            _gameTime.Tick += OnGameTick;
            Assert.IsNull(_currentStateHandler);
            ChangeState(PlayerState.Idle);
            
        }
        
        private void OnGameTick()
        {
            _currentStateHandler.OnGameTick();
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

            _currentStateHandler = _states[(int)state];
            _currentStateHandler.EnterState();
        }
        
    }
}
