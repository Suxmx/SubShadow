using System.Collections.Generic;
using UnityEngine;

public class EnemyInfoData : ScriptableObject
{
    [Header("����������Ϣ��")]
    public List<EnemyInfo> enemyInfos;
    [Header("�������������Ϣ��")]
    public List<SpecialEnemyInfo> specialEnemyInfos;

    [Space(10)]
    [Header("���˻�����Ϣ")]
    [Label("�������˱����˾���")]
    public float hitBeatDist;
    [Label("�������˱����˾���")]
    public float hitBeatSpeed;

    [Space(5)]
    [Label("���۹���Ϣ")]
    public BigEyeInfo bigEyeInfo;
    [Space(5)]
    [Label("�Ա�����Ϣ")]
    public SelfExplosionEnemyInfo selfExplosionEnemyInfo;
}
