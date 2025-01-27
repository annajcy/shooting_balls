using System;
using Bullet.Base;
using Bullet.Default;
using Cameras.Aim;
using Framework.Event;
using Framework.Singleton;
using UnityEngine;
using EventType = Framework.Event.EventType;

namespace Bullet
{
    public class BulletSpawner : MonoBehaviour
    {
        public BulletType currentBulletType = BulletType.DefaultBullet;
        public AimTransformProvider aimTransformProvider;

        private void Awake()
        {
            EventCenter.Instance.AddEventListener<float>(EventType.Shoot, OnShoot);
        }

        private void OnDestroy()
        {
            EventCenter.Instance.RemoveEventListener<float>(EventType.Shoot, OnShoot);
        }

        private void FireBullet(BaseBullet bullet, float force)
        {
            bullet.Fire(aimTransformProvider.Forward(), force);
        }

        private void OnShoot(float force)
        {
            if (currentBulletType == BulletType.DefaultBullet)
            {
                SpawnBullet<DefaultBullet>((bulletGameObject) =>
                {
                    Rigidbody rb = bulletGameObject.GetComponent<Rigidbody>();
                    if (rb != null) rb.WakeUp();

                    DefaultBullet bullet = bulletGameObject.GetComponent<DefaultBullet>();
                    EventCenter.Instance.EventTrigger<Transform>(EventType.OnBulletSpawn, bullet.gameObject.transform);
                    FireBullet(bullet, force);
                });
            }
        }

        public void SpawnBullet<T>(Action<T> callback) where T : BaseBullet, new()
        {
            var bulletFactory = new BulletFactory<T>();
            bulletFactory.CreateBullet(callback);
        }

    }
}
