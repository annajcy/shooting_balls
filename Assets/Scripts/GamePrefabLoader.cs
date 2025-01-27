using System;
using System.Collections;
using System.Collections.Generic;
using Bullet;
using Cameras.Aim;
using Cinemachine;
using Framework.Pool;
using Framework.Resource;
using Framework.Singleton;
using Framework.StateMachine;
using Framework.UI;
using Gameplay;
using Gameplay.State;
using UnityEngine;

public class GamePrefabLoader : SingletonMono<GamePrefabLoader>
{
    public GameplayManager gameplayManager;

    public CinemachineTargetGroup towerTargetGroup;
    public CinemachineTargetGroup shooterTargetGroup;
    public CinemachineTargetGroup allTargetGroup;
    public CinemachineTargetGroup aimTargetGroup;
    public CinemachineTargetGroup bulletTargetGroup;

    protected override void Initialize()
    {
        LoadEnvironment();
        LoadTower();
        LoadShooter();
    }

    public void LoadEnvironment()
    {
        AddressablesManager.Instance.LoadAssetAsync<GameObject>("Environment", handle =>
        {
            GameObject environment = Instantiate(handle.Result);
            environment.name = "Environment";
        });
    }

    public void LoadShooter()
    {
        AddressablesManager.Instance.LoadAssetAsync<GameObject>("Shooter", handle =>
        {
            GameObject shooter = Instantiate(handle.Result);
            shooter.name = "Shooter";
            shooterTargetGroup.AddMember(shooter.transform, 1, 10);
            allTargetGroup.AddMember(shooter.transform, 1, 10);

            Transform aimer = GameObject.Find("CameraAimer").transform;
            aimTargetGroup.AddMember(aimer, 1, 10);

            Transform bullet = GameObject.Find("Aimer").transform;
            bulletTargetGroup.AddMember(bullet, 1, 10);

        });
    }

    public void LoadTower()
    {
        AddressablesManager.Instance.LoadAssetAsync<GameObject>("Tower", handle =>
        {
            GameObject tower = Instantiate(handle.Result);
            tower.name = "Tower";
            towerTargetGroup.AddMember(tower.transform, 1, 10);
            allTargetGroup.AddMember(tower.transform, 1, 10);
        });
    }

    public void UnloadTower()
    {
        GameObject tower = GameObject.Find("Tower");
        towerTargetGroup.RemoveMember(tower.transform);
        allTargetGroup.RemoveMember(tower.transform);
        DestroyImmediate(tower);
    }

    private void Start()
    {
        gameplayManager.stateMachine.ChangeState<AimState>();
    }

}
