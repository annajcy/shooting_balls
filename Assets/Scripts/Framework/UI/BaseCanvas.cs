using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;


namespace Framework.UI
{
    public abstract class BaseCanvas : MonoBehaviour
    {
        public bool IsActive => gameObject.activeSelf;

        protected virtual void OnShow() {}
        protected virtual void OnHide() {}

        public void Show()
        {
            gameObject.SetActive(true);
            OnShow();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            OnHide();
        }

        public void Toggle()
        {
            if (IsActive) Hide();
            else Show();
        }
    }
}