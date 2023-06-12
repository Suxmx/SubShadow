[System.Serializable]
public class EnemyGroupConfigInfo
{
    [Label("开始时间")]
    public float startMoment;
    [Label("结束时间")]
    public float endMoment;
    [Label("敌群索引")]
    public int groupInfoIndex;
    [Label("配置开启")]
    public bool configOn;
}
