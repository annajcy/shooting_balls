using System.Collections.Generic;
using Framework.StateMachine;
using UnityEngine;

namespace Gameplay
{
    public abstract class GameplayBaseState : IBaseState
    {
        protected GameplayManager gameplayManager;
        protected GameplayBaseState(GameplayManager gameplayManager)
        { this.gameplayManager = gameplayManager; }

        public abstract void OnUpdate();
        public abstract void OnEnter();
        public abstract void OnQuit();
    }
}