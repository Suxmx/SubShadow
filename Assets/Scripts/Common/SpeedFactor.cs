using MyTimer;

public class SpeedFactor
{
    /// <summary>
    /// ����Ӱ����ӣ����㷽ʽΪspeed*(1+multiplier)
    /// ����Ч��Ϊ��������Ч��Ϊ��
    /// </summary>
    public float multiplier;

    /// <summary>
    /// ��������SpeedMultiplier
    /// </summary>
    protected CharacterSpeedMultiplier speedMultiplier;

    /// <summary>
    /// �Ƿ��ǹ̶�ʱ��
    /// ��Ϊtrue�������Ӱ��ֻ�����̶�ʱ��
    /// ����һֱ����ֱ���ֶ��ر�
    /// </summary>
    public bool FixedTime { get; protected set; }

    /// <summary>
    /// ��Ϊ�̶�ʱ��ʱ�ü�ʱ�����ڵ���ʱ
    /// </summary>
    protected CountdownTimer lastingTimer;

    /// <summary>
    /// ��Ϊ����Ӱ��ʱΪ������������ֹ���ӣ�
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
    /// �Ƿ���Ч
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
