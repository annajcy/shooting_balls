using UnityEngine;

namespace Framework.Singleton
{
    public abstract class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance => instance;

        protected abstract void Initialize();

        protected virtual void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
                return;
            }

            instance = this as T;
            DontDestroyOnLoad(gameObject);

            Initialize();
        }
    }
}