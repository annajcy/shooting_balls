using Canvases.Arrival;
using Framework.Input;
using Framework.StateMachine;
using Framework.UI;

namespace Gameplay.State
{
    public class ArrivalTowerState : GameplayBaseState
    {
        public ArrivalTowerState(GameplayManager gameplayManager) : base(gameplayManager) { }

        public override void OnEnter()
        {
            gameplayManager.arrivalTowerManager.gameObject.SetActive(true);
            InputManager.Instance.Overview.Enable();
            CanvasManager.Instance.ShowCanvas<ArrivalCanvas>();
        }

        public override void OnQuit()
        {
            gameplayManager.arrivalTowerManager.gameObject.SetActive(false);
            InputManager.Instance.Overview.Disable();
            CanvasManager.Instance.HideCanvas<ArrivalCanvas>();
        }

        public override void OnUpdate() { }
    }
}