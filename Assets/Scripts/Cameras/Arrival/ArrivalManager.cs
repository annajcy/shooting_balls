using System;
using Canvases.Arrival;
using Cinemachine;
using Framework.Event;
using Framework.Timer;
using Framework.UI;
using UnityEngine;
using EventType = Framework.Event.EventType;

namespace Cameras.Arrival
{
    public class ArrivalManager : MonoBehaviour
    {
        public bool isAutoEnterAim = false;
        public int arrivalTowerStayTime = 1000;
        private void OnEnable()
        {
            EventCenter.Instance.AddEventListener(EventType.BulletDisabled, OnBulletDisabled);
        }

        private void OnDisable()
        {
            EventCenter.Instance.RemoveEventListener(EventType.BulletDisabled, OnBulletDisabled);
        }

        private void OnDestroy()
        {
            EventCenter.Instance.RemoveEventListener(EventType.BulletDisabled, OnBulletDisabled);
        }

        private void AutoEnterAim()
        {
            int enterAimTimer = TimerManager.Instance.CreateTimer(TimerType.Regular,
                arrivalTowerStayTime, arrivalTowerStayTime, new TimerEvents());

            TimerManager.Instance.GetTimer(enterAimTimer).events.finishCallback += i =>
            {
                EventCenter.Instance.EventTrigger(EventType.RebuildTower);
                EventCenter.Instance.EventTrigger(EventType.EnterAim);
            } ;

            TimerManager.Instance.StartTimer(enterAimTimer);
        }

        private void OnBulletDisabled()
        {
            if (isAutoEnterAim) AutoEnterAim();
            else EventCenter.Instance.EventTrigger(EventType.EnterArrivalTower);
        }
    }
}