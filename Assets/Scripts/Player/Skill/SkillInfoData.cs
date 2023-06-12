using System.Collections.Generic;
using UnityEngine;

public class SkillInfoData : ScriptableObject
{
    [Header("攻击技能信息")]
    public List<SkillInfo> attackSkillInfos;
    [Header("辅助技能信息")]
    public List<SkillInfo> assistantSkillInfos;

    [Space(5)]
    [Header("系统属性初始值")]
    [Label("基础威力")]
    public float basicDamage;
    [Label("生命上限")]
    public int playerMaxHP;
    [Label("移速")]
    public float playerSpeed;
    [Label("充能量")]
    public int shadowChargeCount;
    [Label("基础对象规格")]
    public float basicShadowScale;
    [Label("拾取半径")]
    public float collectRadius;
    [Label("发射半径")]
    public float shadowSetRadius;
    [Label("充能周期")]
    public float shadowChargeCD;
    [Label("滞留时长")]
    public float shadowStayingDuration;
    [Label("发射量")]
    public int missileCount;
    [Label("飞行速度")]
    public float flyingSpeed;
    [Label("发射物规格")]
    public float missileScale;
    [Label("玩家受伤卡顿时长")]
    public float getHurtFreezeTime;
    [Label("玩家受伤击退范围半径")]
    public float getHurtBeatRadius;
    [Label("玩家受伤击退敌人距离")]
    public float getHurtBeatEnemyDist;
    [Label("玩家受伤击退敌人速度")]
    public float getHurtBeatEnemySpeed;
    [Label("玩家受伤无敌时间")]
    public float getHurtInvincibleDuration;

    [Space(5)]
    [Header("旋刃")]
    [Label("刃长")]
    public float whirlLength_Whirl;
    [Label("旋刃一阶角速度")]
    public float whirlAngularVelocity1_Whirl;
    [Label("一阶刃割伤害")]
    public float whirlDamage1_Whirl;
    [Label("二阶刃割伤害")]
    public float whirlDamage2_Whirl;
    [Label("旋刃二阶角速度")]
    public float whirlAngularVelocity2_Whirl;

    [Space(5)]
    [Header("收割")]
    [Label("收回切割威力乘数")]
    public float reapDamageMultiplier_Reap;
    [Label("对象规格转威力乘数")]
    public float scaleDamageMultiplier_Reap;
    [Label("反射飞行距离")]
    public float reflectFlyingDist_Reap;
    [Label("等级乘数")]
    public float gradeMultiplier_Reap;

    [Space(5)]
    [Header("黑子")]
    [Label("召回伤害半径")]
    public float recallDamageRadius_Sunspot;
    [Label("召回威力乘数")]
    public float recallDamageMultiplier_Sunspot;
    [Label("推行距离")]
    public float pushDist_Sunspot;
    [Label("推行移速")]
    public float pushSpeed_Sunspot;
    [Label("推行威力乘数")]
    public float pushDamageMultiplier_Sunspot;
    [Label("停下伤害半径")]
    public float stopDamageRadius_Sunspot;
    [Label("停下威力乘数")]
    public float stopDamageMultiplier_Sunspot;
    [Label("爆炸种子滞留时长")]
    public float explodeSeedStayingTime_Sunspot;

    [Space(5)]
    [Header("反戈")]
    [Label("推击圆心角")]
    public float pushCentralAngle_TurnWeaponAround;
    [Label("推击半径")]
    public float pushRadius_TurnWeaponAround;
    [Label("推击击退距离")]
    public float pushBeatDist_TurnWeaponAround;
    [Label("推击击退时长")]
    public float pushBeatDuration_TurnWeaponAround;
    [Label("斩击圆心角")]
    public float chopCentralAngle_TurnWeaponAround;
    [Label("斩击半径")]
    public float chopRadius_TurnWeaponAround;
    [Label("斩击威力乘数")]
    public float chopDamageMultiplier_TurnWeaponAround;
    [Label("飞行速度转威力乘数")]
    public float flyingSpeedDamageMultiplier_TurnWeaponAround;
    [Label("扩张斩击圆心角")]
    public float expandChopCentralAngle_TurnWeaponAround;
    [Label("扩张斩击半径")]
    public float expandChopRadius_TurnWeaponAround;

    [Space(5)]
    [Header("噩焰")]
    [Label("燃烧时长")]
    public float burnDuration_NightmarishFlame;
    [Label("激发间隔")]
    public float damageInterval_NightmarishFlame;
    [Label("燃烧威力")]
    public float burnDamage_NightmarishFlame;
    [Label("爆燃半径")]
    public float burnRadius_NightmarishFlame;
    [Label("生命上限转比例伤害乘数")]
    public float hpDamageMulitiplier_NightmarishFlame;
    [Label("激发间隔缩减乘数")]
    public float damageIntervalMulitiplier_NightmarishFlame;

    [Space(5)]
    [Header("飞矢")]
    [Label("箭塔激发间隔")]
    public float arrowInterval_FlyingArrow;
    [Label("箭塔发射量一阶加成")]
    public int missileCountIncrease1_FlyingArrow;
    [Label("箭塔监视半径")]
    public float watchRadius_FlyingArrow;
    [Label("箭矢一阶伤害")]
    public float damage1_FlyingArrow;
    [Label("箭矢飞行速度")]
    public float flyingSpeed_FlyingArrow;
    [Label("箭矢射程")]
    public float shootRange_FlyingArrow;
    [Label("箭矢二阶伤害")]
    public float damage2_FlyingArrow;
    [Label("箭塔发射量二阶加成")]
    public int missileCountIncrease2_FlyingArrow;
    [Label("箭塔激发间隔上限")]
    public float arrowIntervalUpperLimit_FlyingArrow;
    [Label("箭塔激发间隔下限")]
    public float arrowIntervalLowerLimit_FlyingArrow;

