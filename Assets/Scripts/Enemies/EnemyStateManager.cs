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

    public class EnemyStateManager
    {
        private IEnemyState _currentStateHandler;
        private EnemyState _currentState = EnemyState.Rest;
        private IEnemyVisualisation _enemyVisualisation;
        public EnemyState CurrentState => _currentState;
        
        private ITickHandler _inGameTime;
        
        List<IEnemyState> _states;
        
        [Inject]
        public void Construct(
            EnemyRest enemyRest,
            EnemyMovement enemyMovement,
            EnemyChasing chasing,
            ITickHandler inGameTime,
            IEnemyVisualisation enemyVisualisation)
        {
            _states = new List<IEnemyState>
            {
                enemyRest, enemyMovement, chasing
            };
           
            _inGameTime = inGameTime;
            _enemyVisualisation = enemyVisualisation;
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
            _currentStateHandler.OnGameTick();
            
        }
        
        private void ChangeState(EnemyState state)
        {
            if (_currentState == state)
            {
                Debug.Log("Enemy_currentState == state");
                return;
            }

            _currentState = state;

            if (_currentStateHandler != null)
            {
                _currentStateHandler.ChangeState -= ChangeState;
                _currentStateHandler.ExitState();
                _currentStateHandler = null;
            }
            _enemyVisualisation.ChangeEnemyVisualisation(state);
            _currentStateHandler = _states[(int)state];
            _currentStateHandler.ChangeState += ChangeState;
            _currentStateHandler.EnterState();
            
        }
    }
}