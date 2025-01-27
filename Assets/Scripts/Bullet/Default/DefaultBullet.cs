using System;
using System.Collections.Generic;
using Bullet.Base;
using Framework.Event;
using Framework.Pool;
using Framework.Timer;
using UnityEngine;
using EventType = Framework.Event.EventType;

namespace Bullet.Default
{
    public class DefaultBullet : BaseBullet
    {
        public bool disableAfterCollidedWall = true;
        public float disableAfterCollidedWallTime = 0.0f;

        public bool disableAfterCollidedTower = false;
        public float disableAfterCollidedTowerTime = 2.0f;

        public bool disableAfterCollidedGround = true;
        public float disableAfterCollidedGroundTime = 2.0f;

        public bool disableAfterLaunched = true;
        public float disableAfterLaunchedTime = 5.0f;

        private List<int> timerList = new List<int>();

        protected override void Initialize()
        {
            EventCenter.Instance.AddEventListener(EventType.RemoveBullet, OnBulletRemove);
        }

        private void OnDisable()
        {
            EventCenter.Instance.RemoveEventListener(EventType.RemoveBullet, OnBulletRemove);
        }

        private void OnDestroy()
        {
            EventCenter.Instance.RemoveEventListener(EventType.RemoveBullet, OnBulletRemove);
        }

        private void OnBulletRemove() { Disable(); }

        protected override void OnFire()
        {
            bulletRigidbody.useGravity = false;
            if (disableAfterLaunched)
                DelayDisable(disableAfterLaunchedTime);
        }

        protected override void OnCollideWithTower()
        {
            bulletRigidbody.useGravity = true;
            if (disableAfterCollidedTower)
                DelayDisable(disableAfterCollidedTowerTime);
        }

        protected override void OnCollideWithGround()
        {
            bulletRigidbody.useGravity = true;
            if (disableAfterCollidedGround)
                DelayDisable(disableAfterCollidedGroundTime);
        }

        protected override void OnCollideWithWall()
        {
            bulletRigidbody.useGravity = true;
            if (disableAfterCollidedWall)
                DelayDisable(disableAfterCollidedWallTime);
        }

        private void Disable()
        {
            PoolManager.Instance.Push(this);
            StopAllDisableTimer();
            EventCenter.Instance.EventTrigger(EventType.BulletDisabled);
        }

        private void DelayDisable(float time)
        {
            int timerID = TimerManager.Instance.CreateTimer(TimerType.Regular, (int)(time * 1000), (int)(time * 1000),
                new TimerEvents());
            TimerManager.Instance.GetTimer(timerID).events.finishCallback += (_) => { Disable(); };
            TimerManager.Instance.StartTimer(timerID);
            timerList.Add(timerID);
        }

        private void StopAllDisableTimer()
        {
            foreach (var timerID in timerList)
                TimerManager.Instance.AddToRemoveList(timerID);
            timerList.Clear();
        }
    }
}