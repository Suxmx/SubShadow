using MyTimer;

public class SpeedFactor
{
    /// <summary>
    /// 移速影响乘子，计算方式为speed*(1+multiplier)
    /// 加速效果为正，减速效果为负
    /// </summary>
    public float multiplier;

    /// <summary>
    /// 所依赖的SpeedMultiplier
    /// </summary>
    protected CharacterSpeedMultiplier speedMultiplier;

    /// <summary>
    /// 是否是固定时间
    /// 若为true则该移速影响只持续固定时间
    /// 否则将一直存在直至手动关闭
    /// </summary>
    public bool FixedTime { get; protected set; }

    /// <summary>
    /// 当为固定时间时该计时器用于倒计时
    /// </summary>
    protected CountdownTimer lastingTimer;

    /// <summary>
    /// 当为持续影响时为因子数量（防止叠加）
    /// </summary>
    protected int factorCount;
    public int FactorCount
    {
        get => factorCount;
        set
        {
            factorCount = value;
            Effective = factorCount > 0;
        }
    }

    /// <summary>
    /// 是否生效
    /// </summary>
    protected bool effective;
    public virtual bool Effective
    {
        get => effective;
        set
        {
            if (effective != value)
            {
                effective = value;
                speedMultiplier.ReCalculateMultiplier();
            }
            if (FixedTime)
            {
                if (effective) lastingTimer.Restart();
                else lastingTimer.Paused = true;
            }
            else
            {
                if (!effective) factorCount = 0;
            }
        }
    }

    public SpeedFactor(CharacterSpeedMultiplier speedMultiplier)
    {
        multiplier = 1f;
        this.speedMultiplier = speedMultiplier;
        effective = false;
    }

    public SpeedFactor(float multiplier, CharacterSpeedMultiplier speedMultiplier) : this(speedMultiplier)
    {
        this.multiplier = multiplier;
        FixedTime = false;
        factorCount = 0;
    }

    public SpeedFactor(float multiplier, float lastingTime, CharacterSpeedMultiplier speedMultiplier) : this(speedMultiplier)
    {
        this.multiplier = multiplier;
        FixedTime = true;
        lastingTimer = new CountdownTimer();
        lastingTimer.OnComplete += () => Effective = false;
        lastingTimer.Initialize(lastingTime, false);
    }
}
