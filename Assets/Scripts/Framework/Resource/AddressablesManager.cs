using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Singleton;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Framework.Resource
{
    public abstract class ReferenceCountBase
    {
        private int referenceCount = 1;
        public int ReferenceCount => referenceCount;

        public void AddReferenceCount() { referenceCount++; }

        /// <summary>
        /// Remove Reference Count
        /// </summary>
        /// <returns>
        /// True : represent it is required to release after removed reference count
        /// False : represent it is required not to release after removed reference count
        /// </returns>
        public bool RemoveReferenceCount()
        {
            referenceCount--;
            if (referenceCount == 0) return true;
            return false;
        }
    }

    public class AddressablesItem : ReferenceCountBase
    {
        public AsyncOperationHandle handle;
        public AddressablesItem(AsyncOperationHandle handle) { this.handle = handle; }
    }


    public class AddressablesManager : SingletonBase<AddressablesManager>
    {
        private readonly Dictionary<string, AddressablesItem> resourceDict = new Dictionary<string, AddressablesItem>();

        private AddressablesManager() {}

        private static string GetKeyName<T>(string name)
        {
            string keyName = name + "_" + typeof(T).Name;
            return keyName;
        }

        private static string GetKeyName<T>(List<string> nameList)
        {
            string keyName = String.Empty;
            for (int i = 0; i < nameList.Count; i++)
                keyName += nameList[i] + "_";
            keyName += typeof(T).Name;
            return keyName;
        }

        public void LoadAssetAsync<T>(string name, Action<AsyncOperationHandle<T>> callback)
        {
            var keyName = GetKeyName<T>(name);
            if (resourceDict.TryGetValue(keyName, out var value))
            {
                value.AddReferenceCount();
                AsyncOperationHandle<T> handle = value.handle.Convert<T>();
                if (handle.IsDone) callback(handle);
                else
                {
                    handle.Completed += (param) =>
                    {
                        if (param.Status == AsyncOperationStatus.Succeeded)
                            callback(param);
                    };
                }
            }
            else
            {
                AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(name);
                handle.Completed += (param) =>
                {
                    if (param.Status == AsyncOperationStatus.Succeeded) callback(param);
                    else
                    {
                        Debug.LogWarning(keyName + ": Resource Load Failed.");
                        if (resourceDict.ContainsKey(keyName)) resourceDict.Remove(keyName);
                    }
                };
                resourceDict.Add(keyName, new AddressablesItem(handle));
            }
        }


        public void LoadAssetAsync<T>(List<string> nameList, Addressables.MergeMode mode,
            Action<AsyncOperationHandle<IList<T>>> callback)
        {
            string keyName = GetKeyName<T>(nameList);

            if (resourceDict.TryGetValue(keyName, out var value))
            {
                value.AddReferenceCount();
                AsyncOperationHandle<IList<T>> handle = value.handle.Convert<IList<T>>();
                if (handle.IsDone) callback(handle);
                else
                {
                    handle.Completed += (param) =>
                    {
                        if (param.Status == AsyncOperationStatus.Succeeded)
                            callback(handle);
                    };
                }
            }
            else
            {
                AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(nameList, (_) => { }, mode);
                handle.Completed += (param) =>
                {
                    if (param.Status == AsyncOperationStatus.Failed)
                    {
                        Debug.LogWarning(keyName + ": Resource Load Failed.");
                        if (resourceDict.ContainsKey(keyName))
                            resourceDict.Remove(keyName);
                    }
                };
                resourceDict.Add(keyName, new AddressablesItem(handle));
            }
        }

        public void LoadAssetAsync<T>(List<string> nameList, Addressables.MergeMode mode, Action<T> callback)
        {
            string keyName = GetKeyName<T>(nameList);

            if (resourceDict.TryGetValue(keyName, out var value))
            {
                value.AddReferenceCount();
                AsyncOperationHandle<IList<T>> handle = value.handle.Convert<IList<T>>();
                if (handle.IsDone)
                {
                    foreach (var item in handle.Result)
                        callback(item);
                }
                else
                {
                    handle.Completed += (param) =>
                    {
                        if (param.Status == AsyncOperationStatus.Succeeded)
                            foreach (var item in handle.Result)
                                callback(item);
                    };
                }
            }
            else
            {
                AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(nameList, callback, mode);
                handle.Completed += (param) =>
                {
                    if (param.Status == AsyncOperationStatus.Failed)
                    {
                        Debug.LogWarning(keyName + ": Resource Load Failed.");
                        if (resourceDict.ContainsKey(keyName))
                            resourceDict.Remove(keyName);
                    }
                };
                resourceDict.Add(keyName, new AddressablesItem(handle));
            }
        }

        public void Release<T>(List<string> nameList)
        {
            var keyName = GetKeyName<T>(nameList);

            if (resourceDict.TryGetValue(keyName, out var value))
            {
                if (value.RemoveReferenceCount())
                {
                    AsyncOperationHandle<IList<T>> handle = resourceDict[keyName].handle.Convert<IList<T>>();
                    Addressables.Release(handle);
                    resourceDict.Remove(keyName);
                }
            }
        }

        public void Release<T>(string name)
        {
            var keyName = GetKeyName<T>(name);

            if (resourceDict.TryGetValue(keyName, out var value))
            {
                if (value.RemoveReferenceCount())
                {
                    AsyncOperationHandle<T> handle = resourceDict[keyName].handle.Convert<T>();
                    Addressables.Release(handle);
                    resourceDict.Remove(keyName);
                }
            }
        }

        public void Clear()
        {
            foreach (var item in resourceDict.Values)
                Addressables.Release(item.handle);
            resourceDict.Clear();
        }

    }
}
