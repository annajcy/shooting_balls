using System;
using Framework.Pool;
using UnityEngine;
using UnityEngine.Events;

namespace Bullet.Base
{
    public class BulletFactory<T> where T : BaseBullet, new()
    {
        public void CreateBullet(Action<T> callback = null) { PoolManager.Instance.Pop<T>(callback); }
    }
}