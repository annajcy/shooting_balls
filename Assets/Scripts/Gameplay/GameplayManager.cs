using System;
using Cameras.Aim;
using Cameras.Arrival;
using Cameras.ArrivalTower;
using Cameras.Overview;
using Cameras.Travel;
using Framework.Event;
using Framework.Pool;
using Framework.Resource;
using Framework.StateMachine;
using Gameplay.State;
using UnityEngine;
using EventType = Framework.Event.EventType;

namespace Gameplay
{
    public class GameplayManager : StateMachineBehaviour<GameplayStateMachine>
    {
        public OverviewManager overviewManager;
        public AimManager aimManager;
        public TravelManager travelManager;
        public ArrivalManager arrivalManager;
        public ArrivalTowerManager arrivalTowerManager;

        protected override void InitializeBehaviour()
        {
            EventCenter.Instance.AddEventListener(EventType.EnterOverview, OnEnterOverview);
            EventCenter.Instance.AddEventListener(EventType.EnterAim, OnEnterAim);
            EventCenter.Instance.AddEventListener(EventType.EnterTravel, OnEnterTravel);
            EventCenter.Instance.AddEventListener(EventType.EnterArrival, OnEnterArrival);
            EventCenter.Instance.AddEventListener(EventType.EnterArrivalTower, OnEnterArrivalTower);
            EventCenter.Instance.AddEventListener(EventType.RebuildTower, OnTowerRebuild);
        }

        private void OnDestroy()
        {
            EventCenter.Instance.RemoveEventListener(EventType.EnterOverview, OnEnterOverview);
            EventCenter.Instance.RemoveEventListener(EventType.EnterAim, OnEnterAim);
            EventCenter.Instance.RemoveEventListener(EventType.EnterTravel, OnEnterTravel);
            EventCenter.Instance.RemoveEventListener(EventType.EnterArrival, OnEnterArrival);
            EventCenter.Instance.AddEventListener(EventType.EnterArrivalTower, OnEnterArrivalTower);
            EventCenter.Instance.RemoveEventListener(EventType.RebuildTower, OnTowerRebuild);
        }

        protected override void InitializeStateMachine() { stateMachine = new GameplayStateMachine(this); }

        protected override void UpdateBehaviour() { }

        private void OnEnterArrival() { stateMachine.ChangeState<ArrivalState>(); }

        private void OnEnterTravel() { stateMachine.ChangeState<TravelState>(); }

        private void OnEnterOverview() { stateMachine.ChangeState<OverviewState>(); }

        private void OnEnterAim() { stateMachine.ChangeState<AimState>(); }

        private void OnEnterArrivalTower() { stateMachine.ChangeState<ArrivalTowerState>(); }

        private void OnTowerRebuild()
        {
            GamePrefabLoader.Instance.UnloadTower();
            GamePrefabLoader.Instance.LoadTower();
        }

    }
}