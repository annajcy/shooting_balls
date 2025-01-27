using Cameras.Aim;
using Framework.Event;
using Framework.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using EventType = Framework.Event.EventType;

namespace Canvases.Aim
{
    public class AimCanvas : BaseCanvas
    {
        public Button overviewButton;
        public Button changeInputButton;
        public TextMeshProUGUI tmpText;

        private const string touch = "Touch";
        private const string slingShot = "Sling Shot";

        private void Awake()
        {
            overviewButton.onClick.AddListener(OnOverviewButtonClicked);
            changeInputButton.onClick.AddListener(OnChangeInteractionButton);

            EventCenter.Instance.AddEventListener<AimInputType>(EventType.AimInputChanged, OnAimInputChanged);
        }

        private void OnDestroy()
        {
            overviewButton.onClick.RemoveListener(OnOverviewButtonClicked);
            changeInputButton.onClick.RemoveListener(OnChangeInteractionButton);

            EventCenter.Instance.RemoveEventListener<AimInputType>(EventType.AimInputChanged, OnAimInputChanged);
        }

        private void OnAimInputChanged(AimInputType type)
        {
            Debug.Log("OnAimInputChanged");
            if (type == AimInputType.SlingShot)
                tmpText.text = slingShot;
            else if (type == AimInputType.Touch)
                tmpText.text = touch;
        }

        private void OnChangeInteractionButton()
        {
            EventCenter.Instance.EventTrigger(EventType.ToggleAimInput);
        }

        private void OnOverviewButtonClicked()
        {
            EventCenter.Instance.EventTrigger(EventType.EnterOverview);
        }
    }
}