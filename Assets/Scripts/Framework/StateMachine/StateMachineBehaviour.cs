using System;
using UnityEngine;

namespace Framework.StateMachine
{
    public abstract class StateMachineBehaviour<T> : MonoBehaviour  where T : BaseStateMachine
    {
        public T stateMachine;

        protected abstract void InitializeStateMachine();
        protected abstract void InitializeBehaviour();
        protected abstract void UpdateBehaviour();

        private void Awake() { InitializeStateMachine(); }

        private void OnEnable() { InitializeBehaviour(); }

        private void Update()
        {
            stateMachine.UpdateState();
            UpdateBehaviour();
        }

    }
}