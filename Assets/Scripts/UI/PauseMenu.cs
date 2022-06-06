using UnityEngine.InputSystem;
using Zenject;

namespace UI
{
    public class PauseMenu : IInitializable
    {
        private TestInputActions _testInputActions;
        private GameTime.InGameTime _inGameTime;

        private bool _isPaused = false;
        
        public PauseMenu(TestInputActions testInputActions, GameTime.InGameTime inGameTime)
        {
            _testInputActions = testInputActions;
            _inGameTime = inGameTime;
        }

        public void Initialize()
        {
            _testInputActions.CameraInput.Pause.performed += Pause;
        }

        private void Pause(InputAction.CallbackContext callback)
        {
            _isPaused = !_isPaused;
            _inGameTime.Pause(_isPaused);
        }
    }
}
