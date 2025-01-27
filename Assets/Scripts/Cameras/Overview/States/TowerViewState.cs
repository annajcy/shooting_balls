using Framework.StateMachine;

namespace Cameras.Overview.States
{
    public class TowerViewState : OverviewBaseState
    {
        public TowerViewState(OverviewManager overviewManager) : base(overviewManager) { }

        public override void OnEnter() { overviewManager.towerViewCamera.SetActive(true); }

        public override void OnQuit() { overviewManager.towerViewCamera.SetActive(false); }

        public override void OnUpdate() { }


    }
}