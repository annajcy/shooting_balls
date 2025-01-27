using UnityEngine;

namespace Framework.Singleton
{
    public class SingletonAutoMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject gameObject = new GameObject();
                    gameObject.name = typeof(T).ToString();
                    instance = gameObject.AddComponent<T>();
                    DontDestroyOnLoad(gameObject);
                }

                return instance;
            }
        }
    }
}