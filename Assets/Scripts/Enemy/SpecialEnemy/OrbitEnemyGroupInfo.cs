[System.Serializable]
public class OrbitEnemyGroupInfo
{
    [Label("特殊敌人索引")]
    public int index;
    [Label("敌群行数")]
    public int rowCount;
    [Label("敌群列数")]
    public int columnCount;
    [Label("敌群行间隔")]
    public float rowSpacing;
    [Label("敌群列间隔")]
    public float columnSpacing;
    [Label("敌群速度")]
    public float moveSpeed;
    [Label("敌群生成周期")]
    public float spawnInterval;
    [Label("敌群经验倍率")]
    public int expMultiplier = 1;
}
