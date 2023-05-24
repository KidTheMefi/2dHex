using System;
using MagicBookScripts;

namespace DiceCubePrototype
{
    public class RunesInCreationButton : BookmarkButton
    {
        private event Action<bool> MouseOnRuneButton;

        public void SetupRuneButton(Action<bool> onMouseEnter)
        {
            MouseOnRuneButton = onMouseEnter;
        }
        private void OnMouseEnter()
        {
           MouseOnRuneButton?.Invoke(true);
        }

        private void OnMouseExit()
        {
           MouseOnRuneButton?.Invoke(false);
        }
    }
}