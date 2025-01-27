using System;
using Framework.Event;
using Framework.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Canvases.Arrival
{
    public class ArrivalCanvas : BaseCanvas
    {
        public Button aimButton;
        public Button rebuildButton;

        private void Awake()
        {
            aimButton.onClick.AddListener(OnAimButtonClicked);
            rebuildButton.onClick.AddListener(OnRebuildButtonClicked);
        }

        private void OnDestroy()
        {
            aimButton.onClick.RemoveListener(OnAimButtonClicked);
            rebuildButton.onClick.RemoveListener(OnRebuildButtonClicked);
        }

        private void OnRebuildButtonClicked()
        {
            EventCenter.Instance.EventTrigger(EventType.RebuildTower);
        }

        private void OnAimButtonClicked()
        {
            EventCenter.Instance.EventTrigger(EventType.EnterAim);
        }
    }
}