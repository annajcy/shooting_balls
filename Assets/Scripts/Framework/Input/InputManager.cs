using System;
using Framework.Singleton;

namespace Framework.Input
{
    public class InputManager : SingletonBase<InputManager>
    {
        private static GameInputManager gameInputManager = new GameInputManager();
        public new static GameInputManager Instance => gameInputManager;

        private InputManager()
        {
            gameInputManager.Enable();
        }

    }
}