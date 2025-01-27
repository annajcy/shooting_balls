using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Framework.Event;
using UnityEngine;
using EventType = Framework.Event.EventType;

public class BulletTargetSetter : MonoBehaviour
{
    private CinemachineTargetGroup bulletTargetGroup;

    private void Awake()
    {
        bulletTargetGroup = GetComponent<CinemachineTargetGroup>();
        EventCenter.Instance.AddEventListener<Transform>(EventType.SetBulletTarget, ResetTravelCamera);
    }

    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<Transform>(EventType.SetBulletTarget, ResetTravelCamera);
    }

    private void ResetTravelCamera(Transform cameraTransform)
    {
        bulletTargetGroup.m_Targets = Array.Empty<CinemachineTargetGroup.Target>();
        bulletTargetGroup.AddMember(cameraTransform, 1, 10);
    }

}
