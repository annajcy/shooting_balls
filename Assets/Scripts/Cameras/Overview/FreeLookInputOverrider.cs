using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Framework.Input;

namespace Cameras.Overview
{
    public class FreeLookInputOverrider : MonoBehaviour
    {
        private CinemachineFreeLook freeLookCam;

        public float rotationSensitivity = 0.1f;
        public float mouseScrollDistanceSensitivity = 0.0001f;

        public void OnEnable()
        {
            freeLookCam = GetComponent<CinemachineFreeLook>();

            InputManager.Instance.Overview.Rotate.performed += RotateInputDetection;
            InputManager.Instance.Overview.Rotate.canceled += ClearRotateInputDetection;
            InputManager.Instance.Overview.Distance.performed += DistanceInputDetection;
        }

        private void OnDisable()
        {
            InputManager.Instance.Overview.Rotate.performed -= RotateInputDetection;
            InputManager.Instance.Overview.Rotate.canceled -= ClearRotateInputDetection;
            InputManager.Instance.Overview.Distance.performed -= DistanceInputDetection;
        }

        private void OnDestroy()
        {
            InputManager.Instance.Overview.Rotate.performed -= RotateInputDetection;
            InputManager.Instance.Overview.Rotate.canceled -= ClearRotateInputDetection;
            InputManager.Instance.Overview.Distance.performed -= DistanceInputDetection;
        }

        private void DistanceInputDetection(InputAction.CallbackContext context)
        {
            var delta = context.ReadValue<Vector2>().y;
            if (delta > 0) ZoomIn(delta, mouseScrollDistanceSensitivity);
            else ZoomOut(-delta, mouseScrollDistanceSensitivity);
        }

        private void ZoomIn(float delta, float distanceSensitivity)
        {
            for (int i = 0; i < 3; i++)
                freeLookCam.m_Orbits[i].m_Radius -= delta * distanceSensitivity;
        }

        private void ZoomOut(float delta, float distanceSensitivity)
        {
            for (int i = 0; i < 3; i++)
                freeLookCam.m_Orbits[i].m_Radius += delta * distanceSensitivity;
        }

        private void RotateInputDetection(InputAction.CallbackContext context)
        {
            freeLookCam.m_XAxis.m_InputAxisValue = context.ReadValue<Vector2>().x * rotationSensitivity;
            freeLookCam.m_YAxis.m_InputAxisValue = context.ReadValue<Vector2>().y * rotationSensitivity;
        }

        private void ClearRotateInputDetection(InputAction.CallbackContext context)
        {
            freeLookCam.m_XAxis.m_InputAxisValue = 0f;
            freeLookCam.m_YAxis.m_InputAxisValue = 0f;
        }
    }
}
