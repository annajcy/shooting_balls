using System;
using Framework.Event;
using Framework.Pool;
using UnityEngine;
using EventType = Framework.Event.EventType;

namespace Bullet.Base
{
    public abstract class BaseBullet : PoolItemMonoBase
    {
        protected Rigidbody bulletRigidbody;

        protected abstract void Initialize();
        protected abstract void OnFire();
        protected abstract void OnCollideWithTower();
        protected abstract void OnCollideWithGround();
        protected abstract void OnCollideWithWall();

        private void Awake()
        {
            bulletRigidbody = GetComponent<Rigidbody>();
            Initialize();
        }

        private void OnEnable()
        {
            Initialize();
        }

        public void Fire(Vector3 direction, float force)
        {
            Debug.Log("Fired at " + force);
            bulletRigidbody.AddForce(direction.normalized * force, ForceMode.Impulse);
            OnFire();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Tower"))
            {
                EventCenter.Instance.EventTrigger(EventType.CollideWithTower);
                OnCollideWithTower();
            }
            else if (other.gameObject.CompareTag("Ground"))
            {
                EventCenter.Instance.EventTrigger(EventType.CollideWithGround);
                OnCollideWithGround();
            }
            else if (other.gameObject.CompareTag("Wall"))
            {
                EventCenter.Instance.EventTrigger(EventType.CollideWithWall);
                OnCollideWithWall();
            }
        }



        public override void OnPush()
        {
            base.OnPush();
            
            bulletRigidbody.WakeUp();
        }

        public override void OnPop()
        {
            base.OnPop();

            bulletRigidbody.velocity = Vector3.zero;
            bulletRigidbody.angularVelocity = Vector3.zero;

            bulletRigidbody.transform.position = GameObject.Find("Aimer").transform.position;
            bulletRigidbody.transform.rotation = GameObject.Find("Aimer").transform.rotation;

            bulletRigidbody.inertiaTensorRotation = Quaternion.identity;
            bulletRigidbody.inertiaTensor = Vector3.one;

            bulletRigidbody.WakeUp();

        }
    }
}