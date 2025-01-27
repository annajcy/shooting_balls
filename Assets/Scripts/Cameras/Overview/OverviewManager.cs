using System;
using System.Collections.Generic;
using Cameras.Overview.States;
using Cinemachine;
using Framework.Event;
using StateMachines;
using UnityEngine;
using UnityEngine.Events;
using Framework.Input;
using Framework.StateMachine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using EventType = Framework.Event.EventType;

namespace Cameras.Overview
{
    public class OverviewManager : StateMachineBehaviour<OverviewStateMachine>
    {
        public GameObject allViewCamera;
        public GameObject towerViewCamera;
        public GameObject shooterViewCamera;

        protected override void InitializeBehaviour()
        {
            EventCenter.Instance.AddEventListener(EventType.ViewAllObject, OnViewAllObject);
            EventCenter.Instance.AddEventListener(EventType.ViewShooter, OnViewShooter);
            EventCenter.Instance.AddEventListener(EventType.ViewTower, OnViewTower);

            InputManager.Instance.Overview.SelectView.performed += OnSelectViewTriggered;
        }

        private void OnDisable()
        {
            EventCenter.Instance.RemoveEventListener(EventType.ViewAllObject, OnViewAllObject);
            EventCenter.Instance.RemoveEventListener(EventType.ViewShooter, OnViewShooter);
            EventCenter.Instance.RemoveEventListener(EventType.ViewTower, OnViewTower);

            InputManager.Instance.Overview.SelectView.performed -= OnSelectViewTriggered;
        }

        private void OnDestroy()
        {
            EventCenter.Instance.RemoveEventListener(EventType.ViewAllObject, OnViewAllObject);
            EventCenter.Instance.RemoveEventListener(EventType.ViewShooter, OnViewShooter);
            EventCenter.Instance.RemoveEventListener(EventType.ViewTower, OnViewTower);

            InputManager.Instance.Overview.SelectView.performed -= OnSelectViewTriggered;
        }

        protected override void UpdateBehaviour() { }

        protected override void InitializeStateMachine() { stateMachine = new OverviewStateMachine(this); }


        private void OnSelectViewTriggered(InputAction.CallbackContext context)
        {
            Vector3 position = new Vector3(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y, 0.0f);
            GameObject selectedObject = RaycastUtility.GetCameraRaySelect(position);
            if (selectedObject == null) return;
            if (selectedObject.CompareTag("Shooter"))
                EventCenter.Instance.EventTrigger(EventType.ViewShooter);
            else if (selectedObject.CompareTag("Tower"))
                EventCenter.Instance.EventTrigger(EventType.ViewTower);
        }

        private void OnViewAllObject() { stateMachine.ChangeState<AllViewState>(); }

        private void OnViewTower() { stateMachine.ChangeState<TowerViewState>(); }

        private void OnViewShooter() { stateMachine.ChangeState<ShooterViewState>(); }

    }
}