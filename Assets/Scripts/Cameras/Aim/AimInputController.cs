using System;
using Framework.Event;
using Framework.Input;
using UnityEngine;
using UnityEngine.Serialization;
using EventType = Framework.Event.EventType;

namespace Cameras.Aim
{
    public enum AimInputType
    {
        SlingShot,
        Touch
    }

    public class AimInputController : MonoBehaviour
    {
        public AimInputType currentAimInputType = AimInputType.SlingShot;

        private void OnEnable()
        {
            EventCenter.Instance.AddEventListener(EventType.ToggleAimInput, OnToggleAimInput);
            EventCenter.Instance.AddEventListener<AimInputType>(EventType.SetAimInput, OnSetAimInput);
        }

        private void OnDisable()
        {
            EventCenter.Instance.RemoveEventListener(EventType.ToggleAimInput, OnToggleAimInput);
            EventCenter.Instance.RemoveEventListener<AimInputType>(EventType.SetAimInput, OnSetAimInput);
        }

        private void OnDestroy()
        {
            EventCenter.Instance.RemoveEventListener(EventType.ToggleAimInput, OnToggleAimInput);
            EventCenter.Instance.RemoveEventListener<AimInputType>(EventType.SetAimInput, OnSetAimInput);
        }

        public void DisableAimInput()
        {
            InputManager.Instance.Aim.Disable();
            InputManager.Instance.AimTouch.Disable();
        }

        private void ChangeInputType(AimInputType aimInputType)
        {
            Debug.Log("Changed To: " + aimInputType);
            if (aimInputType == AimInputType.Touch)
            {
                currentAimInputType = AimInputType.Touch;
                InputManager.Instance.Aim.Disable();
                InputManager.Instance.AimTouch.Enable();
                EventCenter.Instance.EventTrigger<AimInputType>(EventType.AimInputChanged, currentAimInputType);
            }
            else if (aimInputType == AimInputType.SlingShot)
            {
                currentAimInputType = AimInputType.SlingShot;
                InputManager.Instance.AimTouch.Disable();
                InputManager.Instance.Aim.Enable();
                EventCenter.Instance.EventTrigger<AimInputType>(EventType.AimInputChanged, currentAimInputType);
            }
        }

        private void OnSetAimInput(AimInputType type) { ChangeInputType(type); }

        private void OnToggleAimInput()
        {
            if (currentAimInputType == AimInputType.SlingShot) ChangeInputType(AimInputType.Touch);
            else if (currentAimInputType == AimInputType.Touch) ChangeInputType(AimInputType.SlingShot);
        }
    }
}