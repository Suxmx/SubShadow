public class ShadowLauncherInfo
{
    /// <summary>
    /// 伤害
    /// </summary>
    public float attackDamage;
    /// <summary>
    /// 攻击间隔
    /// </summary>
    public float attackInterval;
    /// <summary>
    /// 探测距离
    /// </summary>
    public float attackDist;
    /// <summary>
    /// 子弹速度
    /// </summary>
    public float bulletSpeed;
    /// <summary>
    /// 最大射程
    /// </summary>
    public float maxFlyingDist;
    /// <summary>
    /// 发射量
    /// </summary>
    public int missileCount;
    /// <summary>
    /// 发射量规格
    /// </summary>
    public float missileScale;

    public ShadowLauncherInfo(float attackDamage, float attackInterval, float attackDist, 
        float bulletSpeed, float maxFlyingDist)
    {
        this.attackDamage = attackDamage;
        this.attackInterval = attackInterval;
        this.attackDist = attackDist;
        this.bulletSpeed = bulletSpeed;
        this.maxFlyingDist = maxFlyingDist;
    }
}
