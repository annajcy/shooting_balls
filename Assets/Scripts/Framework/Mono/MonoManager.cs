using System;
using Framework.Singleton;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

namespace Framework.Mono
{
    public class MonoManager : SingletonAutoMono<MonoManager>
    {
        public UnityEvent awakeEvents;
        public UnityEvent updateEvents;
        public UnityEvent fixedUpdateEvents;
        public UnityEvent lateUpdateEvents;

        private void Awake()
        {
            awakeEvents?.Invoke();
        }

        private void Update()
        {
            updateEvents?.Invoke();
        }

        private void LateUpdate()
        {
            lateUpdateEvents?.Invoke();
        }

        private void FixedUpdate()
        {
            fixedUpdateEvents?.Invoke();
        }
    }
}