[System.Serializable]
public class SelfExplosionEnemyInfo
{
    /// <summary>
    /// 探测距离
    /// </summary>
    [Label("探测距离")]
    public float explodeDist;
    /// <summary>
    /// 爆炸范围
    /// </summary>
    [Label("爆炸范围")]
    public float explosionRadius;
    /// <summary>
    /// 爆炸前时间
    /// </summary>
    [Label("爆炸前时间")]
    public float explosionDuration;
    /// <summary>
    /// 爆炸对玩家的伤害
    /// </summary>
    [Label("爆炸对玩家的伤害")]
    public float damageToPlayer;
    /// <summary>
    /// 爆炸对怪物的伤害
    /// </summary>
    [Label("爆炸对怪物的伤害")]
    public float damageToEnemy;
}
