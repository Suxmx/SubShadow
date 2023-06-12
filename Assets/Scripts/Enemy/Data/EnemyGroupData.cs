using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupData : ScriptableObject
{
    [Header("��Ⱥ������Ϣ��")]
    public List<EnemyGroupInfo> enemyGroupInfos;
    [Header("��Ⱥ���úϼ�")]
    public List<EnemyGroupConfigInfo> enemyGroupConfigInfos;

    [Header("�����Ⱥ������Ϣ��")]
    public List<OrbitEnemyGroupInfo> orbitEnemyGroupInfos;
    [Header("�����Ⱥ���úϼ�")]
    public List<EnemyGroupConfigInfo> orbitEnemyGroupConfigInfos;

    [Header("��Բ��Ⱥ������Ϣ��")]
    public List<EllipseEnemyGroupInfo> ellipseEnemyGroupInfos;
    [Header("��Բ��Ⱥ���úϼ�")]
    public List<EnemyGroupConfigInfo> ellipseEnemyGroupConfigInfos;

    [Header("������Ⱥ������Ϣ��")]
    public List<CorkscrewEnemyGroupInfo> corkscrewEnemyGroupInfos;
    [Header("������Ⱥ���úϼ�")]
    public List<EnemyGroupConfigInfo> corkscrewEnemyGroupConfigInfos;

    [Space(5)]
    [Label("II�����I��İ볤����")]
    public float generateZoneIncreasement;
    [Label("���������II��İ볤����")]
    public float bufferZoneIncreasement;

    [Space(5)]
    [Label("��������������")]
    public int maxEnemyCount;
    [Label("I����Ұ�ȫ�뾶")]
    public float playerSafeRadius;
}
