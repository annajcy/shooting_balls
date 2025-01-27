using System;
using Framework.Resource;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework.Pool
{

    public interface IPoolItem
    {
        public static void Initialize<T>(Action<T> callback = null) where T : class, IPoolItem, new()
        {
            if (typeof(MonoBehaviour).IsAssignableFrom(typeof(T)))
            {
                string name = typeof(T).Name;
                AddressablesManager.Instance.LoadAssetAsync<GameObject>(name, handle =>
                {
                    GameObject go = Object.Instantiate(handle.Result);
                    go.name = name;
                    T poolItem = go.GetComponent<T>();
                    callback?.Invoke(poolItem);
                } );
            }
            else
            {
                T poolItem = new T();
                callback?.Invoke(poolItem);
            }
        }

        public void OnPop();
        public void OnPush();
    }
}