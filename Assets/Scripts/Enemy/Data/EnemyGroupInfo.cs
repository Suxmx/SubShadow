using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyGroupInfo
{
    [Header("敌人索引表")]
    public List<int> enemyIndexList;
    [Header("偏移向量表")]
    public List<Vector2> enemyOffsets;
    [Label("敌群生成周期")]
    public float spawnInterval;
    [Label("单次生成数量")]
    public int spawnCount = 1;
    [Label("敌群生成区")]
    public EnemySpawnZone spawnZone;
    [Label("敌群经验倍率")]
    public int expMultiplier = 1;
}
