using Framework.Event;
using Framework.UI;
using UnityEngine.UI;
using EventType = Framework.Event.EventType;

namespace Canvases.OverView
{
    public class OverviewCanvas : BaseCanvas
    {
        public Button viewAllButton;
        public Button aimButton;
        public Button rebuildButton;

        private void Start()
        {
            viewAllButton.onClick.AddListener(OnViewAllButtonClicked);
            aimButton.onClick.AddListener(OnAimButtonClicked);
            rebuildButton.onClick.AddListener(OnRebuildButtonClicked);
        }

        private void OnRebuildButtonClicked()
        {
            EventCenter.Instance.EventTrigger(EventType.RebuildTower);
        }

        private void OnViewAllButtonClicked()
        {
            EventCenter.Instance.EventTrigger(EventType.ViewAllObject);
        }

        private void OnAimButtonClicked()
        {
            EventCenter.Instance.EventTrigger(EventType.EnterAim);
        }

        private void OnDestroy()
        {
            viewAllButton.onClick.RemoveListener(OnViewAllButtonClicked);
            aimButton.onClick.RemoveListener(OnAimButtonClicked);
            rebuildButton.onClick.RemoveListener(OnRebuildButtonClicked);
        }
    }
}
