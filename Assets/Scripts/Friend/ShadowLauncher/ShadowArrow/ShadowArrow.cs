using MyTimer;
using UnityEngine;

public class ShadowArrow : NormalBullet
{
    private TimerOnly appearTimer;

    protected override void Awake()
    {
        base.Awake();
        appearTimer = new TimerOnly();
        appearTimer.OnTick += x => transform.localScale = new Vector3(
            x * speed, transform.localScale.y, 1f);

        myObject.OnRecycle += OnRecycle;
    }

    public void Initialize(Vector3 shootDir, ShadowLauncherInfo shadowLauncherInfo)
    {
        base.Initialize(shootDir, shadowLauncherInfo.attackDamage, shadowLauncherInfo.bulletSpeed, 
            shadowLauncherInfo.maxFlyingDist);
        transform.localScale = defaultScale * shadowLauncherInfo.missileScale;
        appearTimer.Initialize(transform.localScale.x / speed);
    }

    private void OnRecycle()
    {
        appearTimer.Paused = true;
    }
}
