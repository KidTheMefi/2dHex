using Cysharp.Threading.Tasks;

namespace PlayerGroup
{
    public interface IPlayerGroupState
    {
        void EnterState();
        void ExitState(); 
        UniTask OnGameTick();
        void OnStopPressed();
    }
}
