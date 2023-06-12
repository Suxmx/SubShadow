[System.Serializable]
public class EllipseEnemyGroupInfo
{
    [Label("特殊敌人索引")]
    public int index;
    [Label("敌群初始长半轴")]
    public float semiLength;
    [Label("敌群圈数")]
    public float circleCount;
    [Label("敌群圈间隔")]
    public float circleSpacing;
    [Label("敌群速度")]
    public float moveSpeed;
    [Label("敌群生成周期")]
    public float spawnInterval;
    [Label("敌群经验倍率")]
    public int expMultiplier = 1;
}
