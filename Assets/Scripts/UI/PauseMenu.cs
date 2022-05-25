using UnityEngine.InputSystem;
using Zenject;

namespace UI
{
    public class PauseMenu : IInitializable
    {
        private TestInputActions _testInputActions;
        private GameTime.GameTime _gameTime;

        private bool _isPaused = false;
        
        public PauseMenu(TestInputActions testInputActions, GameTime.GameTime gameTime)
        {
            _testInputActions = testInputActions;
            _gameTime = gameTime;
        }

        public void Initialize()
        {
            _testInputActions.CameraInput.Pause.performed += Pause;
        }

        private void Pause(InputAction.CallbackContext callback)
        {
            _isPaused = !_isPaused;
            _gameTime.Pause(_isPaused);
        }
    }
}
