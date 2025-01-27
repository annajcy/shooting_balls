using System.Collections;
using Framework.Mono;
using Framework.Singleton;
using Framework.Event;
using UnityEngine;
using UnityEngine.Events;
using EventType = Framework.Event.EventType;


namespace Framework.Scene
{
    public class SceneManager : SingletonBase<SceneManager>
    {
        private SceneManager() {}

        public void LoadScene(string name, UnityAction callback = null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(name);
            callback?.Invoke();
            callback = null;
        }

        public void LoadSceneAsync(string name, UnityAction callback = null)
        {
            MonoManager.Instance.StartCoroutine(LoadSceneAsyncReal(name, callback));
        }

        private IEnumerator LoadSceneAsyncReal(string name, UnityAction callback = null)
        {
            AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name);
            while (asyncOperation != null && !asyncOperation.isDone)
            {
                EventCenter.Instance.EventTrigger<float>(EventType.SceneLoadChange, asyncOperation.progress);
                yield return 0;
            }
            EventCenter.Instance.EventTrigger<float>(EventType.SceneLoadChange, 1.0f);

            callback?.Invoke();
            callback = null;
        }

    }
}