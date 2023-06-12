using System.Collections.Generic;
using System.Linq;

public enum EnemySpeedFactorType
{
    /// <summary>
    /// ����
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
        // ��Ӱ������Ϊ�̶�ʱ�����͵ģ�ֱ������Effective����������FactorCount
        if (speedFactor.FixedTime) speedFactor.Effective = true;
        else speedFactor.FactorCount++;
    }

    public void InactivateSpeedFactor(EnemySpeedFactorType factorType)
    {
        SpeedFactor speedFactor = speedFactorDict[factorType];
        // ��Ӱ������Ϊ�̶�ʱ�����͵ģ�ֱ������Effective����������FactorCount
        if (speedFactor.FixedTime) speedFactor.Effective = false;
        else speedFactor.FactorCount--;
    }
}
