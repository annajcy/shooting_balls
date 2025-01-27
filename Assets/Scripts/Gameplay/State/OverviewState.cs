using Cameras.Overview.States;
using Canvases.OverView;
using Framework.Input;
using Framework.StateMachine;
using Framework.UI;

namespace Gameplay.State
{
    public class OverviewState : GameplayBaseState
    {
        //private int stateNum = 0;
        public OverviewState(GameplayManager gameplayManager) : base(gameplayManager) { }

        public override void OnUpdate() { }

        public override void OnEnter()
        {
            gameplayManager.overviewManager.gameObject.SetActive(true);
            gameplayManager.overviewManager.stateMachine.ChangeState<AllViewState>();

            InputManager.Instance.Overview.Enable();
            CanvasManager.Instance.ShowCanvas<OverviewCanvas>();
        }

        public override void OnQuit()
        {
            gameplayManager.overviewManager.stateMachine.ChangeEmptyState();
            gameplayManager.overviewManager.gameObject.SetActive(false);

            InputManager.Instance.Overview.Disable();
            CanvasManager.Instance.HideCanvas<OverviewCanvas>();
        }


    }
}