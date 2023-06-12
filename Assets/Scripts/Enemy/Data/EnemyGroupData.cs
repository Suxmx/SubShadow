using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupData : ScriptableObject
{
    [Header("敌群描述信息表")]
    public List<EnemyGroupInfo> enemyGroupInfos;
    [Header("敌群配置合集")]
    public List<EnemyGroupConfigInfo> enemyGroupConfigInfos;

    [Header("轨道敌群描述信息表")]
    public List<OrbitEnemyGroupInfo> orbitEnemyGroupInfos;
    [Header("轨道敌群配置合集")]
    public List<EnemyGroupConfigInfo> orbitEnemyGroupConfigInfos;

    [Header("椭圆敌群描述信息表")]
    public List<EllipseEnemyGroupInfo> ellipseEnemyGroupInfos;
    [Header("椭圆敌群配置合集")]
    public List<EnemyGroupConfigInfo> ellipseEnemyGroupConfigInfos;

    [Header("螺旋敌群描述信息表")]
    public List<CorkscrewEnemyGroupInfo> corkscrewEnemyGroupInfos;
    [Header("螺旋敌群配置合集")]
    public List<EnemyGroupConfigInfo> corkscrewEnemyGroupConfigInfos;

    [Space(5)]
    [Label("II域相对I域的半长增量")]
    public float generateZoneIncreasement;
    [Label("缓冲域相对II域的半长增量")]
    public float bufferZoneIncreasement;

    [Space(5)]
    [Label("敌人最多存在数量")]
    public int maxEnemyCount;
    [Label("I域玩家安全半径")]
    public float playerSafeRadius;
}
