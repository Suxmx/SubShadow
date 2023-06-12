using MyTimer;

public class InstantaneousTrigger : PlayerTrigger
{
    protected CountdownTimer destroyTimer;

    protected override void Awake()
    {
        base.Awake();
        destroyTimer = new CountdownTimer();
        destroyTimer.OnComplete += DestroySelf;
        destroyTimer.Initialize(0.1f, false);
    }

    protected override void OnActivate()
    {
        base.OnActivate();
        destroyTimer.Restart();
    }

    protected override void OnRecycle()
    {
        base.OnRecycle();
        destroyTimer.Paused = true;
    }
}
