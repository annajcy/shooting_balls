using System;
using System.Reflection;
using UnityEngine;

namespace Framework.Singleton
{
    public abstract class SingletonBaseInitialized<T> where T : class, new()
    {
        private static T instance = new T();
        public static T Instance => instance;
    }
}