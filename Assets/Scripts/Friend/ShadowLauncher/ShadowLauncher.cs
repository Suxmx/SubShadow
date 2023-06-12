using MyTimer;
using Services;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShadowLauncher : MonoBehaviour
{
    protected ObjectManager objectManager;
    protected EnemyManager enemyManager;
    protected MyObject myObject;

    protected ShootTimer shootTimer;
    protected ShadowLauncherInfo shadowLauncherInfo;

    protected virtual void Awake()
    {
        objectManager = ServiceLocator.Get<ObjectManager>();
        enemyManager = ServiceLocator.Get<EnemyManager>();
        myObject = GetComponent<MyObject>();

        shootTimer = new ShootTimer();
        shootTimer.OnComplete += Shoot;

        myObject.OnRecycle += OnRecycle;
    }

    public void Initialize(ShadowLauncherInfo shadowLauncherInfo)
    {
        this.shadowLauncherInfo = shadowLauncherInfo;
        Shoot();
        shootTimer.Initialize(shadowLauncherInfo.attackInterval);
    }

    private void Shoot()
    {
        List<Enemy> enemies = enemyManager.FindRandomEnemies(transform.position, 
            shadowLauncherInfo.attackDist, shadowLauncherInfo.missileCount);
        foreach (var enemy in enemies)
            ShootToEnemy(enemy);
    }

    protected abstract void ShootToEnemy(Enemy enemy);

    public void DestroySelf()
    {
        myObject.Recycle();
    }

    protected virtual void OnRecycle()
    {
        shootTimer.Paused = true;
    }
}

public class ShootTimer : RepeatTimer
{
    public void SetInterval(float interval)
    {
        Target = Duration = interval;
    }
}
