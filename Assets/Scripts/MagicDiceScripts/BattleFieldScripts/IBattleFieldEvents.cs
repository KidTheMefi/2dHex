using System;

namespace BattleFieldScripts
{
    public interface IBattleFieldEvents
    {
        public event Action<BattleFieldPosition> MouseOnPosition ;
        public event Action Confirm ;
        public event Action MouseExitPosition;
    }
}