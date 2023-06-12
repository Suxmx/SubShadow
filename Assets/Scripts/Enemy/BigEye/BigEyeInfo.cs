[System.Serializable]
public class BigEyeInfo
{
    /// <summary>
    /// 保持距离
    /// </summary>
    [Label("保持距离")]
    public float keepDist;
    /// <summary>
    /// 射击间隔
    /// </summary>
    [Label("射击间隔")]
    public float shootInterval;
    /// <summary>
    /// 射击时的僵直时间
    /// </summary>
    [Label("射击时的僵直时间")]
    public float stillDurationWhileShooting;
    /// <summary>
    /// 静止区间半宽
    /// </summary>
    [Label("静止区间半宽")]
    public float stillDist;
    /// <summary>
    /// 子弹伤害
    /// </summary>
    [Label("子弹伤害")]
    public float bulletDamage;
    /// <summary>
    /// 子弹速度
    /// </summary>
    [Label("子弹速度")]
    public float bulletSpeed;
    /// <summary>
    /// 子弹最长射程
    /// </summary>
    [Label("子弹最长射程")]
    public float maxFlyingDist;
}
