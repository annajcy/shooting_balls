using Canvases.Arrival;
using Framework.Input;
using Framework.StateMachine;
using Framework.UI;

namespace Gameplay.State
{
    public class ArrivalState : GameplayBaseState
    {
        public ArrivalState(GameplayManager gameplayManager) : base(gameplayManager) { }

        public override void OnUpdate() { }

        public override void OnEnter()
        {
            gameplayManager.arrivalManager.gameObject.SetActive(true);
        }

        public override void OnQuit()
        {
            gameplayManager.arrivalManager.gameObject.SetActive(false);
        }


    }
}