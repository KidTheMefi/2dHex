using System;
using System.Collections.Generic;
using Enemies.EnemyStates;
using GameTime;
using ModestTree;
using UnityEngine;
using Zenject;

namespace Enemies
{
    public enum EnemyState
    {
        Rest,
        Moving,
        Chasing
    }

    public class EnemyStateManager : IInitializable
    {
        private IEnemyState _currentStateHandler;
        private EnemyState _currentState = EnemyState.Rest;
        public EnemyState CurrentState => _currentState;
        
        private ITickHandler _inGameTime;
        
        List<IEnemyState> _states;
        
        [Inject]
        public void Construct(EnemyRest enemyRest, EnemyMovement enemyMovement, EnemyChasing chasing, ITickHandler inGameTime)
        {
            _states = new List<IEnemyState>
            {
                enemyRest, enemyMovement, chasing
            };
           
            _inGameTime = inGameTime;
        }

        public void Initialize()
        {
            _inGameTime.Tick += OnInGameTick;
            
            ChangeState(EnemyState.Moving);
        }

        public void Dispose()
        {
            ChangeState(EnemyState.Rest);
            _inGameTime.Tick -= OnInGameTick;
          
        }

        private void OnInGameTick()
        {
            Debug.Log("tick on statr");
            _currentStateHandler.OnGameTick();
            
        }
        
        private void ChangeState(EnemyState state)
        {
            if (_currentState == state)
            {
                Debug.Log("_currentState == state");
                return;
            }
            
            Log.Trace("View Changing state from {0} to {1}", _currentState, state);
            
            _currentState = state;

            if (_currentStateHandler != null)
            {
                _currentStateHandler.ChangeState -= ChangeState;
                Debug.Log("UnSub");
                _currentStateHandler.ExitState();
                _currentStateHandler = null;
            }

            _currentStateHandler = _states[(int)state];
            Debug.Log("Sub");
            _currentStateHandler.ChangeState += ChangeState;
            _currentStateHandler.EnterState();
            
        }
    }
}