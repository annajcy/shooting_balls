using System;
using Cinemachine;
using Framework.Event;
using Framework.Timer;
using Framework.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using EventType = Framework.Event.EventType;

namespace Cameras.Travel
{
    public class TravelManager : MonoBehaviour
    {
        public int hitBulletTime = 500;
        public float hitTimeScale = 0.2f;

        private void OnEnable()
        {
            EventCenter.Instance.AddEventListener(EventType.HitBulletTimeStart, OnHitBulletTimeStart);
        }

        private void OnDisable()
        {
            EventCenter.Instance.RemoveEventListener(EventType.HitBulletTimeStart, OnHitBulletTimeStart);
        }

        private void OnDestroy()
        {
            EventCenter.Instance.RemoveEventListener(EventType.HitBulletTimeStart, OnHitBulletTimeStart);
        }

        public void StartHitBulletTime()
        {
            int bulletTimeTimer = TimerManager.Instance.CreateBulletTimeTimer(hitTimeScale, hitBulletTime,
                hitBulletTime, new TimerEvents());
            TimerManager.Instance.StartTimer(bulletTimeTimer);
        }

        private void OnHitBulletTimeStart()
        {
            EventCenter.Instance.EventTrigger(EventType.EnterArrival);
            StartHitBulletTime();
        }

    }
}