using System.Collections.Generic;
using UnityEngine;

public class SkillInfoData : ScriptableObject
{
    [Header("����������Ϣ")]
    public List<SkillInfo> attackSkillInfos;
    [Header("����������Ϣ")]
    public List<SkillInfo> assistantSkillInfos;

    [Space(5)]
    [Header("ϵͳ���Գ�ʼֵ")]
    [Label("��������")]
    public float basicDamage;
    [Label("��������")]
    public int playerMaxHP;
    [Label("����")]
    public float playerSpeed;
    [Label("������")]
    public int shadowChargeCount;
    [Label("����������")]
    public float basicShadowScale;
    [Label("ʰȡ�뾶")]
    public float collectRadius;
    [Label("����뾶")]
    public float shadowSetRadius;
    [Label("��������")]
    public float shadowChargeCD;
    [Label("����ʱ��")]
    public float shadowStayingDuration;
    [Label("������")]
    public int missileCount;
    [Label("�����ٶ�")]
    public float flyingSpeed;
    [Label("��������")]
    public float missileScale;
    [Label("������˿���ʱ��")]
    public float getHurtFreezeTime;
    [Label("������˻��˷�Χ�뾶")]
    public float getHurtBeatRadius;
    [Label("������˻��˵��˾���")]
    public float getHurtBeatEnemyDist;
    [Label("������˻��˵����ٶ�")]
    public float getHurtBeatEnemySpeed;
    [Label("��������޵�ʱ��")]
    public float getHurtInvincibleDuration;

    [Space(5)]
    [Header("����")]
    [Label("�г�")]
    public float whirlLength_Whirl;
    [Label("����һ�׽��ٶ�")]
    public float whirlAngularVelocity1_Whirl;
    [Label("һ���и��˺�")]
    public float whirlDamage1_Whirl;
    [Label("�����и��˺�")]
    public float whirlDamage2_Whirl;
    [Label("���ж��׽��ٶ�")]
    public float whirlAngularVelocity2_Whirl;

    [Space(5)]
    [Header("�ո�")]
    [Label("�ջ��и���������")]
    public float reapDamageMultiplier_Reap;
    [Label("������ת��������")]
    public float scaleDamageMultiplier_Reap;
    [Label("������о���")]
    public float reflectFlyingDist_Reap;
    [Label("�ȼ�����")]
    public float gradeMultiplier_Reap;

    [Space(5)]
    [Header("����")]
    [Label("�ٻ��˺��뾶")]
    public float recallDamageRadius_Sunspot;
    [Label("�ٻ���������")]
    public float recallDamageMultiplier_Sunspot;
    [Label("���о���")]
    public float pushDist_Sunspot;
    [Label("��������")]
    public float pushSpeed_Sunspot;
    [Label("������������")]
    public float pushDamageMultiplier_Sunspot;
    [Label("ͣ���˺��뾶")]
    public float stopDamageRadius_Sunspot;
    [Label("ͣ����������")]
    public float stopDamageMultiplier_Sunspot;
    [Label("��ը��������ʱ��")]
    public float explodeSeedStayingTime_Sunspot;

    [Space(5)]
    [Header("����")]
    [Label("�ƻ�Բ�Ľ�")]
    public float pushCentralAngle_TurnWeaponAround;
    [Label("�ƻ��뾶")]
    public float pushRadius_TurnWeaponAround;
    [Label("�ƻ����˾���")]
    public float pushBeatDist_TurnWeaponAround;
    [Label("�ƻ�����ʱ��")]
    public float pushBeatDuration_TurnWeaponAround;
    [Label("ն��Բ�Ľ�")]
    public float chopCentralAngle_TurnWeaponAround;
    [Label("ն���뾶")]
    public float chopRadius_TurnWeaponAround;
    [Label("ն����������")]
    public float chopDamageMultiplier_TurnWeaponAround;
    [Label("�����ٶ�ת��������")]
    public float flyingSpeedDamageMultiplier_TurnWeaponAround;
    [Label("����ն��Բ�Ľ�")]
    public float expandChopCentralAngle_TurnWeaponAround;
    [Label("����ն���뾶")]
    public float expandChopRadius_TurnWeaponAround;

    [Space(5)]
    [Header("ج��")]
    [Label("ȼ��ʱ��")]
    public float burnDuration_NightmarishFlame;
    [Label("�������")]
    public float damageInterval_NightmarishFlame;
    [Label("ȼ������")]
    public float burnDamage_NightmarishFlame;
    [Label("��ȼ�뾶")]
    public float burnRadius_NightmarishFlame;
    [Label("��������ת�����˺�����")]
    public float hpDamageMulitiplier_NightmarishFlame;
    [Label("���������������")]
    public float damageIntervalMulitiplier_NightmarishFlame;

    [Space(5)]
    [Header("��ʸ")]
    [Label("�����������")]
    public float arrowInterval_FlyingArrow;
    [Label("����������һ�׼ӳ�")]
    public int missileCountIncrease1_FlyingArrow;
    [Label("�������Ӱ뾶")]
    public float watchRadius_FlyingArrow;
    [Label("��ʸһ���˺�")]
    public float damage1_FlyingArrow;
    [Label("��ʸ�����ٶ�")]
    public float flyingSpeed_FlyingArrow;
    [Label("��ʸ���")]
    public float shootRange_FlyingArrow;
    [Label("��ʸ�����˺�")]
    public float damage2_FlyingArrow;
    [Label("�������������׼ӳ�")]
    public int missileCountIncrease2_FlyingArrow;
    [Label("���������������")]
    public float arrowIntervalUpperLimit_FlyingArrow;
    [Label("���������������")]
    public float arrowIntervalLowerLimit_FlyingArrow;

