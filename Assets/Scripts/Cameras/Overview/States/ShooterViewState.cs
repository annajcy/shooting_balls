using Framework.StateMachine;

namespace Cameras.Overview.States
{
    public class ShooterViewState : OverviewBaseState
    {
        public ShooterViewState(OverviewManager overviewManager) : base(overviewManager) { }

        public override void OnEnter() { overviewManager.shooterViewCamera.SetActive(true); }

        public override void OnQuit() { overviewManager.shooterViewCamera.SetActive(false); }

        public override void OnUpdate() { }

    }
}