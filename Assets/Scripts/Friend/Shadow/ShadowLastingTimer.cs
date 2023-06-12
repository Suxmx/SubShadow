using MyTimer;
using System;

public class ShadowLastingTimer : Timer<float, CurrentPercent>
{
    private float conditionalTime;
    private Func<bool> condition;

    public ShadowLastingTimer()
    {
        Origin = 0f;
        Target = 1f;
    }

    public void SetLastingTime(float lastingTime)
    {
        Duration = lastingTime;
    }

    public void AddLastingTime(float addLastingTime)
    {
        Duration += addLastingTime;
    }

    public void AddConditionalLastingTime(float addLastingTime, Func<bool> condition)
    {
        Duration += addLastingTime;
        conditionalTime = addLastingTime;
        this.condition = condition;
        OnTick += CheckCondition;
    }

    private void CheckCondition(float _)
    {
        if (Duration - Time < conditionalTime && condition != null && !condition())
        {
            OnTick -= CheckCondition;
            ForceComplete();
        }
    }

    public void ResetTimer()
    {
        OnTick -= CheckCondition;
        conditionalTime = 0f;
        condition = null;
        ReturnToZero();
    }
}
