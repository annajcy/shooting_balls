using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Framework.StateMachine
{
    public abstract class BaseStateMachine
    {
        private Dictionary<string, IBaseState> stateDict = new Dictionary<string, IBaseState>();
        private string currentState = string.Empty;

        public IBaseState CurrentBaseState
        {
            get
            {
                if (currentState == string.Empty) return null;
                return stateDict[currentState];
            }
        }

        public void InitializeStateMachine(List<IBaseState> stateList)
        {
            foreach (var state in stateList)
                AddState(state);
        }

        public void InitializeStateMachine<T>(List<IBaseState> stateList) where T : IBaseState
        {
            foreach (var state in stateList)
                AddState(state);
            ChangeState<T>();
        }

        public void UpdateState() { CurrentBaseState?.OnUpdate(); }

        public bool AddState(IBaseState baseState)
        {
            string name = baseState.GetType().Name;
            if (!stateDict.TryAdd(name, baseState)) return true;
            return false;
        }

        public void ChangeEmptyState()
        {
            if (currentState == string.Empty) return;
            CurrentBaseState?.OnQuit();
            currentState = string.Empty;
        }

        public void ChangeState<T>()
        {
            string name = typeof(T).Name;
            if (currentState == name) return;
            CurrentBaseState?.OnQuit();

            if (stateDict.ContainsKey(name))
            {
                currentState = name;
                CurrentBaseState?.OnEnter();
            }
            else Debug.LogError("Invalid state");
        }

    }
}