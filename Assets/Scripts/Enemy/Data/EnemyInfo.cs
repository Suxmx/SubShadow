[System.Serializable]
public class EnemyInfo
{
    [Label("敌人类别")]
    public EnemyType enemyType;
    [Label("敌人血量")]
    public float HP;
    [Label("敌人追踪移速")]
    public float followSpeed;
    [Label("敌人经验掉落")]
    public int dropEXP = 1;
    [Label("是精英敌人")]
    public bool isBoss;
}
