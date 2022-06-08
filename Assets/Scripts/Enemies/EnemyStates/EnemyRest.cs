using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Enemies.EnemyStates
{
    public class EnemyRest : IEnemyState
    {
        public event Action<EnemyState> ChangeState = delegate(EnemyState state) { };
        private EnemyModel _enemyModel;
        private EnemyView _enemyView;

        public EnemyRest(EnemyView enemyView, EnemyModel enemyModel)
        {
            _enemyView = enemyView;
            _enemyModel = enemyModel;
        }

        public void EnterState()
        {
            Debug.Log("Enter rest state");
        }
        public void ExitState()
        {
            Debug.Log("Exit rest state");
        }

        public async UniTask OnGameTick()
        {
            await UniTask.Yield();
            _enemyModel.ChangeEnergy(2);
            if (_enemyModel.Energy >= _enemyModel.EnemyProperties.MaxEnergy)
            {
                ChangeState.Invoke(EnemyState.Moving);
            }
        }
    }
}