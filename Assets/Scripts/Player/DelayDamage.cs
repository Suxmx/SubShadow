using MyTimer;

public class DelayDamage
{
    public float damage;
    public CountdownPercentTimer delayTimer;

    public DelayDamage(float damage, float delayTime, DelayDamageManager delayDamageManager)
    {
        this.damage = damage;
        delayTimer = new CountdownPercentTimer();
        delayTimer.OnComplete += () => delayDamageManager.RemoveDelayDamage(this);
        delayTimer.Initialize(delayTime);
    }

    public void RemoveDelay()
    {
        delayTimer.Paused = true;
    }
}
