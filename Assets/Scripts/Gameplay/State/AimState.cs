using Cameras.Aim;
using Canvases.Aim;
using Framework.Event;
using Framework.Input;
using Framework.StateMachine;
using Framework.UI;
using UnityEngine.Windows;

namespace Gameplay.State
{
    public class AimState : GameplayBaseState
    {
        public AimState(GameplayManager gameplayManager) : base(gameplayManager) { }

        public override void OnUpdate() { }

        public override void OnEnter()
        {
            gameplayManager.aimManager.gameObject.SetActive(true);

            EventCenter.Instance.EventTrigger<AimInputType>(EventType.SetAimInput, gameplayManager.aimManager.aimInputController.currentAimInputType);
            EventCenter.Instance.EventTrigger<ShotIndicatorMode>(EventType.ChangeShotState, ShotIndicatorMode.StandBy);
            EventCenter.Instance.EventTrigger(EventType.AimRotationRestore);
            CanvasManager.Instance.ShowCanvas<AimCanvas>();
        }

        public override void OnQuit()
        {
            gameplayManager.aimManager.aimInputController.DisableAimInput();
            gameplayManager.aimManager.gameObject.SetActive(false);

            CanvasManager.Instance.HideCanvas<AimCanvas>();
        }


    }
}