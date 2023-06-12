public class FireInfo
{
    /// <summary>
    /// 燃烧持续总时间
    /// </summary>
    public float fireDuration;
    /// <summary>
    /// 燃烧判定间隔
    /// </summary>
    public float judgeInterval;
    /// <summary>
    /// 燃烧单次判定伤害
    /// </summary>
    public float damage;
    /// <summary>
    /// 生命上限转比例伤害乘数
    /// </summary>
    public float hpDamageMulitiplier;
    public PlayerStatusInfo playerStatusInfo;
    public float HPDamageMultiplier => hpDamageMulitiplier * playerStatusInfo.MaxHP;
}
