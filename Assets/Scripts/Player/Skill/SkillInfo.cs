using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    /// <summary>
    /// 旋刃
    /// </summary>
    [Label("旋刃")]
    Whirl,
    /// <summary>
    /// 收割
    /// </summary>
    [Label("收割")]
    Reap,
    /// <summary>
    /// 黑子
    /// </summary>
    [Label("黑子")]
    Sunspot,
    /// <summary>
    /// 反戈
    /// </summary>
    [Label("反戈")]
    TurnWeaponAround,
    /// <summary>
    /// 噩焰
    /// </summary>
    [Label("噩焰")]
    NightmarishFlame,
    /// <summary>
    /// 飞矢
    /// </summary>
    [Label("飞矢")]
    FlyingArrow,
    /// <summary>
    /// 汹浪
    /// </summary>
    [Label("汹浪")]
    RoughSea,
    /// <summary>
    /// 推进
    /// </summary>
    [Label("推进")]
    PushOn,
    /// <summary>
    /// 聚集
    /// </summary>
    [Label("聚集")]
    Gather,
    /// <summary>
    /// 喧哗
    /// </summary>
    [Label("喧哗")]
    ConfusedNoise,
    /// <summary>
    /// 实像
    /// </summary>
    [Label("实像")]
    RealImage,
    /// <summary>
    /// 幽灵
    /// </summary>
    [Label("幽灵")]
    Ghost,
    /// <summary>
    /// 无羁
    /// </summary>
    [Label("无羁")]
    Unbridled,
    /// <summary>
    /// 克隆
    /// </summary>
    [Label("克隆")]
    Clone,
    /// <summary>
    /// 新生
    /// </summary>
    [Label("新生")]
    Reborn,
    /// <summary>
    /// 海纳
    /// </summary>
    [Label("海纳")]
    AllRiversRunIntoSea,
}

[System.Serializable]
public class SkillInfo
{
    [Label("技能种类")]
    public SkillType skillType;
    [Label("技能名字")]
    public string name;
    [Label("技能图标")]
    public Sprite icon;
    [Header("技能描述")]
    [TextArea]
    public List<string> desc;
}