    [Space(5)]
    [Header("汹浪")]
    [Label("波塔一阶激发间隔")]
    public float waveInterval1_RoughSea;
    [Label("波塔监视半径")]
    public float watchRadius_RoughSea;
    [Label("波穿透数量")]
    public int penetrateCount_RoughSea;
    [Label("波一阶伤害")]
    public float damage1_RoughSea;
    [Label("波飞行速度")]
    public float flyingSpeed_RoughSea;
    [Label("波射程")]
    public float shootRange_RoughSea;
    [Label("波二阶伤害")]
    public float damage2_RoughSea;
    [Label("波塔二阶激发间隔")]
    public float waveInterval2_RoughSea;

    [Space(5)]
    [Header("推进")]
    [Label("发射切割威力乘数")]
    public float damageMultiplier_PushOn;
    [Label("发射击退生效次数")]
    public int beatCount_PushOn;
    [Label("发射击退距离")]
    public float beatDist_PushOn;
    [Label("发射击退时长")]
    public float beatDuration_PushOn;
    [Label("特殊发射冷却")]
    public float missileChargeCD_PushOn;
    [Label("拼接数")]
    public int missileJoinCount_PushOn;
    [Label("拼接距离")]
    public float missileJoinDist_PushOn;
    [Label("特殊发射物威力乘数")]
    public float missileDamageMultiplier_PushOn;
    [Label("特殊发射物滞留时长")]
    public float missileStayingDuration_PushOn;

    [Space(5)]
    [Header("聚集")]
    [Label("充能周期缩减")]
    public float chargeCDDecrease_Gather;
    [Label("任务充能次数")]
    public int needChargedCount_Gather;
    [Label("充能量一阶加成")]
    public int maxChargeCountIncrease1_Gather;
    [Label("预充能周期")]
    public float prechargeDuration_Gather;
    [Label("预充能上限")]
    public float maxPrecharge_Gather;
    [Label("充能量二阶加成")]
    public int maxChargeCountIncrease2_Gather;

    [Space(5)]
    [Header("喧哗")]
    [Label("滞留时长一阶加成")]
    public float stayingDurationIncrease1_ConfusedNoise;
    [Label("减速半径")]
    public float decelerateRadius_ConfusedNoise;
    [Label("减速乘数")]
    public float decelerateMultiplier_ConfusedNoise;
    [Label("滞留时长二阶加成")]
    public float stayingDurationIncrease2_ConfusedNoise;

    [Space(5)]
    [Header("实像")]
    [Label("基础威力一阶加成")]
    public float basicDamageIncrease1_RealImage;
    [Label("基础威力二阶加成")]
    public float basicDamageIncrease2_RealImage;
    [Label("基础威力乘数一阶加成")]
    public float basicDamageMultiplierIncrease1_RealImage;
    [Label("基础威力乘数二阶加成")]
    public float basicDamageMultiplierIncrease2_RealImage;

    [Space(5)]
    [Header("幽灵")]
    [Label("滞留时长一阶缩减")]
    public float stayingDurationDecrease1_Ghost;
    [Label("飞行速度一阶加成")]
    public float flyingSpeedIncrease1_Ghost;
    [Label("滞留时长二阶缩减")]
    public float stayingDurationDecrease2_Ghost;
    [Label("飞行速度二阶加成")]
    public float flyingSpeedIncrease2_Ghost;

    [Space(5)]
    [Header("无羁")]
    [Label("对象规格加成")]
    public float scaleIncrease_Unbridled;
    [Label("发射半径加成")]
    public float setRadiusIncrease_Unbridled;
    [Label("冲撞眩晕时长")]
    public float vertigoDuration_Unbridled;
    [Label("飞行距离转对象规格乘数")]
    public float flyingDistScaleMultiplier_Unbridled;
    [Label("对象规格上限")]
    public float scaleUpperLimit_Unbridled;

    [Space(5)]
    [Header("克隆")]
    [Label("发射量一阶加成")]
    public int missileCountIncrease1_Clone;
    [Label("发射物规格加成")]
    public float missileScaleIncrease_Clone;
    [Label("发射量二阶加成")]
    public int missileCountIncrease2_Clone;
    [Label("复制概率")]
    public float cloneProbability_Clone;

    [Space(5)]
    [Header("新生")]
    [Label("生命上限加成")]
    public int maxHPIncrease_Reborn;
    [Label("生命回复周期")]
    public float healDuration_Reborn;
    [Label("回复所需击杀量")]
    public int healKillCount_Reborn;

    [Space(5)]
    [Header("海纳")]
    [Label("拾取半径一阶加成")]
    public float collectRadiusIncrease1_AllRiversRunIntoSea;
    [Label("拾取半径二阶加成")]
    public float collectRadiusIncrease2_AllRiversRunIntoSea;
    [Label("移速加成")]
    public float speedIncrease_AllRiversRunIntoSea;
    [Label("拾取冷却时间")]
    public float collectCD_AllRiversRunIntoSea;
    [Label("吸收半径")]
    public float absorbRadius_AllRiversRunIntoSea;
}
