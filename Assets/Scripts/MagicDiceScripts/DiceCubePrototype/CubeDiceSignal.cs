using System;


namespace DiceCubePrototype
{
    public class CubeDiceSignal
    {
        public event Action<DiceCube> MouseEnterDice = delegate(DiceCube dice) { };
        public event Action<DiceCube> MouseDownOnDice = delegate(DiceCube dice) { };
        public event Action MouseExitDice = delegate { };

        private static CubeDiceSignal _instance;

        public static CubeDiceSignal GetInstance()
        {
            return _instance ??= new CubeDiceSignal();
        }

        public void InvokeMouseEnterDice(DiceCube cubeDice)
        {
            MouseEnterDice.Invoke(cubeDice);
        }

        public void InvokeMouseExit()
        {
            MouseExitDice.Invoke();
        }

        public void InvokeMouseDown(DiceCube cubeDice)
        {
            MouseDownOnDice.Invoke(cubeDice);
        }
    }
}