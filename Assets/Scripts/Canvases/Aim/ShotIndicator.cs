using System;
using System.Collections.Generic;
using Framework.Event;
using TMPro;
using UnityEngine;
using EventType = Framework.Event.EventType;

namespace Canvases.Aim
{
    public enum ShotIndicatorMode
    {
        StandBy = 0,
        Ready = 1,
    }

    [Serializable]
    public class ShotIndicatorData
    {
        public string text;
        public Color color;
    }

    public class ShotIndicator : MonoBehaviour
    {
        public List<ShotIndicatorData> modeData;
        public TextMeshProUGUI text;
        public ShotIndicatorMode currentMode = ShotIndicatorMode.StandBy;

        private void Awake()
        {
            EventCenter.Instance.AddEventListener<ShotIndicatorMode>(EventType.ChangeShotState, OnChangeShotState);
        }

        private void OnDestroy()
        {
            EventCenter.Instance.RemoveEventListener<ShotIndicatorMode>(EventType.ChangeShotState, OnChangeShotState);
        }

        private void Start()
        {
            ChangeMode(ShotIndicatorMode.StandBy);
        }

        private void OnChangeShotState(ShotIndicatorMode mode)
        {
            ChangeMode(mode);
        }

        public void ChangeMode(ShotIndicatorMode mode)
        {
            currentMode = mode;
            text.color = modeData[(int)mode].color;
            text.text = modeData[(int)mode].text;
        }
    }
}