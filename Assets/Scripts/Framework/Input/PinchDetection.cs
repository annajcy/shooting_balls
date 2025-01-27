using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Framework.Input
{
    public class PinchDetection : MonoBehaviour
    {
        private Coroutine pinchDetectionCoroutine;
        private float epsilon = 0.9f;

        public UnityEvent<float> onPinchIn;
        public UnityEvent<float> onPinchOut;

        private void Start()
        {
            InputManager.Instance.Overview.SecondaryFingerPressed.started += StartDetection;
            InputManager.Instance.Overview.SecondaryFingerPressed.canceled += EndDetection;
        }

        private void OnDestroy()
        {
            InputManager.Instance.Overview.SecondaryFingerPressed.started -= StartDetection;
            InputManager.Instance.Overview.SecondaryFingerPressed.canceled -= EndDetection;
        }

        void StartDetection(InputAction.CallbackContext context)
        {
            pinchDetectionCoroutine = StartCoroutine(PinchDetector());
        }

        void EndDetection(InputAction.CallbackContext context)
        {
            StopCoroutine(pinchDetectionCoroutine);
        }

        IEnumerator PinchDetector()
        {
            float previousDistance = 0f;
            float currentDistance = 0f;

            while (true)
            {
                Vector2 position0 = InputManager.Instance.Overview.PrimaryFingerPosition.ReadValue<Vector2>();
                Vector2 position1 = InputManager.Instance.Overview.SecondaryFingerPosition.ReadValue<Vector2>();
                Vector2 delta0 = InputManager.Instance.Overview.PrimaryFingerDelta.ReadValue<Vector2>().normalized;
                Vector2 delta1 = InputManager.Instance.Overview.SecondaryFingerDelta.ReadValue<Vector2>().normalized;

                currentDistance = Vector2.Distance(position0, position1);

                if (Vector2.Dot(delta0, delta1) < -epsilon)
                {
                    if (currentDistance > previousDistance) onPinchOut?.Invoke(currentDistance - previousDistance);
                    else if (currentDistance < previousDistance) onPinchOut?.Invoke(previousDistance - currentDistance);
                }

                previousDistance = currentDistance;
                yield return null;
            }
        }
    }
}