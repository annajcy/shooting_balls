using Framework.StateMachine;
using UnityEngine;

namespace Cameras.Overview.States
{
    public class AllViewState : OverviewBaseState
    {
        public AllViewState(OverviewManager overviewManager) : base(overviewManager) { }

        public override void OnEnter() { overviewManager.allViewCamera.SetActive(true); }

        public override void OnQuit() { overviewManager.allViewCamera.SetActive(false); }

        public override void OnUpdate() { }

    }
}