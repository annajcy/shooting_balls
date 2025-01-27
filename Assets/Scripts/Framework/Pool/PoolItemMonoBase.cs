using System;
using Framework.Resource;
using UnityEngine;

namespace Framework.Pool
{
    public abstract class PoolItemMonoBase : MonoBehaviour, IPoolItem
    {
        public virtual void OnPop()
        {
            gameObject.SetActive(true);
        }

        public virtual void OnPush()
        {
            gameObject.SetActive(false);
        }
    }
}