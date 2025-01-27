using System;
using Canvases.Aim;
using Cinemachine;
using Framework.Event;
using Framework.Input;
using Framework.Timer;
using Shooter;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using EventType = Framework.Event.EventType;

namespace Cameras.Aim
{
    public class AimManager : MonoBehaviour
    {
        [HideInInspector]
        public AimInputController aimInputController;

        private CinemachineVirtualCamera aimCamera;

        public float aimCameraFOVSensitivity = 0.01f;
        public float yawSensitivity = 0.05f;

        public float pitchSensitivity = 0.01f;
        public float pitchGyroSensitivity = 0.01f;
        public float powerScale = 1f;

        public float cancelThreshold = 100.0f;
        public float powerMax = 500.0f;

        public float pitchMax = 10.0f;
        public float pitchMin = -10.0f;

        public float yawMax = 90.0f;
        public float yawMin = -90.0f;

        public bool invertYaw = false;
        public bool invertPitch = false;

        private float startY = 0.0f;
        private float currentY = 0.0f;
        private float deltaY = 0.0f;

        private float startFOV = 0.0f;

        private float pitchSum = 0.0f;
        private float yawSum = 0.0f;

        private float pitchRotationLast = 0.0f;
        private bool isPitchRotationFirstEntry = true;

        public int spawnBulletTime = 500;
        public float spawnTimeScale = 0.2f;

        private void OnEnable()
        {
            aimCamera = GetComponent<CinemachineVirtualCamera>();
            aimInputController = GetComponent<AimInputController>();

            InputManager.Instance.Aim.Yaw.performed += YawOnPerformed;

            InputManager.Instance.Aim.Pitch.performed += PitchOnPerformed;
            InputManager.Instance.Aim.PitchGyro.performed += OnPitchGyroPerformed;

            InputManager.Instance.Aim.Power.started += PowerOnStarted;
            InputManager.Instance.Aim.Power.performed += PowerOnPerformed;
            InputManager.Instance.Aim.Power.canceled += PowerOnCanceled;

            EventCenter.Instance.AddEventListener(EventType.EnterAim, OnEnterAim);
            EventCenter.Instance.AddEventListener<Transform>(EventType.OnBulletSpawn, OnBulletSpawn);
        }

        private void OnDisable()
        {
            InputManager.Instance.Aim.Yaw.performed -= YawOnPerformed;

            InputManager.Instance.Aim.Pitch.performed -= PitchOnPerformed;
            InputManager.Instance.Aim.PitchGyro.performed -= OnPitchGyroPerformed;
            InputManager.Instance.Aim.PitchGyro.started -= OnPitchGyroStarted;

            InputManager.Instance.Aim.Power.started -= PowerOnStarted;
            InputManager.Instance.Aim.Power.performed -= PowerOnPerformed;
            InputManager.Instance.Aim.Power.canceled -= PowerOnCanceled;

            EventCenter.Instance.RemoveEventListener(EventType.EnterAim, OnEnterAim);
            EventCenter.Instance.RemoveEventListener<Transform>(EventType.OnBulletSpawn, OnBulletSpawn);
        }

        private void OnDestroy()
        {
            InputManager.Instance.Aim.Yaw.performed -= YawOnPerformed;

            InputManager.Instance.Aim.Pitch.performed -= PitchOnPerformed;
            InputManager.Instance.Aim.PitchGyro.performed -= OnPitchGyroPerformed;
            InputManager.Instance.Aim.PitchGyro.started -= OnPitchGyroStarted;

            InputManager.Instance.Aim.Power.started -= PowerOnStarted;
            InputManager.Instance.Aim.Power.performed -= PowerOnPerformed;
            InputManager.Instance.Aim.Power.canceled -= PowerOnCanceled;

            EventCenter.Instance.RemoveEventListener(EventType.EnterAim, OnEnterAim);
            EventCenter.Instance.RemoveEventListener<Transform>(EventType.OnBulletSpawn, OnBulletSpawn);
        }

