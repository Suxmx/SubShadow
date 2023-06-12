using System.Collections.Generic;
using System.Linq;

public enum PlayerSpeedFactorType
{
    /// <summary>
    /// 拾取影子后
    /// </summary>
    AfterCollect,
    /// <summary>
    /// 放置影子后
    /// </summary>
    AfterSetShadow,
    /// <summary>
    /// 影子充能为零时
    /// </summary>
    ShadowChargedNone,
    /// <summary>
    /// 触碰影子后
    /// </summary>
    AfterTouchShadow,
    /// <summary>
    /// 受到伤害后
    /// </summary>
    AfterGetHurt,
    /// <summary>
    /// 穿过影子连线后
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
        // 若影响因素为固定时间类型的，直接设置Effective，否则设置FactorCount
        if (speedFactor.FixedTime) speedFactor.Effective = active;
        else if (active) speedFactor.FactorCount++;
        else speedFactor.FactorCount--;
    }
}
