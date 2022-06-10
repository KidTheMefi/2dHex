using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Enemies.EnemyStates
{
    public class EnemyRest : IEnemyState
    {
        public event Action<EnemyState> ChangeState = delegate(EnemyState state) { };
        private EnemyMapModel _enemyMapModel;
        private EnemyView _enemyView;

        public EnemyRest(EnemyView enemyView, EnemyMapModel enemyMapModel)
        {
            _enemyView = enemyView;
            _enemyMapModel = enemyMapModel;
        }

        public void EnterState()
        {
        }
        public void ExitState()
        {
        }

        public async UniTask OnGameTick()
        {
            await UniTask.Yield();
            _enemyMapModel.ChangeEnergy(2);
            if (_enemyMapModel.Energy >= _enemyMapModel.EnemyProperties.MaxEnergy)
            {
                ChangeState.Invoke(EnemyState.Moving);
            }
        }
    }
}