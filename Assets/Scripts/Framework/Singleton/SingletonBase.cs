using System;
using System.Reflection;
using UnityEngine;

namespace Framework.Singleton
{
    public abstract class SingletonBase<T> where T : class
    {
        private static T instance;
        protected static readonly object lockObject = new object();

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        Type type = typeof(T);
                        ConstructorInfo info = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,
                            null, Type.EmptyTypes, null);
                        if (info == null) Debug.LogError("No existence of parameterless constructor");
                        else instance = info.Invoke(null) as T;
                    }
                }

                return instance;
            }
        }
    }
}