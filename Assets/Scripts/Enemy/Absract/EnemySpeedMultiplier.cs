using System.Collections.Generic;
using System.Linq;

public enum EnemySpeedFactorType
{
    /// <summary>
    /// 涡流
    /// </summary>
    Vortex,
}

public class EnemySpeedMultiplier : CharacterSpeedMultiplier
{
    protected Dictionary<EnemySpeedFactorType, SpeedFactor> speedFactorDict;

    public EnemySpeedMultiplier()
    {
        speedFactorDict = new Dictionary<EnemySpeedFactorType, SpeedFactor>
        {
            { EnemySpeedFactorType.Vortex, new SpeedFactor(this) },
        };
        speedFactors = speedFactorDict.Values.ToList();
    }

    public void ActivateSpeedFactor(EnemySpeedFactorType factorType, float multiplier)
    {
        SpeedFactor speedFactor = speedFactorDict[factorType];
        speedFactor.multiplier = multiplier;
        // 若影响因素为固定时间类型的，直接设置Effective，否则设置FactorCount
        if (speedFactor.FixedTime) speedFactor.Effective = true;
        else speedFactor.FactorCount++;
    }

    public void InactivateSpeedFactor(EnemySpeedFactorType factorType)
    {
        SpeedFactor speedFactor = speedFactorDict[factorType];
        // 若影响因素为固定时间类型的，直接设置Effective，否则设置FactorCount
        if (speedFactor.FixedTime) speedFactor.Effective = false;
        else speedFactor.FactorCount--;
    }
}
