/// <summary>
/// 来自深渊数据
/// </summary>
public class TentacleInfo
{
    /// <summary>
    /// 一阶触须攻击间隔
    /// </summary>
    public float attackInterval;
    /// <summary>
    /// 一阶触须攻击距离
    /// </summary>
    public float attackDist;
    /// <summary>
    /// 每次攻击数目
    /// </summary>
    public int attackNum;
    /// <summary>
    /// 攻击怪物时基于影子伤害的倍率
    /// </summary>
    public float damageToEnemyMultiplier;
    /// <summary>
    /// 攻击玩家时造成的伤害
    /// </summary>
    public float damageToPlayer;
    /// <summary>
    /// 二阶时触须攻击间隔
    /// </summary>
    public float shorterAttackInterval;
    /// <summary>
    /// 三阶时缩短攻击间隔所需攻击的次数
    /// </summary>
    public int attackNumToDecreaseAttackInterval;
    /// <summary>
    /// 三阶攻击一定次数缩短攻击间隔的倍率
    /// </summary>
    public float decreaseAttackIntervalMultiplier;
    /// <summary>
    /// 三阶影子最低攻击间隔
    /// </summary>
    public float minAttackInterval;
    /// <summary>
    /// 四阶眩晕普通怪物的概率
    /// </summary>
    public float vertigoProbability_NormalEnemy;
    /// <summary>
    /// 四阶眩晕BOSS或精英敌人的概率
    /// </summary>
    public float vertigoProbability_Boss;
    /// <summary>
    /// 四阶眩晕时间
    /// </summary>
    public float vertigoTime;
    /// <summary>
    /// 尺寸
    /// </summary>
    public float size;
}
