using MyTimer;
using UnityEngine;

public class Mist : PlayerTrigger
{
    private RepeatTimer releaseTimer;
    private EaseFloatTimer radiusTimer;
    public int attractCount;

    protected override void Awake()
    {
        base.Awake();

        releaseTimer = new RepeatTimer();
        releaseTimer.OnComplete += () => radiusTimer.Restart();

        radiusTimer = new EaseFloatTimer(EaseType.OutQuad);
        radiusTimer.OnTick += x => SetRadius(x);
        radiusTimer.OnComplete += () => SetRadius(0f);
    }

    public void Initialize(float releaseInterval, float maxRadius, float releaseDuration)
    {
        radiusTimer.Initialize(0, maxRadius, releaseDuration);
        releaseTimer.Initialize(releaseInterval);
        attractCount = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Bullet>(out var bullet))
        {
            bullet.DestroySelf();
            attractCount++;
        }
    }

    protected override void OnRecycle()
    {
        base.OnRecycle();
        releaseTimer.Paused = true;
        radiusTimer.Paused = true;
    }
}
