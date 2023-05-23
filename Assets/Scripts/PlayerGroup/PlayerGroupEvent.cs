using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlayerGroup
{
    public class PlayerGroupEvent : IPlayerGroupState
    {
        public void EnterState()
        {
            
        }
        public void ExitState()
        {
            
        }
        public async UniTask OnGameTick()
        {
            Debug.LogError("Tick on EventState");
            await UniTask.Yield();

            //throw new System.NotImplementedException();
        }
        public void OnStopPressed()
        {
            
        }
    }
}
