using System.Collections.Generic;
using UnityEngine;

public class EnemyInfoData : ScriptableObject
{
    [Header("敌人描述信息表")]
    public List<EnemyInfo> enemyInfos;
    [Header("特殊敌人描述信息表")]
    public List<SpecialEnemyInfo> specialEnemyInfos;

    [Space(10)]
    [Header("敌人基础信息")]
    [Label("敌人受伤被击退距离")]
    public float hitBeatDist;
    [Label("敌人受伤被击退均速")]
    public float hitBeatSpeed;

    [Space(5)]
    [Label("大眼怪信息")]
    public BigEyeInfo bigEyeInfo;
    [Space(5)]
    [Label("自爆怪信息")]
    public SelfExplosionEnemyInfo selfExplosionEnemyInfo;
}
