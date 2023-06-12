using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyGroupInfo
{
    [Header("����������")]
    public List<int> enemyIndexList;
    [Header("ƫ��������")]
    public List<Vector2> enemyOffsets;
    [Label("��Ⱥ��������")]
    public float spawnInterval;
    [Label("������������")]
    public int spawnCount = 1;
    [Label("��Ⱥ������")]
    public EnemySpawnZone spawnZone;
    [Label("��Ⱥ���鱶��")]
    public int expMultiplier = 1;
}