    [Space(5)]
    [Header("����")]
    [Label("����һ�׼������")]
    public float waveInterval1_RoughSea;
    [Label("�������Ӱ뾶")]
    public float watchRadius_RoughSea;
    [Label("����͸����")]
    public int penetrateCount_RoughSea;
    [Label("��һ���˺�")]
    public float damage1_RoughSea;
    [Label("�������ٶ�")]
    public float flyingSpeed_RoughSea;
    [Label("�����")]
    public float shootRange_RoughSea;
    [Label("�������˺�")]
    public float damage2_RoughSea;
    [Label("�������׼������")]
    public float waveInterval2_RoughSea;

    [Space(5)]
    [Header("�ƽ�")]
    [Label("�����и���������")]
    public float damageMultiplier_PushOn;
    [Label("���������Ч����")]
    public int beatCount_PushOn;
    [Label("������˾���")]
    public float beatDist_PushOn;
    [Label("�������ʱ��")]
    public float beatDuration_PushOn;
    [Label("���ⷢ����ȴ")]
    public float missileChargeCD_PushOn;
    [Label("ƴ����")]
    public int missileJoinCount_PushOn;
    [Label("ƴ�Ӿ���")]
    public float missileJoinDist_PushOn;
    [Label("���ⷢ������������")]
    public float missileDamageMultiplier_PushOn;
    [Label("���ⷢ��������ʱ��")]
    public float missileStayingDuration_PushOn;

    [Space(5)]
    [Header("�ۼ�")]
    [Label("������������")]
    public float chargeCDDecrease_Gather;
    [Label("������ܴ���")]
    public int needChargedCount_Gather;
    [Label("������һ�׼ӳ�")]
    public int maxChargeCountIncrease1_Gather;
    [Label("Ԥ��������")]
    public float prechargeDuration_Gather;
    [Label("Ԥ��������")]
    public float maxPrecharge_Gather;
    [Label("���������׼ӳ�")]
    public int maxChargeCountIncrease2_Gather;

    [Space(5)]
    [Header("����")]
    [Label("����ʱ��һ�׼ӳ�")]
    public float stayingDurationIncrease1_ConfusedNoise;
    [Label("���ٰ뾶")]
    public float decelerateRadius_ConfusedNoise;
    [Label("���ٳ���")]
    public float decelerateMultiplier_ConfusedNoise;
    [Label("����ʱ�����׼ӳ�")]
    public float stayingDurationIncrease2_ConfusedNoise;

    [Space(5)]
    [Header("ʵ��")]
    [Label("��������һ�׼ӳ�")]
    public float basicDamageIncrease1_RealImage;
    [Label("�����������׼ӳ�")]
    public float basicDamageIncrease2_RealImage;
    [Label("������������һ�׼ӳ�")]
    public float basicDamageMultiplierIncrease1_RealImage;
    [Label("���������������׼ӳ�")]
    public float basicDamageMultiplierIncrease2_RealImage;

    [Space(5)]
    [Header("����")]
    [Label("����ʱ��һ������")]
    public float stayingDurationDecrease1_Ghost;
    [Label("�����ٶ�һ�׼ӳ�")]
    public float flyingSpeedIncrease1_Ghost;
    [Label("����ʱ����������")]
    public float stayingDurationDecrease2_Ghost;
    [Label("�����ٶȶ��׼ӳ�")]
    public float flyingSpeedIncrease2_Ghost;

    [Space(5)]
    [Header("���")]
    [Label("������ӳ�")]
    public float scaleIncrease_Unbridled;
    [Label("����뾶�ӳ�")]
    public float setRadiusIncrease_Unbridled;
    [Label("��ײѣ��ʱ��")]
    public float vertigoDuration_Unbridled;
    [Label("���о���ת���������")]
    public float flyingDistScaleMultiplier_Unbridled;
    [Label("����������")]
    public float scaleUpperLimit_Unbridled;

    [Space(5)]
    [Header("��¡")]
    [Label("������һ�׼ӳ�")]
    public int missileCountIncrease1_Clone;
    [Label("��������ӳ�")]
    public float missileScaleIncrease_Clone;
    [Label("���������׼ӳ�")]
    public int missileCountIncrease2_Clone;
    [Label("���Ƹ���")]
    public float cloneProbability_Clone;

    [Space(5)]
    [Header("����")]
    [Label("�������޼ӳ�")]
    public int maxHPIncrease_Reborn;
    [Label("�����ظ�����")]
    public float healDuration_Reborn;
    [Label("�ظ������ɱ��")]
    public int healKillCount_Reborn;

    [Space(5)]
    [Header("����")]
    [Label("ʰȡ�뾶һ�׼ӳ�")]
    public float collectRadiusIncrease1_AllRiversRunIntoSea;
    [Label("ʰȡ�뾶���׼ӳ�")]
    public float collectRadiusIncrease2_AllRiversRunIntoSea;
    [Label("���ټӳ�")]
    public float speedIncrease_AllRiversRunIntoSea;
    [Label("ʰȡ��ȴʱ��")]
    public float collectCD_AllRiversRunIntoSea;
    [Label("���հ뾶")]
    public float absorbRadius_AllRiversRunIntoSea;
}
