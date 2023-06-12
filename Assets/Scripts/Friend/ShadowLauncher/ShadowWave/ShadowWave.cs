using MyTimer;
using UnityEngine;

public class ShadowWave : Bullet, IMovable
{
    private Vector3 defaultScale;
    private TimerOnly appearTimer;
    private int penetrateCount;

    public Vector3 MoveDir => transform.right;

    protected override void Awake()
    {
        base.Awake();
        defaultScale = transform.localScale;
        appearTimer = new TimerOnly();
        // appearTimer.OnTick += x => transform.localScale = new Vector3(
        //     x * speed, transform.localScale.y, 1f);

        myObject.OnRecycle += OnRecycle;
    }

    public void Initialize(Vector3 shootDir, ShadowLauncherInfo shadowLauncherInfo, int penetrateCount)
    {
        base.Initialize(shootDir, shadowLauncherInfo.attackDamage, shadowLauncherInfo.bulletSpeed,
            shadowLauncherInfo.maxFlyingDist);
        transform.localScale = defaultScale * shadowLauncherInfo.missileScale;
        this.penetrateCount = penetrateCount;
        appearTimer.Initialize(transform.localScale.x / speed);
    }

    private void OnRecycle()
    {
        appearTimer.Paused = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && --penetrateCount == 0)
        {
            DestroySelf();
        }
    }
}
