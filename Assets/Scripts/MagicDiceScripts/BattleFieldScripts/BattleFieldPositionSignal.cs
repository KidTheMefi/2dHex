using System;
using CharactersScripts;

namespace BattleFieldScripts
{
    public class BattleFieldPositionSignal
    {
        public event Action<Character> MouseEnterCharacter = delegate(Character character) { };
        public event Action MouseExitCharacter = delegate { };
        public event Action<Character> SelectedCharacter = delegate(Character character)
        {
            
        };
        public event Action<BattleFieldPosition> MouseOnPosition = delegate(BattleFieldPosition position) { };
        public event Action<BattleFieldPosition> SelectPosition = delegate(BattleFieldPosition position) { };
        public event Action MouseExitPosition = delegate { };
        
        private static BattleFieldPositionSignal _instance;

        public static BattleFieldPositionSignal GetInstance()
        {
            return _instance ??= new BattleFieldPositionSignal();
        }
        
        public void InvokeMouseEnterCharacter(Character character)
        {
            MouseEnterCharacter.Invoke(character);
        }
        
        public void InvokeMouseExitCharacter()
        {
            MouseExitCharacter.Invoke();
        }
        public void InvokeMouseExitPosition()
        {
            MouseExitPosition.Invoke();
        }
        public void InvokeMouseOnPosition(BattleFieldPosition battleFieldPosition)
        {
            MouseOnPosition.Invoke(battleFieldPosition);
        }
        
        public void InvokeSelectPosition(BattleFieldPosition battleFieldPosition)
        {
            SelectPosition.Invoke(battleFieldPosition);
        }
    }
}