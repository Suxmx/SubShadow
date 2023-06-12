[System.Serializable]
public class CorkscrewEnemyGroupInfo
{
    [Label("特殊敌人索引")]
    public int index;
    [Label("敌群总角度")]
    public float totalAngle;
    [Label("敌群初始距离")]
    public float originalDist;
    [Label("敌群最终距离")]
    public float finalDist;
    [Label("敌群圈间隔")]
    public float circleSpacing;
    [Label("敌群径向速度")]
    public float radialSpeed;
    [Label("敌群角速度")]
    public float angularSpeed;
    [Label("顺时针旋转")]
    public bool clockwise;
    [Label("敌群生成周期")]
    public float spawnInterval;
    [Label("敌群经验倍率")]
    public int expMultiplier = 1;
}