        public void StartSpawnBulletTime()
        {
            int bulletTimeTimer = TimerManager.Instance.CreateBulletTimeTimer(spawnTimeScale, spawnBulletTime,
                spawnBulletTime, new TimerEvents());
            TimerManager.Instance.StartTimer(bulletTimeTimer);
        }

        private void OnBulletSpawn(Transform bulletTransform)
        {
            EventCenter.Instance.EventTrigger(EventType.EnterTravel);
            EventCenter.Instance.EventTrigger(EventType.SetBulletTarget, bulletTransform);
            StartSpawnBulletTime();
        }

        private void OnPitchGyroStarted(InputAction.CallbackContext context)
        {
            isPitchRotationFirstEntry = true;
        }

        private void OnPitchGyroPerformed(InputAction.CallbackContext context)
        {
            Quaternion rotation = context.ReadValue<Quaternion>();
            Vector3 eulerRotation = rotation.eulerAngles;
            float pitchRotation = eulerRotation.x;
            float delta;
            if (!isPitchRotationFirstEntry)
                delta = pitchRotation - pitchRotationLast;
            else
            {
                delta = 0;
                isPitchRotationFirstEntry = false;
            }

            pitchRotationLast = pitchRotation;

            delta *= pitchGyroSensitivity;
            delta *= invertPitch ? -1 : 1;
            if (pitchSum + delta > pitchMin && pitchSum + delta < pitchMax)
            {
                pitchSum += delta;
                EventCenter.Instance.EventTrigger<float>(EventType.ChangePitch, delta);
            }
        }

        private void OnEnterAim() { aimCamera.m_Lens.FieldOfView = startFOV; }

        private void PowerOnPerformed(InputAction.CallbackContext context)
        {
            currentY = context.ReadValue<float>();
            deltaY = -(currentY - startY);
            EventCenter.Instance.EventTrigger<ShotIndicatorMode>(EventType.ChangeShotState,
                deltaY > cancelThreshold ? ShotIndicatorMode.Ready : ShotIndicatorMode.StandBy);
            aimCamera.m_Lens.FieldOfView = startFOV + deltaY * aimCameraFOVSensitivity;
        }

        private void PowerOnStarted(InputAction.CallbackContext context)
        {
            EventCenter.Instance.EventTrigger<ShotIndicatorMode>(EventType.ChangeShotState,
                ShotIndicatorMode.StandBy);
            startFOV = aimCamera.m_Lens.FieldOfView;
            startY = context.ReadValue<float>();
        }

        private void PowerOnCanceled(InputAction.CallbackContext context)
        {
            EventCenter.Instance.EventTrigger<ShotIndicatorMode>(EventType.ChangeShotState,
                ShotIndicatorMode.StandBy);
            if (deltaY > cancelThreshold)
            {
                float power = Mathf.Clamp(deltaY - cancelThreshold, 0.0f, powerMax);
                EventCenter.Instance.EventTrigger<float>(EventType.Shoot, power * powerScale);
            }
        }

        private void PitchOnPerformed(InputAction.CallbackContext context)
        {
            float delta = context.ReadValue<float>() * pitchSensitivity;
            delta *= invertPitch ? -1 : 1;
            if (pitchSum + delta > pitchMin && pitchSum + delta < pitchMax)
            {
                pitchSum += delta;
                EventCenter.Instance.EventTrigger<float>(EventType.ChangePitch, delta);
            }
        }

        private void YawOnPerformed(InputAction.CallbackContext context)
        {
            float delta = context.ReadValue<float>() * yawSensitivity;
            delta *= invertYaw ? -1 : 1;
            if (yawSum + delta > yawMin && yawSum + delta < yawMax)
            {
                yawSum += delta;
                EventCenter.Instance.EventTrigger<float>(EventType.ChangeYaw, delta);
            }
        }
    }
}