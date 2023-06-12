using System.Collections.Generic;

public class DelayDamageManager
{
    private float delayTime;
    private readonly PlayerStatusInfo playerStatusInfo;
    private readonly List<DelayDamage> delayDamageList;

    public DelayDamageManager(PlayerStatusInfo playerStatusInfo)
    {
        this.playerStatusInfo = playerStatusInfo;
        delayDamageList = new List<DelayDamage>();
    }

    public void SetDelayTime(float delayTime)
    {
        this.delayTime = delayTime;
    }

    public DelayDamage AddDelayDamage(float damage)
    {
        DelayDamage delayDamage = new DelayDamage(damage, delayTime, this);
        delayDamageList.Add(delayDamage);
        return delayDamage;
    }

    public void RemoveDelayDamage(DelayDamage delayDamage)
    {
        playerStatusInfo.GetRealHurt(delayDamage.damage);
        delayDamageList.Remove(delayDamage);
    }

    public void ClearAllDelayDamages()
    {
        foreach (var delayDamage in delayDamageList)
        {
            delayDamage.RemoveDelay();
        }
        delayDamageList.Clear();
    }
}
