using System;
using System.Collections;
using System.Collections.Generic;
using Utilities;
using Cameras.Aim;
using Framework.Event;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using EventType = Framework.Event.EventType;

namespace Shooter
{
    public class ShooterMotionController : MonoBehaviour
    {
        private Animator shooterMotionAnimator;
        private AimTransformProvider aimTransformProvider;
        public Transform shooterPitchTransform;
        public Transform shooterYawTransform;

        private static readonly int Boom = Animator.StringToHash("Boom");

        private void Awake()
        {
            shooterMotionAnimator = GetComponent<Animator>();
            aimTransformProvider = GetComponentInChildren<AimTransformProvider>();

            EventCenter.Instance.AddEventListener<float>(EventType.Shoot, OnShoot);
            EventCenter.Instance.AddEventListener<float>(EventType.ChangePitch, OnPitchChanged);
            EventCenter.Instance.AddEventListener<float>(EventType.ChangeYaw, OnYawChanged);
            EventCenter.Instance.AddEventListener<Vector3>(EventType.AimLookAt, OnAimLookAt);
            EventCenter.Instance.AddEventListener(EventType.AimRotationRestore, OnAimRotationRestore);
        }

        private void OnDestroy()
        {
            EventCenter.Instance.RemoveEventListener<float>(EventType.Shoot, OnShoot);
            EventCenter.Instance.RemoveEventListener<float>(EventType.ChangePitch, OnPitchChanged);
            EventCenter.Instance.RemoveEventListener<float>(EventType.ChangeYaw, OnYawChanged);
            EventCenter.Instance.RemoveEventListener<Vector3>(EventType.AimLookAt, OnAimLookAt);
            EventCenter.Instance.RemoveEventListener(EventType.AimRotationRestore, OnAimRotationRestore);
        }

        private void OnAimLookAt(Vector3 lookAtPosition)
        {
            Vector3 currentAimLookAtDirection = aimTransformProvider.Forward();
            Vector3 expectedAimLookAtDirection = lookAtPosition - shooterPitchTransform.position;

            var yawAndPitch = MathUtility.GetYawAndPitchRotation(currentAimLookAtDirection, expectedAimLookAtDirection);

            Debug.Log(currentAimLookAtDirection.normalized + " " + expectedAimLookAtDirection.normalized);
            Debug.Log(yawAndPitch.Item1 + " " + yawAndPitch.Item2);

            RotateYaw(- yawAndPitch.Item1);
            RotatePitch(- yawAndPitch.Item2);
        }

        private void OnYawChanged(float delta) { RotateYaw(delta); }

        private void OnPitchChanged(float delta) { RotatePitch(delta); }

        private void OnShoot(float shootPower) { Shake(); }

        public void Stable() { shooterMotionAnimator.SetBool(Boom, false); }

        private void Shake() { shooterMotionAnimator.SetBool(Boom, true); }

        private void ResetRotation()
        {
            shooterPitchTransform.localRotation = Quaternion.Euler(0, 0, 60);
            shooterYawTransform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        private void OnAimRotationRestore() { ResetRotation(); }

        private void RotateYaw(float delta)
        {
            Debug.Log("RotateYaw" + delta);
            shooterYawTransform.Rotate(shooterYawTransform.up, delta, Space.World);
        }

        private void RotatePitch(float delta)
        {
            Debug.Log("RotatePitch" + delta);
            shooterPitchTransform.Rotate(shooterPitchTransform.forward, delta, Space.World);
        }

    }
}
