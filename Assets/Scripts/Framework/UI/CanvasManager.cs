using System;
using System.Collections.Generic;
using Framework.Resource;
using Framework.Singleton;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Framework.UI
{
    public enum CanvasLayer
    {
        Top,
        Middle,
        Bottom,
        System,
    }

    public abstract class CanvasInfoBase
    {
        public bool isLoaded = false;
        protected BaseCanvas canvas = null;

        public T Get<T>() where T : BaseCanvas { return (T)canvas; }

        public void Set(BaseCanvas canvas)
        {
            if (canvas == null) return;
            this.canvas = canvas;
        }

        public CanvasInfo<T> Convert<T>() where T : BaseCanvas
        { return (CanvasInfo<T>)this; }
    }

    public class CanvasInfo<T> : CanvasInfoBase where T : BaseCanvas
    {
        public Action<T> onLoaded;

        public T Get()
        {
            if (canvas == null) return null;
            return canvas as T;
        }
    }

    public class CanvasManager : SingletonBase<CanvasManager>
    {
        private Transform top;
        private Transform middle;
        private Transform bottom;
        private Transform system;

        private CanvasManager()
        {
            top = GameObject.Find("Top").transform;
            middle = GameObject.Find("Middle").transform;
            bottom = GameObject.Find("Bottom").transform;
            system = GameObject.Find("System").transform;
        }

        private Dictionary<string, CanvasInfoBase> canvasDict = new Dictionary<string, CanvasInfoBase>();

        private Transform GetLayerTransform(CanvasLayer canvasLayer)
        {
            if (canvasLayer == CanvasLayer.Bottom) return bottom;
            else if (canvasLayer == CanvasLayer.Middle) return middle;
            else if (canvasLayer == CanvasLayer.Top) return top;
            else if (canvasLayer == CanvasLayer.System) return system;
            return new RectTransform();
        }

        public void ChangeCanvasLayer<T>(CanvasLayer canvasLayer) where T : BaseCanvas
        {
            string name = typeof(T).Name;
            if (canvasDict.TryGetValue(name, out var canvas))
                canvas.Get<T>().gameObject.transform.SetParent(GetLayerTransform(canvasLayer), false);
            else Debug.LogError("Invalid canvas name");
        }

        public T GetCanvas<T>() where T : BaseCanvas
        {
            string name = typeof(T).Name;
            if (canvasDict.TryGetValue(name, out var canvas))
                return canvas.Get<T>();
            return null;
        }

        public void ShowCanvas<T>(CanvasLayer canvasLayer = CanvasLayer.Middle, Action<T> callback = null) where T : BaseCanvas
        {
            string name = typeof(T).Name;

            if (canvasDict.TryGetValue(name, out var value))
            {
                if (value.isLoaded)
                {
                    value.Get<T>().Show();
                    callback?.Invoke(value.Get<T>());
                }
                else value.Convert<T>().onLoaded += callback;
            }
            else
            {
                canvasDict.Add(name, new CanvasInfo<T>());
                canvasDict[name].Convert<T>().onLoaded += callback;
                AddressablesManager.Instance.LoadAssetAsync<GameObject>(name, handle =>
                {
                    GameObject go = Object.Instantiate(handle.Result,
                        GetLayerTransform(canvasLayer), false);
                    go.name = name;
                    T canvas = go.GetComponent<T>();
                    canvasDict[name].Set(canvas);
                    canvasDict[name].Convert<T>().isLoaded = true;
                    canvasDict[name].Convert<T>().onLoaded?.Invoke(canvas);
                });
            }
        }

        public void HideCanvas<T>(Action<T> callback = null) where T : BaseCanvas
        {
            string name = typeof(T).Name;
            if (canvasDict.TryGetValue(name, out var value))
            {
                if (value.isLoaded)
                {
                    value.Get<T>().Hide();
                    callback?.Invoke(value.Get<T>());
                }
                else
                {
                    value.Convert<T>().onLoaded += canvas => { canvas.Hide(); };
                    value.Convert<T>().onLoaded += callback;
                }
            }
            else Debug.LogError("Invalid canvas name");
        }

        public void DestroyCanvas<T>(Action callback = null) where T : BaseCanvas
        {
            string name = typeof(T).Name;
            if (canvasDict.TryGetValue(name, out var value))
            {
                if (value.isLoaded)
                {
                    canvasDict.Remove(name);
                    Object.Destroy(value.Get<T>().gameObject);

                    callback?.Invoke();
                }
                else
                {
                    value.Convert<T>().onLoaded += canvas =>
                    {
                        canvasDict.Remove(name);
                        Object.Destroy(value.Get<T>().gameObject);
                    };

                    value.Convert<T>().onLoaded += canvas => { callback?.Invoke(); };
                }
            } else Debug.LogError("Invalid canvas name");
        }
    }
}