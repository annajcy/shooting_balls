using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Framework.Pool
{
    public class PoolData
    {
        private readonly Queue<IPoolItem> poolQueue = new Queue<IPoolItem>();

        public int PoolCount => poolQueue.Count;

        public PoolData() { }

        protected virtual void Activate(IPoolItem poolItem) { }
        protected virtual void Deactivate(IPoolItem poolItem) { }

        public void Push(IPoolItem poolItem)
        {
            poolQueue.Enqueue(poolItem);
            Activate(poolItem);
            poolItem.OnPush();
        }

        public T Pop<T>() where T : class, IPoolItem
        {
            var poolItem = poolQueue.Dequeue();
            Deactivate(poolItem);
            poolItem.OnPop();
            return poolItem as T;
        }
    }
}