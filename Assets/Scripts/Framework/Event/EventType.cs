namespace Framework.Event
{
    public enum EventType
    {
        SceneLoadChange,

        //Gameplay mode change events
        EnterOverview,
        EnterAim,
        EnterTravel,
        EnterArrival,

        //OverviewModeEvent
        ViewAllObject,
        ViewShooter,
        ViewTower,

        //Aim mode events
        Shoot,
        ChangePitch,
        ChangeYaw,
        ChangeShotState,
        ToggleAimInput,
        OnBulletSpawn,

        AimLookAt,
        AimInputChanged,
        AimRotationRestore,

        //Travel and Arrival mode events
        HitBulletTimeStart,

        CollideWithTower,
        CollideWithGround,
        CollideWithWall,

        BulletDisabled,
        RebuildTower,


        SetAimInput,
        RemoveBullet,
        SetBulletTarget,
        EnterArrivalTower
    }
}