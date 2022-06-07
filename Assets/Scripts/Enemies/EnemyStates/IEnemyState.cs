using Cysharp.Threading.Tasks;

namespace Enemies
{
    public interface IEnemyState
    {
        void EnterState();
        void ExitState(); 
        UniTask OnGameTick();
        
    }
}
