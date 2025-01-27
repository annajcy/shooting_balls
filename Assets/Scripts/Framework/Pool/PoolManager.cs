using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Resource;
using Framework.Singleton;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Framework.Pool
{
    public class PoolManager : SingletonBase<PoolManager>
    {
        private PoolManager() { }

        private readonly Dictionary<string, PoolData> poolDict = new Dictionary<string, PoolData>();

        public void Pop<T>(Action<T> action = null) where T : class, IPoolItem, new()
        {
            string name = typeof(T).Name;
            if (poolDict.ContainsKey(name) && poolDict[name].PoolCount > 0) action?.Invoke(poolDict[name].Pop<T>());
            else IPoolItem.Initialize<T>(action);
        }

        public void Push<T>(T poolItem, Action<T> callback = null) where T : class, IPoolItem
        {
            string name = typeof(T).Name;
            if (poolDict.TryGetValue(name, out var pool)) pool.Push(poolItem);
            else
            {
                poolDict.Add(name, new PoolData());
                poolDict[name].Push(poolItem);
            }
            callback?.Invoke(poolItem);
        }

        public void Clear() { poolDict.Clear(); }
    }
}