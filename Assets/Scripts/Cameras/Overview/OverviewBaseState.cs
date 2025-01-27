using Framework.StateMachine;
using UnityEngine;

namespace Cameras.Overview
{
    public abstract class OverviewBaseState : IBaseState
    {
        protected OverviewManager overviewManager;
        protected OverviewBaseState(OverviewManager overviewManager)
        { this.overviewManager = overviewManager; }

        public abstract void OnUpdate();
        public abstract void OnEnter();
        public abstract void OnQuit();
    }
}