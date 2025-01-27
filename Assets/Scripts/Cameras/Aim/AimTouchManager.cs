using System;
using System.Collections.Generic;
using Cameras.Overview;
using Canvases.Aim;
using Framework.Event;
using Framework.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using EventType = Framework.Event.EventType;

namespace Cameras.Aim
{
    public class AimTouchManager : MonoBehaviour
    {
        public float touchShootForce = 250.0f;
        public float positionX;
        public float positionY;

        private bool isReadyToShoot = false;

        private void OnEnable()
        {
            InputManager.Instance.AimTouch.Shoot.performed += OnAimTouchPerformed;
            InputManager.Instance.AimTouch.Shoot.canceled += OnAimTouchCanceled;
        }

        private void OnDisable()
        {
            InputManager.Instance.AimTouch.Shoot.performed -= OnAimTouchPerformed;
            InputManager.Instance.AimTouch.Shoot.canceled -= OnAimTouchCanceled;
        }

        private void OnDestroy()
        {
            InputManager.Instance.AimTouch.Shoot.performed -= OnAimTouchPerformed;
            InputManager.Instance.AimTouch.Shoot.canceled -= OnAimTouchCanceled;
        }

        private void OnAimTouchCanceled(InputAction.CallbackContext context)
        {
            Debug.Log("OnAimTouchCanceled!");
            if (isReadyToShoot)
            {
                EventCenter.Instance.EventTrigger<float>(EventType.Shoot, touchShootForce);
                EventCenter.Instance.EventTrigger<ShotIndicatorMode>(EventType.ChangeShotState,
                    ShotIndicatorMode.Ready);
            }
        }

        private void OnAimTouchPerformed(InputAction.CallbackContext context)
        {
            positionX = context.ReadValue<Vector2>().x;
            positionY = context.ReadValue<Vector2>().y;

            Vector3 position = new Vector3(positionX, positionY, 0.0f);
            if (RaycastUtility.GetCameraRaySelectPoint(position, out var lookAtPosition))
            {
                EventCenter.Instance.EventTrigger<Vector3>(EventType.AimLookAt, lookAtPosition);
                isReadyToShoot = true;
            }
            else isReadyToShoot = false;
        }
    }
}