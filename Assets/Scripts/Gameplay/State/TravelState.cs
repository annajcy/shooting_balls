using Framework.StateMachine;
using Framework.Timer;
using Framework.UI;

namespace Gameplay.State
{
    public class TravelState : GameplayBaseState
    {
        public TravelState(GameplayManager gameplayManager) : base(gameplayManager) { }

        public override void OnUpdate() { }

        public override void OnEnter()
        {
            gameplayManager.travelManager.gameObject.SetActive(true);
        }

        public override void OnQuit()
        {
            gameplayManager.travelManager.gameObject.SetActive(false);
        }

    }
}