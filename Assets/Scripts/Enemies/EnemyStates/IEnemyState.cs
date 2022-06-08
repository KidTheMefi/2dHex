using System;
using Cysharp.Threading.Tasks;

namespace Enemies
{
    public interface IEnemyState
    {
        public event Action<EnemyState> ChangeState;
        void EnterState();
        void ExitState(); 
        UniTask OnGameTick();

    }
}
