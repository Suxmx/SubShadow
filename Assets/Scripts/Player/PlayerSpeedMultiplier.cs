using System.Collections.Generic;
using System.Linq;

public enum PlayerSpeedFactorType
{
    /// <summary>
    /// ʰȡӰ�Ӻ�
    /// </summary>
    AfterCollect,
    /// <summary>
    /// ����Ӱ�Ӻ�
    /// </summary>
    AfterSetShadow,
    /// <summary>
    /// Ӱ�ӳ���Ϊ��ʱ
    /// </summary>
    ShadowChargedNone,
    /// <summary>
    /// ����Ӱ�Ӻ�
    /// </summary>
    AfterTouchShadow,
    /// <summary>
    /// �ܵ��˺���
    /// </summary>
    AfterGetHurt,
    /// <summary>
    /// ����Ӱ�����ߺ�
    /// </summary>
    AfterCrossShadowLine,
}

public class PlayerSpeedMultiplier : CharacterSpeedMultiplier
{
    private readonly Dictionary<PlayerSpeedFactorType, SpeedFactor> speedFactorDict;

    public PlayerSpeedMultiplier(SkillInfoData skillInfoData) : base()
    {
        speedFactorDict = new Dictionary<PlayerSpeedFactorType, SpeedFactor>
        {
            //{ PlayerSpeedFactorType.AfterCollect, new SpeedFactor(
            //    skillInfoData.speedFactorMultiplier_AfterCollect, 
            //    skillInfoData.speedFactorLastingTime_AfterCollect, this) }, 
            //{ PlayerSpeedFactorType.AfterSetShadow, new SpeedFactor(
            //    skillInfoData.speedFactorMultiplier_AfterSetShadow, 
            //    skillInfoData.speedFactorLastingTime_AfterSetShadow, this) }, 
            //{ PlayerSpeedFactorType.ShadowChargedNone, new SpeedFactor(
            //    skillInfoData.speedFactorMultiplier_ShadowChargedNone, this) }, 
            //{ PlayerSpeedFactorType.AfterTouchShadow, new SpeedFactor(
            //    skillInfoData.speedFactorMultiplier_AfterTouchShadow, 
            //    skillInfoData.speedFactorLastingTime_AfterTouchShadow, this) }, 
            //{ PlayerSpeedFactorType.AfterGetHurt, new SpeedFactor(
            //    skillInfoData.speedFactorMultiplier_AfterGetHurt, 
            //    skillInfoData.speedFactorLastingTime_AfterGetHurt, this) }, 
            //{ PlayerSpeedFactorType.AfterCrossShadowLine, new SpeedFactor(
            //    skillInfoData.speedFactorMultiplier_AfterCrossShadowLine, 
            //    skillInfoData.speedFactorLastingTime_AfterCrossShadowLine, this) }, 
        };
        speedFactors = speedFactorDict.Values.ToList();
    }

    public void SetSpeedFactorActive(PlayerSpeedFactorType factorType, bool active)
    {
        SpeedFactor speedFactor = speedFactorDict[factorType];
        // ��Ӱ������Ϊ�̶�ʱ�����͵ģ�ֱ������Effective����������FactorCount
        if (speedFactor.FixedTime) speedFactor.Effective = active;
        else if (active) speedFactor.FactorCount++;
        else speedFactor.FactorCount--;
    }
}
